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

            if (transform.position.x > 9f)
            {
                Destroy(gameObject);
            }
        }
        else if (gameObject.tag == "Left")
        {
            moveX -= 0.1f;

            if (transform.position.x < -9f)
            {
                Destroy(gameObject);
            }
        }

        transform.localPosition = new Vector3(moveX, transform.position.y, -1);
    }
}