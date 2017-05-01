using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossAttack : MonoBehaviour
{
    GameObject Player;
    GameObject[] Stuff;
    float moveX, moveY, attackSpeed = 0.1f;
    bool yPos = false, start = false;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");
        StartCoroutine(Timer());
        StartCoroutine(DestroyAttack());
    }

    void FixedUpdate()
    {
        if (start == true)
        {
            if (yPos == true)
                transform.position = new Vector2(transform.position.x - attackSpeed, -4f);
            else
                transform.position = new Vector2(transform.position.x - attackSpeed, -1.5f);
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        start = true;
    }

    public void yPosition(int i)
    {
        if (i % 2 == 0)
            yPos = true;
        else
            yPos = false;
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
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }
}