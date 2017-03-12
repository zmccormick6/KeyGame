using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoorSpawn : MonoBehaviour
{
    public GameObject CurrentDoor;
    int enemyCount;

    public void DoorFind(GameObject Door)
    {
        CurrentDoor = Door;
        Door.SetActive(false);
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
            CurrentDoor.SetActive(true);
        }
    }
}