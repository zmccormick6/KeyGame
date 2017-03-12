using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelSwitch : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    private GameObject[] Levels;
    public GameObject currentLevel;
    private GameObject nextLevel;
    private Vector2 playerPosition;
    private Vector2 playerDoorStart = new Vector2(0, -3.25f);
    private Vector2 upperPosition = new Vector2(0, 10);
    private Vector2 middlePosition = new Vector2(0, 0);
    private Vector2 lowerPosition = new Vector2(0, -10);
    public int level = 0;

    private bool movement = false;
    float startTime;

    void Start()
    {
        Levels = Resources.LoadAll("Levels", typeof(GameObject)).Cast<GameObject>().ToArray();
        StartCoroutine(NextLevel());
        GetComponent<DoorSpawn>().EnemyCount(currentLevel);
    }

    void FixedUpdate()
    {
        if (movement == true)
        {
            Player.transform.position = Vector2.Lerp(playerPosition, playerDoorStart, (Time.time - startTime) / 2);
            currentLevel.transform.position = Vector2.Lerp(middlePosition, lowerPosition, (Time.time - startTime) / 2);
            nextLevel.transform.position = Vector2.Lerp(upperPosition, middlePosition, (Time.time - startTime) / 2);
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
            currentLevel.transform.SetSiblingIndex(2);
            currentLevel.tag = "Current";
        }
        else
        {
            currentLevel = GameObject.FindGameObjectWithTag("Current");
            nextLevel = Instantiate(Levels[level], new Vector2(0, 10), Quaternion.identity);
            nextLevel.transform.SetSiblingIndex(2);
            nextLevel.tag = "Next";

            GetComponent<DoorSpawn>().EnemyCount(currentLevel);

            StartCoroutine(MoveLevel());
            yield return new WaitForSeconds(2);
        }

        level++;
    }

    private IEnumerator MoveLevel()
    {
        startTime = Time.time;
        movement = true;
        playerPosition = new Vector2(Player.transform.position.x, Player.transform.position.y);
        yield return new WaitForSeconds(2);
        movement = false;
            
        Destroy(GameObject.FindGameObjectWithTag("Current"));
        currentLevel = GameObject.FindGameObjectWithTag("Next");
        currentLevel.tag = "Current";
    }
}
