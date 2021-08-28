using UnityEngine;
using TMPro;
using Proyecto26;
using System.Text;
using System.Security.Cryptography;

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
            RequestToBackEnd(url, dataString, "POST");
        }
        else
        {
            createUserMessage.text = "Por favor, ingresa usuario y contraseña.";
        }
    }

    /// <summary>
    /// Using the Proyecto26 package to easily manage the sending
    /// and receiving of JSON bodies instead of forms. Made in a reusable function
    /// shape.
    /// </summary>
    /// <param name="url">url to hit with the request.</param>
    /// <param name="body">The body (if any) for the request.</param>
    /// <param name="method">The method (For this project, only POST and GET are necessary).</param>
    public void RequestToBackEnd(string url, string body, string method)
    {
        RestClient.Request(new RequestHelper
        {
            Uri = url,
            Method = method,
            BodyRaw = Encoding.UTF8.GetBytes(body),
            ContentType = "application/json",
            Retries = 3,
            RetrySecondsDelay = 2
        }).Then(res =>
        {
            #if UNITY_EDITOR && DEBUG
            Debug.Log($"Response: {res.Text}");
            #endif
            createUserMessage.text = $"{res.Text}";
        })
        .Catch(err =>
        {
            if (err.Message == "HTTP/1.1 400 Unauthorized")
            {
                createUserMessage.text = "Ingresa usuario Y contraseña, por favor.";
            }
            else if (err.Message == "HTTP/1.1 403 Forbidden")
            {
                createUserMessage.text = "Usuario ya existente.";
            }
            #if UNITY_EDITOR && DEBUG
            Debug.LogWarning($"Error: {err.Message}");
            #endif
        })
        .Done(() =>
        {
            #if UNITY_EDITOR && DEBUG
            Debug.Log("Update Result");
            #endif
        });
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
