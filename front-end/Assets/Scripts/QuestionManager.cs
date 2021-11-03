using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{

    public TextMeshProUGUI[] options;
    public TextMeshProUGUI textQuestion;
    private int index = -1;
    private string questionNumbers;


    [Serializable]
    public class Question
    {
        public string answers;
        public int difficulty;
        public int id;
        public string question;
    }

    [Serializable]
    public class Root
    {
        public List<Question> questioni;
    }

    public Root questions;

    [Serializable]
    public class ScoreInfo
    {
        public string question;
        public string answer;
        public int time;
    }

    [Serializable]
    public class ScoreRoot
    {
        public string id;
        public ScoreInfo score_info;
        public string token;
    }

    public ScoreRoot scoreRoot = new ScoreRoot();
    public ScoreInfo scoreInfo = new ScoreInfo();

    private void Awake()
    {
        GetQuestions();
    }

    private void Start()
    {
        GetEachQuestion();
    }

    void GetQuestions() => StartCoroutine(Request_Coroutine_Questions());

    /// <summary>
    /// IEnumerator for a custom request coroutine
    /// </summary>
    IEnumerator Request_Coroutine_Questions()
    {
        string url = "http://127.0.0.1:5000/get_questions?id=" + DataManager.instance.match_id;
        string method = "GET";
        var request = new UnityWebRequest(url, method);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        if (request.responseCode == 200)
        {
            questionNumbers = request.downloadHandler.text;
        }
        else
        {

            Debug.Log(request.error);
        }
    }

    void GetEachQuestion() => StartCoroutine(Request_Coroutine(questionNumbers));

    IEnumerator Request_Coroutine(string id)
    {
        string url = "http://127.0.0.1:5000/get_question?id=" + id;
        string method = "GET";
        var request = new UnityWebRequest(url, method);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode + id);
        if (request.responseCode == 200)
        {
            this.questions = JsonConvert.DeserializeObject<Root>("{\"questioni\":" + request.downloadHandler.text + "}");
            Debug.Log(JsonConvert.DeserializeObject<Root>("{\"questions\":" + request.downloadHandler.text + "}"));
            //Debug.Log("{\"questions\":" + request.downloadHandler.text + "}");
        }
        else
        {
            Debug.Log(request.error);
        }
    }

    public void SetQuestion()
    {
        int optionIndex = 0;
        if (index < this.questions.questioni.Count)
        {
            textQuestion.text = this.questions.questioni[index].question;
            foreach (string answer in this.questions.questioni[index].answers.Split('/'))
            {
                options[optionIndex].text = answer;
                optionIndex++;
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
    public void clicked()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        Debug.Log("clickity clakitty" + go.GetComponentInChildren<TextMeshProUGUI>().text);
        if (index > -1 && index < this.questions.questioni.Count)
        {
            scoreInfo.answer = go.GetComponentInChildren<TextMeshProUGUI>().text;
            scoreInfo.question = this.questions.questioni[index].id.ToString();
            scoreInfo.time = 100;
            scoreRoot.id = DataManager.instance.match_id;
            scoreRoot.token = DataManager.instance.token;
            scoreRoot.score_info = scoreInfo;
            Debug.Log(JsonConvert.SerializeObject(scoreRoot));
            PostScore(JsonConvert.SerializeObject(scoreRoot));
        }
        this.index++;
        SetQuestion();
    }

    IEnumerator Score_Coroutine(string bodyJsonString)
    {
        string url = "http://127.0.0.1:5000" + "/add_score";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        if (request.responseCode == 200)
        {
            Debug.Log("Scored posted successfully.");
        }
    }

    public void PostScore(string jsonBody) => StartCoroutine(Score_Coroutine(jsonBody));
}
