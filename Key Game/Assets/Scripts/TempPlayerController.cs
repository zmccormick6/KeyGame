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
        MovementAnimation();
        SwingAnimation();
    }

    public void MovementAnimation()
    {
        movex = Input.GetAxis("Horizontal");
        movey = Input.GetAxis("Vertical");

        //West
        if (movex < 0)
        {
            anim.SetInteger("Direction", 4);
        }
        //East
        else if (movex > 0)
        {
            anim.SetInteger("Direction", 2);
        }
        //North
        else if (movey > 0)
        {
            anim.SetInteger("Direction", 1);
        }
        //South
        else if (movey < 0)
        {
            anim.SetInteger("Direction", 3);
        }
        else
        {
            anim.SetInteger("Direction", 0);
        }
    }

    public void SwingAnimation()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetInteger("Swing", 1);
        }
        else
        {
            anim.SetInteger("Swing", 0);
        }
    }
}