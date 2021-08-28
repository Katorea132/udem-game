using UnityEngine;

/// <summary>
/// Manages the UI_MainMenu buttons and functionalities.
/// </summary>
public class UI_MainMenu : MonoBehaviour
{
    /// <summary>
    /// Quits the game for good.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Ok, bye. :(");
        Application.Quit();
    }
}
