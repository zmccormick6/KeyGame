﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    GameObject Player;
    Collider2D SwordHitbox;

    Vector2 CurrentPosition, PlayerPosition, StartPosition;

    public bool reverse = false;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");
        PlayerPosition =  new Vector2(Player.transform.position.x, Player.transform.position.y);
        StartPosition = new Vector2(transform.position.x, transform.position.y);

        SwordHitbox = Player.GetComponents<Collider2D>()[0];

        StartCoroutine(Destroy());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == SwordHitbox)
        {
            reverse = true;
        }
        else
        {
            if (other.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        CurrentPosition = new Vector2(transform.position.x, transform.position.y);

        if (reverse == false)
            transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, 0.15f);
        else if (reverse == true)
            transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, -0.25f);

        if (CurrentPosition == PlayerPosition)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}