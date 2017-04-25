using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossAttack : MonoBehaviour
{
    bool move = false;
    float moveX, moveY;

    void Start()
    {
        StartCoroutine(Move());
        StartCoroutine(Destroy());
    }

    void FixedUpdate()
    {
        if (move == true)
            transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.75f);

        moveX = Random.Range(-0.25f, 0.25f);
        moveY = Random.Range(-0.25f, 0.25f);

        if (moveX > -0.05f && moveX < 0.05f)
        {
            moveX += 0.1f;
        }
        if (moveY > -0.05f && moveY < 0.05f)
        {
            moveY += 0.1f;
        }

        move = true;
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}