using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    void Start()
    {
        if (GameObject.Find("Level").GetComponent<LevelHold>().Level != 0)
            gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Speed"))
        {
            gameObject.SetActive(false);
        }
    }
}