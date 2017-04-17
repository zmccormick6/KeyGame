using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    [SerializeField] private GameObject MageAttack;

    private Collider2D MageCollider;
    private Animator animator;

    int attackCount;
    float attackTime, moveY;
    bool attackReady = false;

    void Start()
    {
        MageCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        attackTime = Random.Range(1f, 3f);
        StartCoroutine(Spawn());
    }

    void FixedUpdate()
    {
        if (attackReady == true)
        {
            if (attackCount < 3)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Spawn()
    {
        animator.SetInteger("Mage", 0);
        moveY = Random.Range(-3.5f, 1f);
        transform.position = new Vector2(transform.position.x, moveY);
        yield return new WaitForSeconds(0.75f);
        attackReady = true;
        attackCount = 0;
    }

    private IEnumerator DeSpawn()
    {
        attackReady = false;
        yield return new WaitForSeconds(3f);
        animator.SetInteger("Mage", 3);
        yield return new WaitForSeconds(3f);
        attackCount = 0;
        attackReady = true;
        StartCoroutine(Spawn());
    }

    private IEnumerator Attack()
    {
        animator.SetInteger("Mage", 2);
        Vector2 Position = new Vector2(transform.position.x, transform.position.y);
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