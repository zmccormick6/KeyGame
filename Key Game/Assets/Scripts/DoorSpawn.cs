using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoorSpawn : MonoBehaviour
{
    public GameObject CurrentDoor;
    public int enemyCount;

    public void DoorFind(GameObject Door, Animator DoorAnim)
    {
        CurrentDoor = Door;

        if (enemyCount != 0)
        {
            Door.GetComponent<Collider2D>().enabled = false;
        }

        if (GameObject.Find("Game Manager").GetComponent<TextController>().talkingDone == false)
        {
            Door.GetComponent<Collider2D>().enabled = false;
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
            GameObject.Find("Keyvi").GetComponent<KeyviController>().AButtoOn();

            if (GameObject.Find("Game Manager").GetComponent<TextController>().talkingDone == true)
            {
                if (GameObject.Find("Level").GetComponent<LevelHold>().Level != 7)
                {
                    CurrentDoor.GetComponent<Collider2D>().enabled = true;
                    CurrentDoor.GetComponent<Animator>().SetInteger("DoorOpen", 1);
                }
                else
                {
                    Debug.Log("Stuff");
                    //GameObject.Find("Pure Keyvil").GetComponent<PureKeyvilController>().ChooseAttackPublic();
                }

                GameObject.Find("Keyvi").GetComponent<KeyviController>().AButtonOff();
                GameObject.Find("Game Manager").GetComponent<TextController>().talkingDone = false;
            }
        }
    }
}