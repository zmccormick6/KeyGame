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
    SpriteRenderer tempSprite;
    private float Speed = 3f;
    private float Health = 5;
    private float movex = 0f;
    private float movey = 0f;

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
        DodgeAnimation();
        PlayerMovement();
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

        if (GameManager.GetComponent<LevelSwitch>().pause != true)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movex * Speed, movey * Speed);
        }
    }

    public void MovementAnimation()
    {
        movex = Input.GetAxisRaw("Horizontal");
        movey = Input.GetAxisRaw("Vertical");

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

    public void DodgeAnimation()
    {
        if (Input.GetButton("Fire2"))
        {
            anim.SetInteger("Dodge", 1);
        }
        else
        {
            anim.SetInteger("Dodge", 0);
        }
    }

    public void PlayerHealth()
    {
        Health--;
    }
}