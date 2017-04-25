using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    GameObject Player;
    Collider2D SwordHitbox;

    Vector2 CurrentPosition, PlayerPosition, StartPosition, ReversePosition;

    public float MageX, MageY;
    public bool reverse = false;
    bool attack = false;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");
        PlayerPosition =  new Vector3(Player.transform.position.x, Player.transform.position.y, -2);
        StartPosition = new Vector3(transform.position.x, transform.position.y, -2);

        SwordHitbox = Player.GetComponents<Collider2D>()[0];

        StartCoroutine(Destroy());
        StartCoroutine(WaitAttack());
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
        if (attack == true)
        {
            ReversePosition = new Vector2(MageX, MageY);
            CurrentPosition = new Vector2(transform.position.x, transform.position.y);

            if (reverse == false)
                transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, 0.15f);
            else if (reverse == true)
                transform.position = Vector2.MoveTowards(CurrentPosition, ReversePosition, 0.25f);

            if (CurrentPosition == PlayerPosition)
            {
                Destroy(gameObject);
            }
            else if (CurrentPosition == ReversePosition)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(0.5f);
        attack = true;
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}