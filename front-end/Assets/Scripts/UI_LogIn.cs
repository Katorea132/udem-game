using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using TMPro;
using Proyecto26;

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
    public TextMeshProUGUI LogInMessage;
    public DataManager dataManager;


    /// <summary>
    /// 
    /// </summary>
    public void LogIn()
    {
        Debug.Log("clicked");
        string url = "http://127.0.0.1:5000/login";
        string dataString = JsonUtility.ToJson(data);
        RequestToBackEnd(url, dataString, "POST");
        
    }

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
        })
        .Catch(err =>
        {
            if (err.Message == "HTTP/1.1 401 Unauthorized")
            {
                LogInMessage.text = "Usuario o contraseña equivocada.";
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


    private string TokenCreator()
    {
        return "token";
    }

    /// <summary>
    /// Reads the username from the input field.
    /// </summary>
    /// <param name="user">Username, plain text.</param>
    public void ReadUsername(string user)
    {
        data.username = user;
    }

    /// <summary>
    /// Reads the password from the input field, hashes it with SHA256
    /// and encodes in base64.
    /// </summary>
    /// <param name="passw">Password, plain text.</param>
    public void ReadPassword(string passw)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Hashes the password
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(passw));
            // Convert byte array to a base64 string
            data.password = System.Convert.ToBase64String(bytes);
        }
    }
}
