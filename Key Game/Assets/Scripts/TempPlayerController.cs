using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    [SerializeField] private AnimationClip Dodge;

    private Animator anim;
    private Collider2D SwordHitbox;
    private Collider2D PlayerHitbox;
    private Collider2D Hitbox;
    SpriteRenderer tempSprite;
    public float Speed;
    private float Health = 6;
    private float movex = 0f;
    private float movey = 0f;
    private float anglex = 0f;
    private float angley = 0f;
    private float angle = 0f;
    float currentTime, previousTime;
    int dodge = 0;
    bool dodgeReady = false;
    bool because = false, please = false;

    public bool dodgeCooldown = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        SwordHitbox = GetComponents<Collider2D>()[0];
        PlayerHitbox = GetComponents<Collider2D>()[1];
        tempSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private IEnumerator HitFlashing()
    {
        tempSprite.color = new Color(1f, 1f, 1f, 0.5f);

        int flashing = 0;

        for (int i = 0; i < 20; i++)
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

    private IEnumerator PlayerInvincibility()
    {
        PlayerHitbox.enabled = false;
        yield return new WaitForSeconds(2);
        PlayerHitbox.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (SwordHitbox.enabled == false)
        {
            if (other != PlayerHitbox)
            {
                if (other.tag != "Door")
                {
                    PlayerHealth();
                    StartCoroutine(PlayerInvincibility());
                    StartCoroutine(HitFlashing());
                }
            }
        }

        if (other.tag == "Door")
        {
            GameManager.GetComponent<DoorSpawn>().CurrentDoor.GetComponent<Collider2D>().enabled = false;
            GameManager.GetComponent<LevelSwitch>().RunNextLevel();
        }
    }

    void FixedUpdate()
    {
        MovementAnimation();
        SwingAnimation();
        PlayerMovement();
        DodgeTime();
    }

    public void PlayerMovement()
    {
        if (anim.GetInteger("Swing") == 0)
        {
            Speed = 3f;
        }
        else if (anim.GetInteger("Swing") == 1)
        {
            Speed = 0f;
        }

        if (Input.GetButton("Fire2"))
        {
            if (dodgeReady == true)
            {
                anim.SetInteger("Dodge", 1);
                //because = true;
                StartCoroutine(DodgeMovementIncrease());
            }
        }
        else
        {
            anim.SetInteger("Dodge", 0);
        }

        if (GameManager.GetComponent<LevelSwitch>().pause != true)
        {
            if (transform.position.x < -6.5)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(-6.49f, transform.position.y);
            }
            else if (transform.position.x > 6.5)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(6.49f, transform.position.y);
            }
            if (transform.position.y < -3.95)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(transform.position.x, -3.94f);
            }
            else if (transform.position.y > 1.25)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(transform.position.x, 1.24f);
            }

            if (transform.position.x > 6.5 && transform.position.y > 1.25)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(6.49f, 1.24f);
            }
            if (transform.position.x > 6.5 && transform.position.y < -3.95)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(6.49f, -3.94f);
            }
            if (transform.position.x < -6.5 && transform.position.y < -3.95)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(-6.49f, -3.94f);
            }
            if (transform.position.x < -6.5 && transform.position.y > 1.25)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(-6.49f, 1.24f);
            }

            GetComponent<Rigidbody2D>().velocity = new Vector2(movex * Speed, movey * Speed);
        }
    }

    public void DodgeTime()
    {
        currentTime = Time.time;

        if (currentTime >= previousTime + 2f)
        {
            dodgeReady = true;
            previousTime = currentTime;
        }
        else if (dodge == 0)
        {
            dodgeReady = true;
        }
        else
        {
            dodgeReady = false;
        }

        Debug.Log(dodge);
    }

    public void MovementAnimation()
    {
        movex = Input.GetAxisRaw("Horizontal");
        movey = Input.GetAxisRaw("Vertical");

        anglex = Input.GetAxisRaw("Horizontal");
        angley = Input.GetAxisRaw("Vertical");

        angle = Mathf.Atan2(anglex, angley) * Mathf.Rad2Deg;

        if (anim.GetInteger("Dodge") != 1)
        {
            //West
            if (movex < 0)
            {
                anim.SetInteger("Direction", 4);
            }
            //East
            else if (movex > 0)
            {
                anim.SetInteger("Direction", 2);
            }
            //North
            else if (movey > 0)
            {
                anim.SetInteger("Direction", 1);
            }
            //South
            else if (movey < 0)
            {
                anim.SetInteger("Direction", 3);
            }
            else
            {
                anim.SetInteger("Direction", 0);
            }
        }
    }

    public void SwingAnimation()
    {
        if (Input.GetButton("Fire1"))
        {
            anim.SetInteger("Swing", 1);
        }
        else
        {
            anim.SetInteger("Swing", 0);
        }
    }

    private IEnumerator DodgeMovementIncrease()
    {
        //if (because == true)
        //{
        PlayerHitbox.enabled = false;
        Speed = 15f;
        dodge = 1;
        //because = false;
        yield return new WaitForSeconds(1f);
        //previousTime = currentTime;
        PlayerHitbox.enabled = true;
        //}
    }

    public void PlayerHealth()
    {
        Health--;
    }
}