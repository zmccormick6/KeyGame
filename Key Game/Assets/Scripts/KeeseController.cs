using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeseController : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    private float Speed = 0.025f;
    private float Health = 3;
    private Vector2 CurrentPosition;
    private Vector2 PlayerPosition;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Keese Hit");
        if (CurrentPosition.x < PlayerPosition.x)
        {
            if (CurrentPosition.y > PlayerPosition.y)
            {
                Debug.Log("Top Left");
                transform.position = new Vector2(CurrentPosition.x - 0.75f, CurrentPosition.y + 0.75f);
            }
            else
            {
                Debug.Log("Bottom Left");
                transform.position = new Vector2(CurrentPosition.x - 0.75f, CurrentPosition.y - 0.75f);
            }
        }
        else if (CurrentPosition.x > PlayerPosition.x)
        {
            if (CurrentPosition.y > PlayerPosition.y)
            {
                Debug.Log("Top Right");
                transform.position = new Vector2(CurrentPosition.x + 0.75f, CurrentPosition.y + 0.75f);
            }
            else
            {
                Debug.Log("Bottom Right");
                transform.position = new Vector2(CurrentPosition.x + 0.75f, CurrentPosition.y - 0.75f);
            }
        }

        KeeseHealth();
    }

    void FixedUpdate()
    {
        CurrentPosition = new Vector2(transform.position.x, transform.position.y);
        PlayerPosition = new Vector2(Player.transform.position.x, Player.transform.position.y);

        transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, Speed);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void KeeseHealth()
    {
        Health--;
    }
}
