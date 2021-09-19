using UnityEngine;
using TMPro;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine.Networking;

public class UI_CreateUser : MonoBehaviour
{
    [System.Serializable]
    private struct CreateUserData
    {
        public string username;
        public string password;
    }

    private CreateUserData data;
    public TextMeshProUGUI createUserMessage;


    /// <summary>
    /// Activated on click.
    /// </summary>
    public void CreateUser()
    {
        string url = "http://127.0.0.1:5000/create_user";
        string dataString = JsonUtility.ToJson(data);
        if (data.username != null && data.password != null)
        {
            StartCoroutine(Request_Coroutine(url, dataString, "POST"));
        }
        else
        {
            createUserMessage.text = "Por favor, ingresa usuario y contraseña.";
        }
    }

    /// <summary>
    /// IEnumerator for a custom request coroutine
    /// </summary>
    /// <param name="url">url to request</param>
    /// <param name="bodyJsonString"> body in JSON format</param>
    /// <param name="method">which method to use</param>
    /// <returns></returns>
    IEnumerator Request_Coroutine(string url, string bodyJsonString, string method)
    {
        var request = new UnityWebRequest(url, method);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        if (request.responseCode == 200)
        {
            createUserMessage.text = "Creación exitosa";
        }
        else if (request.responseCode == 400)
        {
            createUserMessage.text = "Ingresa usuario Y contraseña, por favor.";
        }
        else if (request.responseCode == 403)
        {
            createUserMessage.text = "Usuario ya existe.";
        }
        else
        {
            createUserMessage.text = request.error;
        }
    }

    /// <summary>
    /// Reads the username from the input field.
    /// </summary>
    /// <param name="user">Username, plain text.</param>
    public void ReadUsername(string user)
    {
        if (user != "")
        {
            data.username = user;
        }
        else
        {
            data.username = null;
        }
    }

    /// <summary>
    /// Reads the password from the input field, hashes it with SHA256
    /// and encodes in base64.
    /// </summary>
    /// <param name="passw">Password, plain text.</param>
    public void ReadPassword(string passw)
    {
        if (passw != "")
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Hashes the password
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(passw));
                // Convert byte array to a base64 string
                data.password = System.Convert.ToBase64String(bytes);
            }
        }
        else
        {
            data.password = null;
        }
    }
}
