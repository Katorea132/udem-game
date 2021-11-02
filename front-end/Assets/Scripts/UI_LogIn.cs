using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// Reigns the log in logic.
/// </summary>
public class UI_LogIn : MonoBehaviour
{

    [System.Serializable]
    private struct logInData
    {
        public string username;
        public string password;
    }

    private logInData data;
    public TextMeshProUGUI logInMessage;


    /// <summary>
    /// Activated on click.
    /// </summary>
    public void LogIn()
    {
        string url = "http://127.0.0.1:5000/login";
        string dataString = JsonUtility.ToJson(data);
        if (data.username != null && data.password != null)
        {
            StartCoroutine(Request_Coroutine(url, dataString, "POST"));
        }
        else
        {
            logInMessage.text = "Por favor, ingresa usuario y contraseña.";
        }
    }

    /// <summary>
    /// Request sender
    /// </summary>
    /// <param name="url">URL to send request to</param>
    /// <param name="bodyJsonString">Body in JSON format to send</param>
    /// <param name="method">method for request</param>
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
            TokenCreator();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (request.responseCode == 400)
        {
            logInMessage.text = "Ingresa usuario Y contraseña, por favor.";
        }
        else if (request.responseCode == 401)
        {
            logInMessage.text = "Usuario o contraseña equivocada.";
        }
        else
        {
            logInMessage.text = request.error;
        }
    }

    private void TokenCreator()
    { 
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{data.password}"));
            string digestedPassword = System.Convert.ToBase64String(bytes);
            bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{data.username}{digestedPassword}"));
            DataManager.instance.token = System.Convert.ToBase64String(bytes);
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
