﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    [SerializeField] private AnimationClip Dodge;
    [SerializeField] private GameObject DeathScreen;
    [SerializeField] private AudioSource Swing;
    [SerializeField] private AudioSource HalfHeart;
    [SerializeField] private AudioSource Death;
    [SerializeField] private AudioSource Dash;

    AudioSource Damage;

    private Animator Heart;
    private Animator anim;
    private Animator DodgeUI;

    private Collider2D SwordHitbox;
    private Collider2D PlayerHitbox;
    private Collider2D Hitbox;

    public int Health = 6;
    private float movex = 0f;
    private float movey = 0f;
    private float anglex = 0f;
    private float angley = 0f;
    private float angle = 0f;

    SpriteRenderer tempSprite;

    Vector2 Joystick;

    private Vector2 _centre;
    private float _angle;

    public bool dodgeCooldown = false;
    public float Speed = 5.5f;

    float currentTime, previousTime;
    int dodge = 0;
    bool dodgeReady = true, water = false;
    bool because = false, please = false, stopSwing = false;

    void Start()
    {
        Heart = GameObject.Find("Hearts").GetComponent<Animator>();
        anim = GetComponent<Animator>();
        DodgeUI = GameObject.Find("DodgeCooldown").GetComponent<Animator>();
        SwordHitbox = GetComponents<Collider2D>()[0];
        PlayerHitbox = GetComponents<Collider2D>()[1];
        tempSprite = GetComponent<SpriteRenderer>();
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
        water = false;
        yield return new WaitForSeconds(2);
        PlayerHitbox.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (SwordHitbox.enabled == false)
        {
            if (other != PlayerHitbox)
            {
                if (other.tag != "Door" && other.tag != "Keyvi" && other.tag != "Obstacle" && other.tag != "Water" && other.tag != "Health" && other.gameObject.name != "Pure Keyvil")
                {
                    Damage.Play();
                    PlayerHealth();
                    StartCoroutine(PlayerInvincibility());
                    StartCoroutine(HitFlashing());

                    if (other.tag != "MageAttack")
                    {
                        other.gameObject.GetComponent<KeeseController>().RunHitPlayer();
                    }

                    GameObject.Find("Main Camera").GetComponent<CameraShake>().ShakeCamera();
                }
            }
        }


        if (other.tag == "Door")
        {
            if (SwordHitbox.enabled == false)
            {
                GameManager.GetComponent<DoorSpawn>().CurrentDoor.GetComponent<Collider2D>().enabled = false;
                GameManager.GetComponent<LevelSwitch>().RunNextLevel();
            }
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
            if (SwordHitbox.enabled == false)
                GetComponent<TempPlayerController>().Speed = 2f;
        }

        if (water == false)
            GetComponent<TempPlayerController>().Speed = 5.5f;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water")
        {
            GetComponent<TempPlayerController>().Speed = 5.5f;
        }
    }

    void FixedUpdate()
    {
        Heart.SetInteger("Heart", Health);

        MovementAnimation();
        //SwingAnimation();
        PlayerMovement();

        if (stopSwing == false)
        {
            if (Input.GetButton("Fire1"))
            {
                stopSwing = true;
                anim.SetInteger("Swing", 1);
               //wing.Play();
                StartCoroutine(SwingSword());
                StartCoroutine(SwingStop());
            }
            else
            {
                anim.SetInteger("Swing", 0);
            }
        }
        /*else
            anim.SetInteger("Swing", 0);*/

        movex = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        movey = Mathf.Round(Input.GetAxisRaw("Vertical"));

        if (movex != 0 || movey != 0)
        {
            if (Input.GetButton("Fire2"))
            {
                if (dodgeReady == true)
                {
                    StartCoroutine(DashSound());
                    StartCoroutine(DodgeMovementIncrease());
                    StartCoroutine(DodgeHitbox());
                }
            }
        }

        //DodgeAnimation();
        DodgeTime();
    }

    private IEnumerator DashSound()
    {
        yield return new WaitForSeconds(0.05f);
        if (!Dash.isPlaying)
            Dash.Play();
    }

    private IEnumerator SwingSword()
    {
        if (!Swing.isPlaying)
        {
            Swing.Stop();
            Swing.Play();
        }

        yield return null;
    }

    public void PlayerMovement()
    {
        /*if (anim.GetInteger("Swing") == 1)
        {
            StartCoroutine(SwingStop());

            if  (!Swing.isPlaying)
                Swing.Play();
        }*/

        /*if (anim.GetInteger("Dodge") == 1)
        {
            StartCoroutine(DodgeHitbox());
            StartCoroutine(DodgeMovementIncrease());
        }*/

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
            if (transform.position.y < -4.35)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(transform.position.x, -4.34f);
            }
            else if (transform.position.y > 0.97f)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(transform.position.x, 0.96f);
            }

            if (transform.position.x > 6.5 && transform.position.y > 0.97f)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(6.49f, 0.96f);
            }
            if (transform.position.x > 6.5 && transform.position.y < -4.35)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(6.49f, -4.34f);
            }
            if (transform.position.x < -6.5 && transform.position.y < -4.35)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(-6.49f, -4.34f);
            }
            if (transform.position.x < -6.5 && transform.position.y > 0.97f)
            {
                GetComponent<Rigidbody2D>().position = new Vector2(-6.49f, 0.96f);
            }

            Joystick = new Vector2(movex, movey);
            Joystick.Normalize();

            GetComponent<Rigidbody2D>().velocity = new Vector2(Joystick.x * Speed, Joystick.y * Speed);
        }
        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    public void MovementAnimation()
    {
        //movex = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        //movey = Mathf.Round(Input.GetAxisRaw("Vertical"));

        if (anim.GetInteger("Dodge") != 1)
        {
            movex = Mathf.Round(Input.GetAxisRaw("Horizontal"));
            movey = Mathf.Round(Input.GetAxisRaw("Vertical"));

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
        if (stopSwing == false)
        {
            if (Input.GetButton("Fire1"))
            {
                stopSwing = true;
                anim.SetInteger("Swing", 1);
                StartCoroutine(SwingStop());
            }
            else
            {
                anim.SetInteger("Swing", 0);
            }
        }
        else
            anim.SetInteger("Swing", 0);

        /*if (Input.GetButton("Fire1"))
        {
            if (stopSwing == false)
            {
                stopSwing = true;
                anim.SetInteger("Swing", 1);
                StartCoroutine(SwingStop());
            }
        }
        else
        {
            anim.SetInteger("Swing", 0);
        }*/
    }

    public void DodgeTime()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            dodgeReady = false;
            DodgeUI.SetFloat("DodgeUI", currentTime);
        }
        else if (currentTime <= 0)
        {
            dodgeReady = true;

            if (anim.GetInteger("Dodge") == 1)
            {
                DodgeUI.SetFloat("DodgeUI", -1);
                currentTime = 2f;
            }
        }
    }

    public void DodgeAnimation()
    {
        if (anim.GetInteger("Direction") != 0)
        {
            if (dodgeReady == true)
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    anim.SetInteger("Dodge", 1);
                }
            }
            else
            {
                anim.SetInteger("Dodge", 0);
            }
        }
        else
        {
            anim.SetInteger("Dodge", 0);
        }
    }

    private IEnumerator SwingStop()
    {
        GetComponent<TempPlayerController>().Speed = 1.5f;
        yield return new WaitForSeconds(0.15f);
        stopSwing = false;
        yield return new WaitForSeconds(0.1f);
        GetComponent<TempPlayerController>().Speed = 5.5f;
    }

    private IEnumerator DodgeHitbox()
    {
        PlayerHitbox.enabled = false;
        yield return new WaitForSeconds(1f);
        PlayerHitbox.enabled = true;
    }

    private IEnumerator DodgeMovementIncrease()
    {
        anim.SetInteger("Dodge", 1);
        yield return new WaitForSeconds(0.1f);
        /*anglex = Input.GetAxisRaw("Horizontal");
        angley = Input.GetAxisRaw("Vertical");
    
        float temp = Mathf.Abs(anglex) / Mathf.Abs(angley);

        Debug.Log(temp);

        Vector2 DodgePosition = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        GetComponent<TempPlayerController>().transform.position = Vector2.MoveTowards(transform.position, DodgePosition, 0.5f);*/

        //transform.position = _centre + offset;
        GetComponent<TempPlayerController>().Speed = 15;
        //GetComponent<TempPlayerController>().Speed = 40;
        yield return new WaitForSeconds(0.25f);
        anim.SetInteger("Dodge", 0);
        GetComponent<TempPlayerController>().Speed = 5.5f;
    }

    public void AddHealth()
    {
        GameObject.Find("HealthSound").GetComponent<AudioSource>().Play(); ;
        Heart.SetInteger("Heart", Health);
    }

    public void PlayerHealth()
    {
        Health--;
        Heart.SetInteger("Heart", Health);

        if (Health == 1)
        {
            HalfHeart.Play();
        }

        if (Health <= 0)
        {
            /*Death.Play();
            Debug.Log(PlayerPrefs.GetInt("Level"));
            SceneManager.LoadScene("GameOver");*/

            StartCoroutine(DeathStuff());
        }
    }

    private IEnumerator DeathStuff()
    {
        Death.Play();
        GameManager.GetComponent<LevelSwitch>().pause = true;
        GameManager.GetComponent<LevelSwitch>().transition = true;

        if (GameObject.Find("Pure Keyvil"))
            GameObject.Find("Pure Keyvil").GetComponent<PureKeyvilController>().Scale();

        GameManager.GetComponent<SoundController>().BossOff();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOver");
    }
}