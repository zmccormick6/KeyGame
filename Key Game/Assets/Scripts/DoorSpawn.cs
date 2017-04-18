using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoorSpawn : MonoBehaviour
{
    public GameObject CurrentDoor;
    public int enemyCount;

    public void DoorFind(GameObject Door)
    {
        CurrentDoor = Door;

        if (enemyCount != 0)
        {
            Door.SetActive(false);
        }

        if (GameObject.Find("Game Manager").GetComponent<TextController>().talkingDone == false)
        {
            Door.SetActive(false);
        }
    }

    public void EnemyCount(GameObject currentLevel)
    {
        enemyCount = currentLevel.GetComponent<EnemyCount>().count;
    }

    public void EnemyCheck()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            if (GameObject.Find("Game Manager").GetComponent<TextController>().talkingDone == true)
            {
                CurrentDoor.SetActive(true);
                GameObject.Find("Game Manager").GetComponent<TextController>().talkingDone = false;
            }
        }
    }
}