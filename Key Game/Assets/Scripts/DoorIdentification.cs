using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIdentification : MonoBehaviour
{
    public GameObject Door;

    Animator DoorAnim;

    void Start()
    {
        Door = gameObject;
        DoorAnim = Door.GetComponent<Animator>();
        GameObject.Find("Game Manager").GetComponent<DoorSpawn>().DoorFind(Door, DoorAnim);
    }
}