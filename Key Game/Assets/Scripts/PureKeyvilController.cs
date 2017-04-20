using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureKeyvilController : MonoBehaviour
{
    [SerializeField] private GameObject BossAttack;
    [SerializeField] private GameObject SecondBossAttack;
    [SerializeField] private GameObject ThirdBossAttack;
    [SerializeField] private GameObject Player;
    [SerializeField] private Collider2D BossCollider;

    AudioSource AttackSound;
    SpriteRenderer tempSprite;

    private Collider2D PlayerCollider;
    private Animator animator;
    private int Health = 5;

    Vector2 CurrentPosition;
    Vector2 PlayerPosition;

    Vector2 TopLeft = new Vector2(-6, 1);
    Vector2 TopRight = new Vector2(6, 1);
    Vector2 MiddleRight = new Vector2(6, -1.5f);
    Vector2 MiddleLeft = new Vector2(-6, -1.5f);
    Vector2 BottomRight = new Vector2(6, -4);
    Vector2 BottomLeft = new Vector2(-6, -4);
    Vector2 Middle = new Vector2(0, -1.5f);

    int temp = 7, lastTemp, attackChoose;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");

        PlayerCollider = Player.GetComponents<BoxCollider2D>()[1];
        animator = GetComponent<Animator>();
        tempSprite = gameObject.GetComponent<SpriteRenderer>();

        AttackSound = GameObject.Find("Attack").GetComponent<AudioSource>();
        StartCoroutine(ChooseAttack());
    }

    void FixedUpdate()
    {
        CurrentPosition = new Vector2(transform.position.x, transform.position.y);
        PlayerPosition = new Vector2(Player.transform.position.x, Player.transform.position.y);

        if (Health <= 0)
        {
            GameObject.Find("Game Manager").GetComponent<DoorSpawn>().EnemyCheck();
            Debug.Log("Dead");
        }

        if (temp == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, TopLeft, 0.1f);
        }
        else if (temp == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, TopRight, 0.1f);
        }
        else if (temp == 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, BottomRight, 0.1f);
        }
        else if (temp == 3)
        {
            transform.position = Vector2.MoveTowards(transform.position, BottomLeft, 0.1f);
        }
        else if (temp == 4)
        {
            transform.position = Vector2.MoveTowards(transform.position, Middle, 0.1f);
        }
        else if (temp == 5)
        {
            transform.position = Vector2.MoveTowards(transform.position, MiddleLeft, 0.1f);
        }
        else if (temp == 6)
        {
            transform.position = Vector2.MoveTowards(transform.position, MiddleRight, 0.1f);
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

                    if (CurrentPosition.y > PlayerPosition.y)
                    {
                        transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y + 0.75f);
                    }
                    else if (CurrentPosition.y < PlayerPosition.y)
                    {
                        transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y - 0.75f);
                    }

                    AttackSound.Play();
                    PureKeyvilHealth();
                }
            }
        }
    }

    public void PureKeyvilHealth()
    {
        Health--;
        StartCoroutine(HitFlashing());
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

    public void ChangePositions()
    {
        temp = Random.Range(0, 7);

        if (lastTemp == temp)
        {
            if (temp == 0 || temp == 7)
            {
                temp = Random.Range(1, 6);
            }
            else
                temp += 1;
        }

        lastTemp = temp;
    }

    private IEnumerator ChooseAttack()
    {
        yield return new WaitForSeconds(3f);

        //int temp = Random.Range(1, 3);
        attackChoose = 2;

        if (attackChoose == 1)
        {
            StartCoroutine(Attack());
        }
        else if (attackChoose == 2)
        {
            StartCoroutine(SecondAttack());
        }
        else if (attackChoose == 3)
        {
            StartCoroutine(ThirdAttack());
        }
    }

    private IEnumerator Attack()
    {
        ChangePositions();

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < 24; i++)
        {
            var Missile = Instantiate(BossAttack, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            Missile.GetComponent<BossAttack>().AttackDirection(i % 4);
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(ChooseAttack());
    }

    private IEnumerator SecondAttack()
    {
        temp = Random.Range(5, 7);

        yield return new WaitForSeconds(3f);

        GameObject Missile;

        for (int i = 0; i < 4; i++)
        {
            if (i % 2 == 0)
                Missile = Instantiate(SecondBossAttack, new Vector3(transform.position.x, transform.position.y - 2f, 0), Quaternion.identity);
            else
                Missile = Instantiate(SecondBossAttack, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            Missile.GetComponent<SecondBossAttack>().Direction(temp);
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine(ChooseAttack());
    }

    private IEnumerator ThirdAttack()
    {

        yield return new WaitForSeconds(0.5f);
    }
}
