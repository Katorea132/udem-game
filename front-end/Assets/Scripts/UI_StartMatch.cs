using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_StartMatch : MonoBehaviour
{

    public void ReadMatch(string match)
    {
        if (match != "")
        {
            DataManager.instance.match_id = match;
            Debug.Log(DataManager.instance.match_id);
        }
        else
        {
            DataManager.instance.match_id = null;
        }
    }

    public void StartQuestions()
    {
        if (DataManager.instance.match_id != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
