using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    GameObject Player;
    float moveX, moveY, attackSpeed = 0.1f;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");
        StartCoroutine(DestroyAttack());
    }

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);
    }

    public void AttackDirection(int direction)
    {
        if (direction == 0)
        {
            moveX = -attackSpeed;
            //Left
        }
        else if (direction == 1)
        {
            moveY = attackSpeed;
            //Up
        }
        else if (direction == 2)
        {
            moveX = attackSpeed;
            //Right
        }
        else if (direction == 3)
        {
            moveY = -attackSpeed;
            //Down
        }
    }

    private IEnumerator DestroyAttack()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
