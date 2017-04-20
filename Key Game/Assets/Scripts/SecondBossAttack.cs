using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossAttack : MonoBehaviour
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
        transform.position = new Vector2(transform.position.x - attackSpeed, -1.5f);
    }

    public void Direction(int direction)
    {
        if (direction == 5)
        {
            attackSpeed = -attackSpeed;
        }
    }

    private IEnumerator DestroyAttack()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}