using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIdentification : MonoBehaviour
{
    public GameObject Door;

    void Start()
    {
        Door = gameObject;
        GameObject.Find("Game Manager").GetComponent<DoorSpawn>().DoorFind(Door);
    }
}