using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    [SerializeField] private ParticleSystem Finished;
    [SerializeField] private ParticleSystem Finished2;

    GameObject Player;
    Collider2D SwordHitbox;

    AudioSource Explosion;
    AudioSource Reflect;

    Vector2 CurrentPosition, PlayerPosition, StartPosition, ReversePosition;

    public float MageX, MageY;
    public bool reverse = false;
    bool attack = false;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");
        PlayerPosition =  new Vector3(Player.transform.position.x, Player.transform.position.y, -2);
        StartPosition = new Vector3(transform.position.x, transform.position.y, -2);

        Explosion = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        Reflect = GameObject.Find("Reflect").GetComponent<AudioSource>();

        SwordHitbox = Player.GetComponents<Collider2D>()[0];

        StartCoroutine(Destroy());
        StartCoroutine(WaitAttack());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == SwordHitbox)
        {
            reverse = true;
            Reflect.Play();
        }
        else
        {
            if (other.tag == "Player")
            {
                Explosion.Play();
                Particles();
                Destroy(gameObject);
            }
        }

        if (reverse == true)
        {
            if (other.tag == "Enemy")
            {
                Explosion.Play();
                Particles();
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        if (attack == true)
        {
            ReversePosition = new Vector2(MageX, MageY);
            CurrentPosition = new Vector2(transform.position.x, transform.position.y);

            if (reverse == false)
                transform.position = Vector2.MoveTowards(CurrentPosition, PlayerPosition, 0.15f);
            else if (reverse == true)
                transform.position = Vector2.MoveTowards(CurrentPosition, ReversePosition, 0.25f);

            if (CurrentPosition == PlayerPosition)
            {
                Explosion.Play();
                Particles();
                Destroy(gameObject);
            }
            else if (CurrentPosition == ReversePosition)
            {
                Explosion.Play();
                var finished = Instantiate(Finished, ReversePosition, transform.rotation);
                var finishedTwo = Instantiate(Finished2, ReversePosition, transform.rotation);
                finished.Play();
                finishedTwo.Play();
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(0.5f);
        attack = true;
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4f);
        Explosion.Play();
        Particles();
        Destroy(gameObject);
    }

    public void Particles()
    {
        var finished = Instantiate(Finished, transform.position, transform.rotation);
        var finishedTwo = Instantiate(Finished2, transform.position, transform.rotation);
        finished.Play();
        finishedTwo.Play();
    }
}