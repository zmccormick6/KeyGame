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
    [SerializeField] private GameObject Keyvi;
    private Animator KeyviAnim;

    string[] message = 
        {"Hiya!  My name's Keyvi!",
         "Do you just no talk or...ok?",
         "Oh man, I'm blushing so the Dev's can show off the sprite.",
         "How rude of them.",
         "I don't know you, get away from me.",
         "Dont' make me call the dungeon Police.",
         "Some say that there's a secret at the end of this dungeon.",
         "Totally not a boss that's missing or anything."};
    int[] emotion = {0, 0, 1, 1, 2, 2, 3, 3};
    int counter = 0;

    void Start()
    {
        KeyviAnim = Keyvi.GetComponent<Animator>();
    }

    public void StartTalking()
    {
        MessageHolder.SetActive(true);
        DisplayMessage.text = null;
        StartCoroutine(TypingOverTime(message, counter));
    }

    private IEnumerator TypingOverTime(string[] message, int counter)
    {
        KeyviAnim.SetInteger("Emotion", emotion[counter]);

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