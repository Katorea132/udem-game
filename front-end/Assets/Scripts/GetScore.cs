using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GetScore : MonoBehaviour
{

    public TextMeshProUGUI scoreText;

    public void QuitGame()
    {
        Debug.Log("Ok, bye. :(");
        Application.Quit();
    }
    IEnumerator Request_Coroutine(string id)
    {
        string url = "http://127.0.0.1:5000" + "/get_score?id=" + id;
        string method = "GET";
        var request = new UnityWebRequest(url, method);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        if (request.responseCode == 200)
        {
            scoreText.text = request.downloadHandler.text;
        }
        else
        {
            Debug.Log(request.error);
        }
    }

    public void getScore(string id) => StartCoroutine(Request_Coroutine(DataManager.instance.match_id));
}
