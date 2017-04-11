using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class LevelSwitch : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private Image LevelTransition;

    private GameObject[] Levels;
    public GameObject currentLevel;
    private GameObject nextLevel;
    private Vector2 playerPosition;
    private Vector2 playerDoorStart = new Vector2(0, -3.25f);
    private Vector2 upperPosition = new Vector2(0, 10);
    private Vector2 middlePosition = new Vector2(0, 0);
    private Vector2 lowerPosition = new Vector2(0, -10);
    public int level = 0;
    public bool pause = false;

    private bool movement = false;
    private bool transition = false;
    Color alpha;
    float startTime;

    void Start()
    {
        Levels = Resources.LoadAll("Levels", typeof(GameObject)).Cast<GameObject>().ToArray();
        StartCoroutine(NextLevel());
        GetComponent<DoorSpawn>().EnemyCount(currentLevel);
        LevelTransition.enabled = true;
        alpha.a = 0f;
        LevelTransition.color = alpha;
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
                alpha.a += 1.5f * Time.deltaTime;
            }
        }
        else
        {
            if (alpha.a > 0f)
            {
                LevelTransition.color = alpha;
                alpha.a -= 1f * Time.deltaTime;
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
            currentLevel = Instantiate(Levels[level], new Vector2(0, 0), Quaternion.identity);
            currentLevel.transform.SetSiblingIndex(3);
            currentLevel.tag = "Current";
        }
        else
        {
            pause = true;
            currentLevel = GameObject.FindGameObjectWithTag("Current");
            nextLevel = Instantiate(Levels[level], new Vector2(0, 10), Quaternion.identity);
            nextLevel.transform.SetSiblingIndex(3);
            nextLevel.tag = "Next";

            GetComponent<DoorSpawn>().EnemyCount(currentLevel);

            StartCoroutine(MoveLevel());
            yield return new WaitForSeconds(2);
            pause = false;
        }

        level++;
    }
    private IEnumerator LevelTransitionEffects()
    {
        transition = true;
        yield return new WaitForSeconds(1.5f);
        transition = false;
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
    }
}
