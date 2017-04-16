using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    [SerializeField] private AnimationClip Dodge;

    AudioSource Damage;

    private Animator Heart;
    private Animator anim;
    private Collider2D SwordHitbox;
    private Collider2D PlayerHitbox;
    private Collider2D Hitbox;
    SpriteRenderer tempSprite;
    public float Speed = 3f;
    private int Health = 6;
    private float movex = 0f;
    private float movey = 0f;
    private float anglex = 0f;
    private float angley = 0f;
    private float angle = 0f;
    float currentTime, previousTime;
    int dodge = 0;
    bool dodgeReady = true, water = false;
    bool because = false, please = false;

    public bool dodgeCooldown = false;

    void Start()
    {
        Heart = GameObject.Find("Hearts").GetComponent<Animator>();
        anim = GetComponent<Animator>();
        SwordHitbox = GetComponents<Collider2D>()[0];
        PlayerHitbox = GetComponents<Collider2D>()[1];
        tempSprite = gameObject.GetComponent<SpriteRenderer>();
        Damage = GetComponent<AudioSource>();
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
                if (other.tag != "Door" && other.tag != "Keyvi" && other.tag != "Obstacle" && other.tag != "Water")
                {
                    Damage.Play();
                    PlayerHealth();
                    StartCoroutine(PlayerInvincibility());
                    StartCoroutine(HitFlashing());
                    other.gameObject.GetComponent<KeeseController>().RunHitPlayer();
                }
            }
        }

        if (other.tag == "Door")
        {
            GameManager.GetComponent<DoorSpawn>().CurrentDoor.GetComponent<Collider2D>().enabled = false;
            GameManager.GetComponent<LevelSwitch>().RunNextLevel();
        }

        if (other.tag == "Water")
        {
            water = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Water")
        {
            GetComponent<TempPlayerController>().Speed = 1.5f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water")
        {
            GetComponent<TempPlayerController>().Speed = 3f;
        }
    }

    void FixedUpdate()
    {
        MovementAnimation();
        SwingAnimation();
        PlayerMovement();
        DodgeAnimation();
        DodgeTime();
    }

    public void PlayerMovement()
    {
        if (anim.GetInteger("Swing") == 1)
        {
            StartCoroutine(SwingStop());
        }

        if (anim.GetInteger("Dodge") == 1)
        {
            StartCoroutine(DodgeHitbox());
            StartCoroutine(DodgeMovementIncrease());
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
        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
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
        if (Mathf.Round(Input.GetAxisRaw("Fire1")) < 0)
        {
            anim.SetInteger("Swing", 1);
        }
        else
        {
            anim.SetInteger("Swing", 0);
        }
    }

    public void DodgeTime()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            dodgeReady = false;
        }
        else if (currentTime <= 0)
        {
            dodgeReady = true;

            if (anim.GetInteger("Dodge") == 1)
            {
                currentTime = 2f;
            }
        }
    }

    public void DodgeAnimation()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (dodgeReady == true)
            {
                anim.SetInteger("Dodge", 1);
            }
        }
        else
        {
            anim.SetInteger("Dodge", 0);
        }
    }

    private IEnumerator SwingStop()
    {
        GetComponent<TempPlayerController>().Speed = 0;
        yield return new WaitForSeconds(0.15f);
        GetComponent<TempPlayerController>().Speed = 3;
    }

    private IEnumerator DodgeHitbox()
    {
        PlayerHitbox.enabled = false;
        yield return new WaitForSeconds(1f);
        PlayerHitbox.enabled = true;
    }

    private IEnumerator DodgeMovementIncrease()
    {
        GetComponent<TempPlayerController>().Speed = 15;
        yield return new WaitForSeconds(0.15f);
        GetComponent<TempPlayerController>().Speed = 3;
    }

    public void PlayerHealth()
    {
        Health--;
        Heart.SetInteger("Heart", Health);
    }
}