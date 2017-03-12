using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelSwitch : MonoBehaviour
{
    private GameObject[] Levels;
    private GameObject currentLevel;
    private GameObject nextLevel;
    private Vector2 upperPosition = new Vector2(0, 10);
    private Vector2 middlePosition = new Vector2(0, 0);
    private Vector2 lowerPosition = new Vector2(0, -10);
    public int level = 0;

    private bool movement = false;
    float startTime;

    void Start()
    {
        Levels = Resources.LoadAll("Levels", typeof(GameObject)).Cast<GameObject>().ToArray();

        currentLevel = Instantiate(Levels[level], new Vector2(0, 0), Quaternion.identity);
    }

    void FixedUpdate()
    {
        if (movement == true)
        {
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
        level++;
        nextLevel = Instantiate(Levels[level], new Vector2(0, 10), Quaternion.identity);

        StartCoroutine(MoveLevel());
        yield return new WaitForSeconds(2);
    }

    private IEnumerator MoveLevel()
    {
        startTime = Time.time;
        movement = true;
        yield return new WaitForSeconds(2);
        movement = false;
    }
}
