using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour
{
    [SerializeField] private AudioSource PauseOn;
    [SerializeField] private AudioSource PauseOff;

    [SerializeField] private EventSystem Event;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject QuitGame;
    [SerializeField] private GameObject RestartGame;
    [SerializeField] private GameObject TryAgainPointer;
    [SerializeField] private GameObject MainMenuPointer;
    [SerializeField] private string MainMenu;
    [SerializeField] private string Game;

    bool activate = true;

    void Start()
    {
        Event.SetSelectedGameObject(RestartGame);
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Submit"))
        {
            PauseActivate();
        }
    }

    public void PauseActivate()
    {
        if (activate == true)
        {
            if (GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause == false)
            {
                PauseMenu.SetActive(true);
                GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = true;
                activate = false;
            }

            PauseOn.Play();
        }
        else
        {
            PauseMenu.SetActive(false);
            GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = false;
            activate = true;
            PauseOff.Play();
        }

        Event.SetSelectedGameObject(QuitGame);
    }

    public void Quit()
    {
        GameObject.Find("Level").GetComponent<LevelHold>().Level = 0;
        SceneManager.LoadScene(MainMenu);
    }

    public void Restart()
    {
        GameObject.Find("Level").GetComponent<LevelHold>().Level = 0;
        SceneManager.LoadScene(Game);
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