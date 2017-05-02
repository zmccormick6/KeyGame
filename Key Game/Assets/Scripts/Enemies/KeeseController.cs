using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeseController : MonoBehaviour
{
    [SerializeField] private ParticleSystem Finished;
    [SerializeField] private ParticleSystem Finished2;
    [SerializeField] private GameObject Player;

    private Vector2 TopRight = new Vector2(5.5f, 0f);
    private Vector2 BottomRight = new Vector2(5.5f, -3.5f);
    private Vector2 TopLeft = new Vector2(-5.5f, 0f);
    private Vector2 BottomLeft = new Vector2(-5.5f, -3.5f);

    private Vector2[] KeesePositions = { new Vector2(-5.5f, 0f), new Vector2(-5.5f, -3.5f), new Vector2(5.5f, -3.5f), new Vector2(5.5f, 0f)};

    private Collider2D PlayerCollider;
    private Collider2D SwordCollider;
    private Collider2D KeeseCollider;

    private float Speed;
    private float Health = 3;
    private Vector2 CurrentPosition;
    private Vector2 PlayerPosition;
    private Animator animator;

    SpriteRenderer tempSprite;
    AudioSource Attack;

    public bool hitPlayer = false;
    bool KeesePassive = false, topRight, bottomRight, bottomLeft, topLeft, passiveLimit = true;
    float attackTime;
    int location;

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
        tempSprite = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(RandomFrame());

        Speed = Random.Range(0.03f, 0.05f);
        attackTime = Random.Range(1f, 3f);

        Attack = GameObject.Find("Attack").GetComponent<AudioSource>();

        StartCoroutine(PassiveRun());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MageAttack")
        {
            if (other.name != "BossAttack(Clone)" && other.name != "ThirdBossAttack(Clone)")
            {
                if (other.GetComponent<MageAttack>().reverse == true)
                {
                    Attack.Play();
                    KeeseHealth();

                    if (CurrentPosition.x < PlayerPosition.x)
                    {
                        if (CurrentPosition.y > PlayerPosition.y)
                        {
                            transform.position = new Vector2(CurrentPosition.x - 1.25f, CurrentPosition.y + 1.25f);
                        }
                        else
                        {
                            transform.position = new Vector2(CurrentPosition.x - 1.25f, CurrentPosition.y - 1.25f);
                        }
                    }
                    else if (CurrentPosition.x > PlayerPosition.x)
                    {
                        if (CurrentPosition.y > PlayerPosition.y)
                        {
                            transform.position = new Vector2(CurrentPosition.x + 1.25f, CurrentPosition.y + 1.25f);
                        }
                        else
                        {
                            transform.position = new Vector2(CurrentPosition.x + 1.25f, CurrentPosition.y - 1.25f);
                        }
                    }
                }
            }
        }

        if (other.tag != "Boss" && other.tag != "Enemy" && other.tag != "Wall" && other.tag != "Keyvi" && other.tag != "Obstacle" && other.tag != "Water" && other.tag != "Right" && other.tag != "Left"  && other.tag != "MageAttack" && other.gameObject.name != "Pure Keyvil")
        {
            if (other.tag != "Hitbox")
            {
                if (other != PlayerCollider)
                {
                    Attack.Play();
                    KeeseHealth();

                    if (CurrentPosition.x < PlayerPosition.x)
                    {
                        if (CurrentPosition.y > PlayerPosition.y)
                        {
                            transform.position = new Vector2(CurrentPosition.x - 1.25f, CurrentPosition.y + 1.25f);
                        }
                        else
                        {
                            transform.position = new Vector2(CurrentPosition.x - 1.25f, CurrentPosition.y - 1.25f);
                        }
                    }
                    else if (CurrentPosition.x > PlayerPosition.x)
                    {
                        if (CurrentPosition.y > PlayerPosition.y)
                        {
                            transform.position = new Vector2(CurrentPosition.x + 1.25f, CurrentPosition.y + 1.25f);
                        }
                        else
                        {
                            transform.position = new Vector2(CurrentPosition.x + 1.25f, CurrentPosition.y - 1.25f);
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
            if (KeesePassive == true)
            {
                if (hitPlayer == false)
                    transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, Speed);
                else if (hitPlayer == true)
                {
                    transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, -Speed * 3);
                    StartCoroutine(PassiveRun());
                }
            }
            else if (KeesePassive == false)
            {
                Passive();
            }
        }
    }

    public void KeeseHealth()
    {
        Health--;
        StartCoroutine(HitFlashing());
        StartCoroutine(SlightInvincibility());

        if (Health <= 0)
        {
            var finished = Instantiate(Finished, transform.position, transform.rotation);
            var finishedTwo = Instantiate(Finished2, transform.position, transform.rotation);
            finished.Play();
            finishedTwo.Play();
            GameObject.Find("Game Manager").GetComponent<DoorSpawn>().EnemyCheck();
            Destroy(gameObject);
        }
    }

    private IEnumerator HitFlashing()
    {
        tempSprite.color = new Color(1f, 1f, 1f, 0.5f);

        int flashing = 0;

        for (int i = 0; i < 10; i++)
        {
            if (flashing % 2 == 0)
            {
                tempSprite.enabled = false;
            }

            else if (flashing % 2 == 1)
            {
                tempSprite.enabled = true;
            }

            flashing++;

            yield return new WaitForSeconds(0.075f);
        }
        tempSprite.color = new Color(1f, 1f, 1f, 1f);
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

    public void Passive()
    {
    }

    private IEnumerator PassiveRun()
    {
        yield return new WaitForSeconds(0.25f);
        KeesePassive = false;
        yield return new WaitForSeconds(attackTime);
        KeesePassive = true;
    }
}