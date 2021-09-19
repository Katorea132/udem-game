using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI_CreateMatch : MonoBehaviour
{
    public DataManager dataManager;

    private void Start()
    {
        GameObject.Find("CreateMatchButton").GetComponent<Button>().onClick.AddListener(CreateMatch);
    }

    /// <summary>
    /// Creates the match.
    /// </summary>
    void CreateMatch() => StartCoroutine(Request_Coroutine());

    /// <summary>
    /// IEnumerator for a custom request coroutine
    /// </summary>
    IEnumerator Request_Coroutine()
    {
        string url = "http://127.0.0.1:5000/create_match";
        string method = "GET";
        var request = new UnityWebRequest(url, method);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        if (request.responseCode == 200)
        {
            dataManager.match_id = request.downloadHandler.text;
            Debug.Log(dataManager.match_id);
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}
