﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField] private AudioSource KeyviSpeak;
    [SerializeField] private GameObject MessageHolder;
    [SerializeField] private Text SpeakingName;
    [SerializeField] private Text DisplayMessage;
    [SerializeField] private GameObject NextButton;
    [SerializeField] private GameObject EndButton;
    [SerializeField] private GameObject Keyvi;
    [SerializeField] private AudioSource Typing;
    [SerializeField] private AudioSource OneTyping;

    GameObject aButton;

    public GameObject InGameKeyvi;
    public GameObject Keyvil;
    public bool talkingDone = false, bossTalk = false;
    public int startEncounter = 0;

    Vector2 KeyviPosition, WhereToGo;
    private Animator KeyviAnim;
    public bool transition = false;
    bool move = false, talking = false, speedUp = false, once = false;
    float startTime, dots, text;
    int talkingTime = 0, displayTextSize, nameTextSize;
    Color KeyviAlpha;

    //0 Normal, 1 Blush, 2 Shifty, 3 Derp, 4 Flustered Blush, 5 Unsure
    string[] message = 
        {"Hiya! My name's Keyvi!\nI'm important!", "My evil twin brother Keyvil has taken\nover the dungeon.", "You can hit stuff by pressing the\n'X' button. Follow me!",
         "Well, that was Key-z!", "If you somehow managed to take damage...\nthere's a health container over there.", "Press the 'B' button while moving to\ndash over to it, or whatever.",
         "Walking through water will slow you\ndown.", "My brothers' diaries are guarding\nthe way up ahead.", "To reflect the magic, hit its attacks\nwith your key...sword?",
         "Where did you get that key\nthingy anyway?", "It's quite large...", "Have him call me once this is over.\nK?",
         "Have you ever been inside a lock\nbefore...?", "You could say that I've been tumbled\na few times.", "Why are you giving me that look?\nIt's just a key thing!",
         "My brothers' room is up ahead, I can\nsense the keyruption from here.", "Low key, he gets real mad when under\nhalf health.", "Hope you don't die!",
         "Oh, guess he's not here?", "Good Keevening...", "Someone so weak is foolish to fight me in my own domain!",
         "Well...that was something. Good\nwork getting through it!", "Finally! The dungeon is mine!", "Thank you for playing the game!"};
    int[] emotion = {0, 2, 0,
                     3, 0, 0,
                     3, 0, 2, 
                     2, 4, 1,
                     4, 0, 1,
                     0, 0, 0,
                     5, 0, 0,
                     2, 0, 0};
    int counter = 0;

    void Start()
    {
        KeyviAnim = Keyvi.GetComponent<Animator>();
        InGameKeyvi = GameObject.Find("Keyvi");

        dots = 0.5f;
        text = 0.05f;

        counter = GameObject.Find("Level").GetComponent<LevelHold>().Level;
        counter--;
        counter = counter * 3;

        displayTextSize = (int) Screen.currentResolution.height / 20;
        nameTextSize    = (int)Screen.currentResolution.height  / 77;
    }

    void FixedUpdate()
    {
        if (move == true)
        {
            InGameKeyvi.transform.position = Vector2.Lerp(KeyviPosition, WhereToGo, ((Time.time - startTime) / 4));
            aButton = GameObject.Find("A Button");

            KeyviAlpha.a -= Time.deltaTime / 1.25f;
            GameObject.Find("Keyvi").GetComponent<SpriteRenderer>().color = KeyviAlpha;
            //aButton.GetComponent<SpriteRenderer>().color = KeyviAlpha;
        }

        if (GameObject.Find("Keyvi").GetComponent<KeyviController>().inRange == true)
        {
            if (talking == false)
            {
                if (Input.GetButtonDown("Fire3"))
                {
                    GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = true;

                    if (talkingTime < 3)
                    {
                        StartTalking();
                    }
                    else
                    {
                        EndTalking();

                        if (GameObject.Find("Level").GetComponent<LevelHold>().Level == 7)
                        {
                            if (startEncounter == 3)
                            {
                                Debug.Log("Start Phase One");
                                GameObject.Find("Pure Keyvil").GetComponent<PureKeyvilController>().StartPhaseOne();
                            }
                        }
                    }

                    talking = true;
                }
            }
        }

        if (speedUp == true)
        {
            if (Input.GetButtonDown("Speed"))
            {
                dots = 0.01f;
                text = 0.01f;
            }
            else if (Input.GetButtonUp("Speed"))
            {
                dots = 0.5f;
                text = 0.05f;
            }
        }

        if (transition == true)
        {
            float tempX = Keyvil.transform.position.x + 50f;
            float center = Screen.width / 2;

            if (Keyvil.transform.position.x <= center)
            {
                Keyvil.transform.position = new Vector2(tempX, Keyvi.transform.position.y);
            }
            else //(Keyvil.transform.position.x >= Keyvi.transform.position.x)
            {
                StartCoroutine(KeyvilHit());

                if (Keyvi.transform.position.x < 5000)
                {
                    Keyvi.transform.position = new Vector2(Keyvi.transform.position.x + 25, Keyvi.transform.position.y + 10);
                    Keyvi.transform.Rotate(Vector3.forward * -30);
                }
            }
        }

        if (GameObject.Find("Keyvi").GetComponent<KeyviController>().dead == true)
        {
            DisplayMessage.color = Color.white;
            SpeakingName.text = "Keyvi";
            SpeakingName.color = Color.white;

            transition = false;
            Keyvi.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
            Keyvi.transform.rotation = new Quaternion(0, 0, 0, 0);
            Keyvil.SetActive(false);
        }
    }

    private IEnumerator KeyvilHit()
    {
        if (once == false)
        {
            GameObject.Find("Attack").GetComponent<AudioSource>().Play();
            once = true;
        }

        yield return null;
    }

    public void StartTalking()
    {
        if ((counter + 3) % 3 == 0)
        {
            KeyviSpeak.Play();
        }

        MessageHolder.SetActive(true);
        DisplayMessage.text = null;
        DisplayMessage.GetComponent<Text>().fontSize = displayTextSize;
        SpeakingName.GetComponent<Text>().fontSize   = nameTextSize;
        Debug.Log(displayTextSize);
        StartCoroutine(TypingOverTime(message, counter));
    }

    private IEnumerator TypingOverTime(string[] message, int counter)
    {
        KeyviAnim.SetInteger("Emotion", emotion[counter]);
        GameObject.Find("Keyvi").GetComponent<Animator>().SetInteger("Emotion", emotion[counter]);

        NextButton.SetActive(false);
        EndButton.SetActive(false);

        GameObject.Find("LevelMusic").GetComponent<AudioSource>().volume = 0.2f;

        if (GameObject.Find("Level").GetComponent<LevelHold>().Level != 7)
        {
            for (int i = 0; i < message[counter].Length; i++)
            {
                speedUp = true;

                DisplayMessage.text += message[counter][i];

                OneTyping.Play();
                if (message[counter][i] == '.')
                    yield return new WaitForSeconds(dots);
                else
                    yield return new WaitForSeconds(text);
            }
        }
        else
        {
            if (startEncounter >= 1)
            {
                DisplayMessage.color = Color.red;
                SpeakingName.text = "Keyvil";
                SpeakingName.color = Color.red;

                transition = true;
            }

            if (startEncounter == 1)
            {
                GameObject.Find("Pure Keyvil").GetComponent<PureKeyvilController>().MoveToCenter();
            }

            for (int i = 0; i < message[counter].Length; i++)
            {
                speedUp = true;

                DisplayMessage.text += message[counter][i];

                OneTyping.Play();
                if (message[counter][i] == '.')
                    yield return new WaitForSeconds(dots);
                else
                    yield return new WaitForSeconds(text);
            }
            startEncounter++;
        }

        if (startEncounter >= 3)
        {
            //GameObject.Find("Pure Keyvil").GetComponent<PureKeyvilController>().StartPhaseOne();
        }

        GetComponent<TextController>().counter++;
        GameObject.Find("Keyvi").GetComponent<KeyviController>().stop = false;

        NextButton.SetActive(true);

        talking = false;
        talkingTime++;
        Typing.Stop();
        GameObject.Find("LevelMusic").GetComponent<AudioSource>().volume = 0.6f;
        speedUp = false;
        dots = 0.5f;
        text = 0.05f;
    }

    private IEnumerator MoveKeyvi()
    {
        GameObject.Find("Keyvi").GetComponent<Collider2D>().enabled = false;
        GameObject.Find("Keyvi").GetComponent<KeyviController>().AButtonOff();
        yield return new WaitForSeconds(0.75f);
        KeyviAlpha = GameObject.Find("Keyvi").GetComponent<SpriteRenderer>().color;
        startTime = Time.time;
        move = true;

        KeyviPosition = new Vector2(InGameKeyvi.transform.position.x, InGameKeyvi.transform.position.y);
        WhereToGo = new Vector2(InGameKeyvi.transform.position.x, 20);
        yield return new WaitForSeconds(2f);
        move = false;
        talking = false;
    }

    public void EndTalking()
    {
        if (InGameKeyvi.GetComponent<KeyviController>().dead != true)
        {
            StartCoroutine(MoveKeyvi());
            MessageHolder.SetActive(false);
            DisplayMessage.text = null;
            talking = false;
            GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = false;
            talkingTime = 0;
            talkingDone = true;
            InGameKeyvi.GetComponent<KeyviController>().inRange = false;
            GameObject.Find("Game Manager").GetComponent<DoorSpawn>().EnemyCheck();
        }
        else
        {
            MessageHolder.SetActive(false);
            DisplayMessage.text = null;
            talking = false;
            GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = false;
            talkingTime = 0;
            talkingDone = true;
            InGameKeyvi.GetComponent<KeyviController>().inRange = false;
            GameObject.Find("Keyvi").GetComponent<KeyviController>().AButtonOff();
        }
    }
}