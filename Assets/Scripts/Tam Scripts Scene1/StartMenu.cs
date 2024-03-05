using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public string nextLevelName; // Name of the next level to load
    public void StartGame()
    {
        SceneManager.LoadScene(nextLevelName); // Load the next level
    }
}
