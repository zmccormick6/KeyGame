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
        GameObject.Find("Level").GetComponent<LevelHold>().Level--;
        SceneManager.LoadScene(LoadTryAgainLevel);
    }

    public void MainMenu()
    {
        GameObject.Find("Level").GetComponent<LevelHold>().Level = 0;
        SceneManager.LoadScene(LoadMainMenu);
    }
}