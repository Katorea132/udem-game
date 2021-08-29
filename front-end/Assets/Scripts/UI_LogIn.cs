using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using TMPro;
using Proyecto26;
using UnityEngine.SceneManagement;

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
    public DataManager dataManager;


    /// <summary>
    /// Activated on click.
    /// </summary>
    public void LogIn()
    {
        string url = "http://127.0.0.1:5000/login";
        string dataString = JsonUtility.ToJson(data);
        if (data.username != null && data.password != null)
        {
            RequestToBackEnd(url, dataString, "POST");
        }
        else
        {
            logInMessage.text = "Por favor, ingresa usuario y contraseña.";
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
            TokenCreator();
            #if UNITY_EDITOR && DEBUG
            #endif
            logInMessage.text = $"{res.Text}";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        })
        .Catch(err =>
        {
            if (err.Message == "HTTP/1.1 401 Unauthorized")
            {
                logInMessage.text = "Usuario o contraseña equivocada.";
            }
            else if (err.Message == "HTTP/1.1 400 Bad Request")
            {
                logInMessage.text = "Ingresa usuario Y contraseña, por favor.";
            }
            #if UNITY_EDITOR && DEBUG
            #endif
        })
        .Done(() =>
        {
            #if UNITY_EDITOR && DEBUG
            #endif
        });
    }

    /// <summary>
    /// Creates the unique token in case the log in is successful.
    /// Also sets it on the dataManagement permanent object.
    /// </summary>
    private void TokenCreator()
    { 
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{data.password}"));
            string digestedPassword = System.Convert.ToBase64String(bytes);
            bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{data.username}{digestedPassword}"));
            dataManager.token = System.Convert.ToBase64String(bytes);
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
