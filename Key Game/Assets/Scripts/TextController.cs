using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField] private GameObject MessageHolder;
    [SerializeField] private Text DisplayMessage;
    [SerializeField] private GameObject NextButton;
    [SerializeField] private GameObject EndButton;

    string[] message = 
        {"Hey...?  Who are you?",
         "I don't know you, get away!",
         "Stuff about dungeons and things",
         "Just testing the clicking really"};
    int counter = 0;

    public void StartTalking()
    {
        MessageHolder.SetActive(true);
        DisplayMessage.text = null;
        StartCoroutine(TypingOverTime(message, counter));
    }

    private IEnumerator TypingOverTime(string[] message, int counter)
    {
        NextButton.SetActive(false);
        EndButton.SetActive(false);

        for (int i = 0; i < message[counter].Length; i++)
        {
            DisplayMessage.text += message[counter][i];

            if (message[counter][i] == '.')
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
                yield return new WaitForSeconds(0.075f);
        }

        GetComponent<TextController>().counter++;
        GameObject.Find("Keyvi").GetComponent<KeyviController>().stop = false;
        if (counter % 2 == 0)
        {
            NextButton.SetActive(true);
        }
        else
            EndButton.SetActive(true);
    }

    public void EndTalking()
    {
        MessageHolder.SetActive(false);
        DisplayMessage.text = null;
        GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = false;
    }
}