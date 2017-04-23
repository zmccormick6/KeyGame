using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    [SerializeField] private GameObject MageAttack;
    [SerializeField] private GameObject Player;
    [SerializeField] private Collider2D MageCollider;

    AudioSource AttackSound;
    SpriteRenderer tempSprite;

    private Collider2D PlayerCollider;
    //private Collider2D MageCollider;
    private Animator animator;

    private int Health = 3;

    Vector2 CurrentPosition;
    Vector2 PlayerPosition;

    int attackCount;
    float attackTime, moveX, moveY;
    bool attackReady = false;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");

        //MageCollider = gameObject.GetComponent<Collider2D>();
        PlayerCollider = Player.GetComponents<BoxCollider2D>()[1];
        animator = GetComponent<Animator>();
        tempSprite = gameObject.GetComponent<SpriteRenderer>();

        AttackSound = GameObject.Find("Attack").GetComponent<AudioSource>();

        attackTime = Random.Range(1f, 1.5f);
        StartCoroutine(StartSpawn());

        MageCollider.enabled = false;
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

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveX, moveY, -2), 0.05f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MageAttack")
        {
            if (other.GetComponent<MageAttack>().reverse == true)
            {
                AttackSound.Play();
                MageHealth();

                if (CurrentPosition.y > PlayerPosition.y)
                {
                    transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y + 0.25f);
                }
                else if (CurrentPosition.y < PlayerPosition.y)
                {
                    transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y - 0.25f);
                }
            }
        }

        if (other.tag != "Enemy" && other.tag != "Wall" && other.tag != "Keyvi" && other.tag != "Obstacle" && other.tag != "Water" && other.tag != "MageAttack")
        {
            if (other.tag != "Hitbox")
            {
                if (other != PlayerCollider)
                {
                    StopAllCoroutines();

                    //StartCoroutine(SlightInvincibility());
                    //MageCollider.enabled = false;

                    if (CurrentPosition.y > PlayerPosition.y)
                    {
                        transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y + 0.75f);
                    }
                    else if (CurrentPosition.y < PlayerPosition.y)
                    {
                        transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y - 0.75f);
                    }

                    //StartCoroutine(SlightInvincibility());
                    AttackSound.Play();
                    MageHealth();

                    StartCoroutine(HitSpawn());
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
        yield return new WaitForSeconds(5f);
        MageCollider.enabled = true;
    }

    private IEnumerator Spawn()
    {
        moveX = Random.Range(-6.5f, 6.5f);
        moveY = Random.Range(-3.5f, 1f);

        transform.position = new Vector3(moveX, moveY, -2);
        animator.SetInteger("Mage", 0);
        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("Mage", 1);
        yield return new WaitForSeconds(1f + attackTime);
        attackCount = 0;

        for (int i = 0; i < 3; i++)
        {
            var missile = Instantiate(MageAttack, new Vector3(transform.position.x, transform.position.y + 0.5f, -2), Quaternion.identity);
            missile.GetComponent<MageAttack>().MageX = transform.position.x;
            missile.GetComponent<MageAttack>().MageY = transform.position.y - 0.5f;
            yield return new WaitForSeconds(attackTime);
        }

        StartCoroutine(DeSpawn());
    }

    private IEnumerator HitSpawn()
    {
        animator.SetInteger("Mage", 2);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Spawn());
    }

    private IEnumerator DeSpawn()
    {
        yield return new WaitForSeconds(2f);
        animator.SetInteger("Mage", 2);
        yield return new WaitForSeconds(1.5f);
        animator.SetInteger("Mage", 0);
        StartCoroutine(Spawn());
    }

    /*private IEnumerator Attack()
    {
        Instantiate(MageAttack, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
        attackCount++;
        attackReady = false;
        yield return new WaitForSeconds(attackTime);

        if (attackCount >= 3)
        {
            StartCoroutine(DeSpawn());
        }
        else
            attackReady = true;
    }*/
}