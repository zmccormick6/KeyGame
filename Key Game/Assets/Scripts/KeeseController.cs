using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeseController : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private Collider2D PlayerCollider;
    private Collider2D SwordCollider;

    private float Speed = 0.025f;
    private float Health = 3;
    private Vector2 CurrentPosition;
    private Vector2 PlayerPosition;

    void Start()
    {
        SwordCollider = Player.GetComponents<BoxCollider2D>()[0];
        PlayerCollider = Player.GetComponents<BoxCollider2D>()[1];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Keese" && other != PlayerCollider)
        {
            KeeseHealth();

            if (CurrentPosition.x < PlayerPosition.x)
            {
                if (CurrentPosition.y > PlayerPosition.y)
                {
                    transform.position = new Vector2(CurrentPosition.x - 0.75f, CurrentPosition.y + 0.75f);
                }
                else
                {
                    transform.position = new Vector2(CurrentPosition.x - 0.75f, CurrentPosition.y - 0.75f);
                }
            }
            else if (CurrentPosition.x > PlayerPosition.x)
            {
                if (CurrentPosition.y > PlayerPosition.y)
                {
                    transform.position = new Vector2(CurrentPosition.x + 0.75f, CurrentPosition.y + 0.75f);
                }
                else
                {
                    transform.position = new Vector2(CurrentPosition.x + 0.75f, CurrentPosition.y - 0.75f);
                }
            }
        }
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
