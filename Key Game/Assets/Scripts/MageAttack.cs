using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    GameObject Player;

    Vector2 CurrentPosition, PlayerPosition, NextPlayerPosition;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");
        PlayerPosition =  new Vector2(Player.transform.position.x, Player.transform.position.y);
        NextPlayerPosition = new Vector2(Player.transform.position.x * 3, Player.transform.position.y * 3);

        Debug.Log(PlayerPosition + ", " + NextPlayerPosition);
    }

    void FixedUpdate()
    {
        CurrentPosition = new Vector2(transform.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, 0.1f);
    }
}