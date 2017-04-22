using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    [SerializeField] private string LoadTryAgainLevel;
    [SerializeField] private string LoadMainMenu;

    public void TryAgain()
    {
        SceneManager.LoadScene(LoadTryAgainLevel);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(LoadMainMenu);
    }
}