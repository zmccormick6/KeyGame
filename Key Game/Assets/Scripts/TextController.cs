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
    [SerializeField] private AudioSource OneTyping;

    GameObject aButton;

    public GameObject InGameKeyvi;
    public bool talkingDone = false;

    Vector2 KeyviPosition, WhereToGo;
    private Animator KeyviAnim;
    bool move = false, talking = false, speedUp = false;
    float startTime, dots, text;
    int talkingTime = 0;
    Color KeyviAlpha;

    //0 Normal, 1 Blush, 2 Shifty, 3 Derp, 4 Flustered Blush, 5 Squidward, 6 
    string[] message = 
        {"Hiya!  My name's Keyvi!", "You should probably be careful, the\nnext room has a Keyse in it.", "But you'll probably hit it using the\nRight Trigger or some tutorial garbage.",
         "Do you just not talk or...?", "I don't know you, get away from me!", "Don't make me call the Dungeon\nPolice.",
         "In the next room will be...well...a\nbook honestly.", "It will fire magic at you rapidly, so\nmake sure to always be moving.", "Maybe if you're good enough you can\nhit it back at them?",
         "The water in this dungeon will slow\nyou down if you walk through it.", "Also it'll get a lot of water in your boots,\nso probably don't do that.", "...also you can't walk into lava because\ndeath and stuff.",
         "Tutorial Garbage #3: Use B to dodge\nover enemies, magic, and obstacles!", "Or you could even...dodge your way\ninto my heart...", "Wait! Um...forget that...uh...bye!",
         "This is the end of the Demo!", "Thank you for playing, we are\nstill working on the rest of the game.", "This will include more levels and\na boss fight!"};
    int[] emotion = {0, 0, 0,
                     0, 2, 3,
                     0, 0, 3,
                     0, 2, 3,
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
            aButton = GameObject.Find("A Button");

            KeyviAlpha.a -= Time.deltaTime / 1.25f;
            GameObject.Find("Keyvi").GetComponent<SpriteRenderer>().color = KeyviAlpha;
            aButton.GetComponent<SpriteRenderer>().color = KeyviAlpha;
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

        GameObject.Find("Music").GetComponent<AudioSource>().volume = 0.2f;

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
}