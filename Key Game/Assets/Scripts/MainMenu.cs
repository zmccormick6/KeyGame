﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject PlayPointer;
    [SerializeField] private string Game;
    [SerializeField] private GameObject CreditScreen;

    int screen = 0;

    AsyncOperation async;

    public void PlayGame()
    {
        PlayerPrefs.SetInt("Level", 0);
        SceneManager.LoadScene(Game);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenCredits()
    {
        if (screen % 2 == 0)
            CreditScreen.SetActive(true);
        else
            CreditScreen.SetActive(false);

        screen++;
    }

    public void CloseCredits()
    {
        CreditScreen.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(Preload());
    }

    private IEnumerator Preload()
    {
        yield return new WaitForSeconds(0.1f);
        async = Application.LoadLevelAsync(Game);
        async.allowSceneActivation = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Speed"))
        {
            if (PlayPointer.activeSelf == true)
            {
                async.allowSceneActivation = true;
            }
            //else
              //  QuitGame();
        }
    }
}