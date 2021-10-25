using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_StartMatch : MonoBehaviour
{
    public DataManager dataManager;

    public void ReadMatch(string match)
    {
        if (match != "")
        {
            dataManager.match_id = match;
        }
        else
        {
            dataManager.match_id = null;
        }
    }

    public void StartQuestions()
    {
        if (dataManager.match_id != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
