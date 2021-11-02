using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI_CreateMatch : MonoBehaviour
{

    private struct MatchData
    {
        public string match_id;
    }

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
        MatchData match_id;
        string url = "http://127.0.0.1:5000/create_match";
        string method = "GET";
        var request = new UnityWebRequest(url, method);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        if (request.responseCode == 200)
        {
            match_id = JsonUtility.FromJson<MatchData>(request.downloadHandler.text);
            DataManager.instance.match_id = match_id.match_id;
            Debug.Log(DataManager.instance.match_id);
            GameObject.Find("MatchId").GetComponent<Text>().text = string.Format("Match Id: {0}", DataManager.instance.match_id);
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}
