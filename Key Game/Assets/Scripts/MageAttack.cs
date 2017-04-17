using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    float moveX;

    void Start()
    {
        moveX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (gameObject.tag == "Right")
        {
            moveX += 0.1f;
        }

        transform.localPosition = new Vector2(moveX, transform.position.y);
    }
}