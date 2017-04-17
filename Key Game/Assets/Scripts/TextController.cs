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
    [SerializeField] private AudioSource Typing;

    public GameObject InGameKeyvi;

    Vector2 KeyviPosition, WhereToGo;
    private Animator KeyviAnim;
    bool move = false, talking = false;
    float startTime;
    int talkingTime = 0;
    Color KeyviAlpha;

    string[] message = 
        {"Hiya!  My name's Keyvi!",
         "Do you just not talk or...?",
         "Oh man, I'm blushing so the Dev's can show off the sprite.",
         "How rude of them.",
         "I don't know you, get away from me.",
         "Don't make me call the Dungeon Police.",
         "Some say that there's a secret at the end of this dungeon.",
         "Totally not a boss that's missing or anything."};
    int[] emotion = {0, 0, 1, 1, 2, 3, 2, 3};
    int counter = 0;

    void Start()
    {
        KeyviAnim = Keyvi.GetComponent<Animator>();
        InGameKeyvi = GameObject.Find("Keyvi");
    }

    void FixedUpdate()
    {
        if (move == true)
        {
            InGameKeyvi.transform.position = Vector2.Lerp(KeyviPosition, WhereToGo, ((Time.time - startTime) / 4));

            KeyviAlpha.a -= Time.deltaTime / 1.25f;
            GameObject.Find("Keyvi").GetComponent<SpriteRenderer>().color = KeyviAlpha;
        }

        if (GameObject.Find("Keyvi").GetComponent<KeyviController>().inRange == true)
        {
            if (talking == false)
            {
                if (Input.GetButton("Fire3"))
                {
                    GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = true;

                    if (talkingTime < 2)
                    {
                        StartTalking();
                    }
                    else
                        EndTalking();

                    talking = true;
                }
            }
        }

        Debug.Log(talking);
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
        GameObject.Find("Keyvi").GetComponent<Animator>().SetInteger("Emotion", emotion[counter]);

        NextButton.SetActive(false);
        EndButton.SetActive(false);

        Typing.Play();
        GameObject.Find("Music").GetComponent<AudioSource>().volume = 0.2f;

        for (int i = 0; i < message[counter].Length; i++)
        {
            DisplayMessage.text += message[counter][i];

            if (message[counter][i] == '.')
                yield return new WaitForSeconds(0.5f);
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

        talking = false;
        talkingTime++;
        Typing.Stop();
        GameObject.Find("Music").GetComponent<AudioSource>().volume = 0.6f;
    }

    private IEnumerator MoveKeyvi()
    {
        GameObject.Find("Keyvi").GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
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
        StartCoroutine(MoveKeyvi());
        MessageHolder.SetActive(false);
        DisplayMessage.text = null;
        talking = false;
        GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = false;
        talkingTime = 0;
    }
}