using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{

    public TextMeshProUGUI[] options;
    public TextMeshProUGUI textQuestion;
    private int index = 0;
    private string questionNumbers;

    //[Serializable]
    //public struct QuestionInfo
    //{
    //    public string question;

    //    public int id;

    //    public string answers;

    //    public int difficulty;
    //}

    //[Serializable]
    //public struct quests
    //{
    //    public QuestionInfo[] questions;
    //}
    //public quests questions;

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
        textQuestion.text = this.questions.questioni[index].question;
        int optionIndex = 0;

        foreach (string answer in this.questions.questioni[index].answers.Split('/'))
        {
            Debug.Log(answer);
            options[optionIndex].text = answer;
            optionIndex++;
        }
        this.index++;
    }

    public void clicked()
    {
        Debug.Log("clickity clakitty");
    }
}
