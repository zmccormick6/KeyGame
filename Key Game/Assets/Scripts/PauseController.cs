using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject QuitGame;
    [SerializeField] private GameObject RestartGame;
    [SerializeField] private string MainMenu;
    [SerializeField] private string Game;

    bool activate = true;

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
        }
        else
        {
            PauseMenu.SetActive(false);
            GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = false;
            activate = true;
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene(MainMenu);
    }

    public void Restart()
    {
        SceneManager.LoadScene(Game);
    }
}