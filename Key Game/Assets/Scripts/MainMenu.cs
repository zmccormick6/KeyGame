using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private string Game;

    AsyncOperation async;

    public void PlayGame()
    {
        SceneManager.LoadScene(Game);
    }

    public void QuitGame()
    {
        Application.Quit();
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
            async.allowSceneActivation = true;
        }
    }
}