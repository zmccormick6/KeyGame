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
    public bool talkingDone = false;

    Vector2 KeyviPosition, WhereToGo;
    private Animator KeyviAnim;
    bool move = false, talking = false, speedUp = false;
    float startTime, dots, text;
    int talkingTime = 0;
    Color KeyviAlpha;

    //0 Normal, 1 Blush, 2 Shifty, 3 Derp, 4 Flustered Blush
    string[] message = 
        {"Hiya!  My name's Keyvi!", "You should probably be careful, the next room has a Keyse in it.", "But you'll probably just hit it using the Right Trigger or some tutorial garbage.",
         "Do you just not talk or...?", "I don't know you, get away from me!", "Don't make me call the Dungeon Police.",
         "The water in this dungeon will slow you down if you walk through it.", "Also it'll get a lot of water in your boots, so probably don't do that.", "...also you can't walk into lava because death and stuff.",
         "That you just fought there was a Keyni.", "It will fire magic from the left or right of the dungeon at you.", "Don't get hit by them or you'll take damage and probably die, but whatever right?",
         "Tutorial Garbage #3: Use B to dodge over enemies, magic, and obstacles!", "Or you could even...dodge your way into my heart...", "Wait, forget that....bye!",
         "Hey...I've totally never met you before!", "From your lack of response I can only assume that you spoke with my evil twin!", "Yeah, totally...that!  Don't talk to him anymore!"};
    int[] emotion = {0, 0, 0,
                     0, 2, 3,
                     0, 2, 0,
                     0, 0, 3,
                     0, 1, 4,
                     2, 0, 4};
    int counter = 0;

    void Start()
    {
        KeyviAnim = Keyvi.GetComponent<Animator>();
        InGameKeyvi = GameObject.Find("Keyvi");

        dots = 0.5f;
        text = 0.05f;
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
                if (Input.GetButtonDown("Fire3"))
                {
                    GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = true;

                    if (talkingTime < 3)
                    {
                        StartTalking();
                    }
                    else
                        EndTalking();

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
            speedUp = true;

            DisplayMessage.text += message[counter][i];

            if (message[counter][i] == '.')
                yield return new WaitForSeconds(dots);
            else
                yield return new WaitForSeconds(text);
        }

        GetComponent<TextController>().counter++;
        GameObject.Find("Keyvi").GetComponent<KeyviController>().stop = false;

        if (counter % 3 == 0)
        {
            NextButton.SetActive(true);
        }
        else
            EndButton.SetActive(true);

        talking = false;
        talkingTime++;
        Typing.Stop();
        GameObject.Find("Music").GetComponent<AudioSource>().volume = 0.6f;
        speedUp = false;
        dots = 0.5f;
        text = 0.05f;
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
        talkingDone = true;
        GameObject.Find("Game Manager").GetComponent<DoorSpawn>().EnemyCheck();
    }
}