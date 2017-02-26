using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerController : MonoBehaviour
{
    private Animator anim;
    private float movex = 0f;
    private float movey = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        movex = Input.GetAxis("Horizontal");
        movey = Input.GetAxis("Vertical");

        //West
        if (movex < 0)
        {
            anim.SetInteger("Direction", 4);
            Debug.Log("Direction is West");
        }
        //East
        else if (movex > 0)
        {
            anim.SetInteger("Direction", 2);
            Debug.Log("Direction is East");
        }
        //North
        else if (movey > 0)
        {
            anim.SetInteger("Direction", 1);
            Debug.Log("Direction is North");
        }
        //South
        else if (movey < 0)
        {
            anim.SetInteger("Direction", 3);
            Debug.Log("Direction is South");
        }
        else
        {
            anim.SetInteger("Direction", 0);
            Debug.Log("Direction is NULL");
        }
    }
}