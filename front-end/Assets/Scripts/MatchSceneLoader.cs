using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads the scene for match finding/creation.
/// </summary>
public class MatchSceneLoader : MonoBehaviour
{
    /// <summary>
    /// Moves to the next scene, which corresponds to the match
    /// creating or joining menu.
    /// </summary>
    public void StartMatch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
