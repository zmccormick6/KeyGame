using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    [SerializeField] private GameObject TryAgainPointer;
    [SerializeField] private GameObject MainMenuPointer;

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

    public void Up()
    {
        TryAgainPointer.SetActive(true);
        MainMenuPointer.SetActive(false);
    }

    public void Down()
    {
        TryAgainPointer.SetActive(false);
        MainMenuPointer.SetActive(true);
    }
}