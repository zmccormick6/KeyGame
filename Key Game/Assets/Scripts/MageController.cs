using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    [SerializeField] private GameObject MageAttack;
    [SerializeField] private GameObject Player;

    AudioSource AttackSound;
    SpriteRenderer tempSprite;

    private Collider2D PlayerCollider;
    private Collider2D MageCollider;
    private Animator animator;

    private int Health = 5;

    Vector2 CurrentPosition;
    Vector2 PlayerPosition;

    int attackCount;
    float attackTime, moveX, moveY;
    bool attackReady = false;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");

        MageCollider = GetComponent<Collider2D>();
        PlayerCollider = Player.GetComponents<BoxCollider2D>()[1];
        animator = GetComponent<Animator>();
        tempSprite = gameObject.GetComponent<SpriteRenderer>();

        AttackSound = GameObject.Find("Attack").GetComponent<AudioSource>();

        attackTime = Random.Range(1f, 3f);
        StartCoroutine(StartSpawn());
    }

    void FixedUpdate()
    {
        CurrentPosition = new Vector2(transform.position.x, transform.position.y);
        PlayerPosition = new Vector2(Player.transform.position.x, Player.transform.position.y);

        if (Health <= 0)
        {
            GameObject.Find("Game Manager").GetComponent<DoorSpawn>().EnemyCheck();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Enemy" && other.tag != "Wall" && other.tag != "Keyvi" && other.tag != "Obstacle" && other.tag != "Water" && other.tag != "Right" && other.tag != "Left")
        {
            if (other.tag != "Hitbox")
            {
                if (other != PlayerCollider)
                {
                    MageCollider.enabled = false;

                    if (CurrentPosition.y > PlayerPosition.y)
                    {
                        transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y + 0.75f);
                    }
                    else if (CurrentPosition.y < PlayerPosition.y)
                    {
                        transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y - 0.75f);
                    }

                    StartCoroutine(SlightInvincibility());
                    AttackSound.Play();
                    MageHealth();
                }
            }
        }
    }

    public void MageHealth()
    {
        Health--;
        StartCoroutine(HitFlashing());
    }

    private IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Spawn());
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
        MageCollider.enabled = false;
        yield return new WaitForSeconds(0.35f);
        MageCollider.enabled = true;
    }

    private IEnumerator Spawn()
    {
        animator.SetInteger("Mage", 0);
        moveX = Random.Range(-6.5f, 6.5f);
        moveY = Random.Range(-3.5f, 1f);

        transform.localPosition = new Vector2(moveX, moveY);
        yield return new WaitForSeconds(1.5f);
        attackCount = 0;

        for (int i = 0; i < 3; i++)
        {
            animator.SetInteger("Mage", 2);
            Instantiate(MageAttack, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
            animator.SetInteger("Mage", 1);
            yield return new WaitForSeconds(attackTime);
        }

        StartCoroutine(DeSpawn());
    }

    private IEnumerator DeSpawn()
    {
        yield return new WaitForSeconds(2f);
        animator.SetInteger("Mage", 3);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Spawn());
    }

    private IEnumerator Attack()
    {
        animator.SetInteger("Mage", 2);
        Instantiate(MageAttack, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        attackCount++;
        animator.SetInteger("Mage", 1);
        attackReady = false;
        yield return new WaitForSeconds(attackTime);

        if (attackCount >= 3)
        {
            StartCoroutine(DeSpawn());
        }
        else
            attackReady = true;
    }
}