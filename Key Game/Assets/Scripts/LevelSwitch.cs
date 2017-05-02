using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private Image LevelTransition;
    [SerializeField] private AudioSource MainMusic;
    [SerializeField] private AudioSource EndMusic;

    private GameObject[] Levels;
    public GameObject currentLevel;
    private GameObject nextLevel;
    private Vector2 playerPosition;
    private Vector2 playerDoorStart = new Vector2(0, -3.25f);
    private Vector2 upperPosition = new Vector2(0, 10);
    private Vector2 middlePosition = new Vector2(0, 0);
    private Vector2 lowerPosition = new Vector2(0, -10);
    //public int level = 0;
    int level;
    public bool pause = false;

    GameObject BossHealth;
    GameObject BossBar;

    private bool movement = false;
    public bool transition = false;
    Color alpha;
    float startTime;

    void Start()
    {
        Levels = Resources.LoadAll("Levels", typeof(GameObject)).Cast<GameObject>().ToArray();
        StartCoroutine(NextLevel());
        GameObject.Find("Game Manager").GetComponent<DoorSpawn>().EnemyCount(currentLevel);
        LevelTransition.enabled = true;
        alpha.a = 1f;
        LevelTransition.color = alpha;
        level = GameObject.Find("Level").GetComponent<LevelHold>().Level;
    }

    void FixedUpdate()
    {
        if (movement == true)
        {
            Player.transform.position = Vector2.Lerp(playerPosition, playerDoorStart, (Time.time - startTime) / 2);
            currentLevel.transform.position = Vector2.Lerp(middlePosition, lowerPosition, (Time.time - startTime) / 2);
            nextLevel.transform.position = Vector2.Lerp(upperPosition, middlePosition, (Time.time - startTime) / 2);
        }

        if (transition == true)
        {
            LevelTransition.color = alpha;

            if (alpha.a < 1f)
            {
                alpha.a += 2f * Time.deltaTime;
            }

            if (GameObject.Find("Level").GetComponent<LevelHold>().Level < 6)
            {
                if (MainMusic.volume > 0.3f)
                {
                    MainMusic.volume -= 0.05f;
                }
            }
        }
        else
        {
            if (alpha.a > 0f)
            {
                LevelTransition.color = alpha;
                alpha.a -= 1f * Time.deltaTime;
            }

            if (GameObject.Find("Level").GetComponent<LevelHold>().Level < 6)
            {
                if (MainMusic.volume < 0.6f && pause == false)
                {
                    MainMusic.volume += 0.025f;
                }
            }
        }
    }

    public void RunNextLevel()
    {
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        if (level == 0)
        {
            currentLevel = Instantiate(Levels[GameObject.Find("Level").GetComponent<LevelHold>().Level], new Vector2(0, 0), Quaternion.identity);
            //currentLevel = Instantiate(Levels[6], new Vector2(0, 0), Quaternion.identity);
            currentLevel.transform.SetSiblingIndex(3);
            currentLevel.tag = "Current";
        }
        else if (GameObject.Find("Keyvi").GetComponent<KeyviController>().dead == true)
        {
            transition = true;
            yield return new WaitForSeconds(0.65f);
            GameObject.Find("Level").GetComponent<LevelHold>().Level = -1;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            //level = PlayerPrefs.GetInt("Level");
            pause = true;
            currentLevel = GameObject.FindGameObjectWithTag("Current");
            //nextLevel = Instantiate(Levels[level], new Vector2(0, 10), Quaternion.identity);
            nextLevel = Instantiate(Levels[GameObject.Find("Level").GetComponent<LevelHold>().Level], new Vector2(0, 10), Quaternion.identity);
            nextLevel.transform.SetSiblingIndex(3);
            nextLevel.tag = "Next";

            StartCoroutine(MoveLevel());
            yield return new WaitForSeconds(2);
            pause = false;
        }

        level = GameObject.Find("Level").GetComponent<LevelHold>().Level++;
        //PlayerPrefs.SetInt("Level", level);
    }
    private IEnumerator LevelTransitionEffects()
    {
        transition = true;
        yield return new WaitForSeconds(1.5f);
        transition = false;
        GameObject.Find("Game Manager").GetComponent<TextController>().InGameKeyvi = GameObject.Find("Keyvi");
    }

    private IEnumerator MoveLevel()
    {
        startTime = Time.time;
        movement = true;
        playerPosition = new Vector2(Player.transform.position.x, Player.transform.position.y);
        StartCoroutine(LevelTransitionEffects());
        yield return new WaitForSeconds(2);
        movement = false;

        Destroy(GameObject.FindGameObjectWithTag("Current"));
        currentLevel = GameObject.FindGameObjectWithTag("Next");
        currentLevel.tag = "Current";
        GameObject.Find("Game Manager").GetComponent<DoorSpawn>().enemyCount = 0;
        GameObject.Find("Game Manager").GetComponent<DoorSpawn>().EnemyCount(currentLevel);
    }

    public void PlayEndMusic()
    {
        EndMusic.Play();
    }
}