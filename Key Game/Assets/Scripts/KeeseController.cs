using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeseController : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private Collider2D PlayerCollider;
    private Collider2D SwordCollider;
    private Collider2D KeeseCollider;

    private float Speed;
    private float Health = 3;
    private Vector2 CurrentPosition;
    private Vector2 PlayerPosition;
    private Animator animator;

    public bool hitPlayer = false;

    private IEnumerator RandomFrame()
    {
        animator.speed = Random.Range(0, 2000);

        yield return new WaitForSeconds(0.1f);
        animator.speed = 1;
    }

    void Start()
    {
        Player = GameObject.Find("TempPlayer");
        SwordCollider = Player.GetComponents<BoxCollider2D>()[0];
        PlayerCollider = Player.GetComponents<BoxCollider2D>()[1];
        KeeseCollider = GetComponent<Collider2D>();

        animator = GetComponent<Animator>();
        StartCoroutine(RandomFrame());

        Speed = Random.Range(0.03f, 0.05f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Enemy" && other.tag != "Wall" && other.tag != "Keyvi" && other.tag != "Obstacle")
        {
            if (other.tag != "Hitbox")
            {
                if (other != PlayerCollider)
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
        }
    }

    void FixedUpdate()
    {
        CurrentPosition = new Vector2(transform.position.x, transform.position.y);
        PlayerPosition = new Vector2(Player.transform.position.x, Player.transform.position.y);

        if (GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause != true)
        {
            if (hitPlayer == false)
                transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, Speed);
            else if (hitPlayer == true)
                transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, -Speed * 2);
        }
    }

    public void KeeseHealth()
    {
        Health--;
        StartCoroutine(SlightInvincibility());

        if (Health <= 0)
        {
            Destroy(gameObject);
            GameObject.Find("Game Manager").GetComponent<DoorSpawn>().EnemyCheck();
        }
    }

    private IEnumerator SlightInvincibility()
    {
        KeeseCollider.enabled = false;
        yield return new WaitForSeconds(0.35f);
        KeeseCollider.enabled = true;
    }

    public void RunHitPlayer()
    {
        StartCoroutine(HitPlayer());
    }

    private IEnumerator HitPlayer()
    {
        hitPlayer = true;
        yield return new WaitForSeconds(0.25f);
        hitPlayer = false;
    }
}
