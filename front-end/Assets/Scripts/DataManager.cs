using UnityEngine;

/// <summary>Manages data for persistance between scenes.</summary>
public class DataManager : MonoBehaviour
{
    /// <summary>Static reference to the instance of the DataManager.</summary>
    public static DataManager instance;

    /// <summary>The player's identity token.</summary>
    public string token;
    public string match_id;

    /// <summary>Awake is called when the script instance is being loaded.</summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Destroyed");
        }
        DontDestroyOnLoad(gameObject);
    }
}
