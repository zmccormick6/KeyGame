using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private ParticleSystem Finished;
    [SerializeField] private ParticleSystem Finished2;

    GameObject Player;
    float moveX, moveY, attackSpeed = 0.1f;
    bool start = false;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");
        StartCoroutine(StartAttack());
        StartCoroutine(DestroyAttack());
    }

    void FixedUpdate()
    {
        if (start == true)
            transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);

        transform.Rotate(Vector3.forward * -5);
    }

    public void AttackDirection(int direction)
    {
        if (direction == 0)
        {
            moveX = -attackSpeed;
            //Left
        }
        else if (direction == 1)
        {
            moveY = attackSpeed;
            //Up
        }
        else if (direction == 2)
        {
            moveX = attackSpeed;
            //Right
        }
        else if (direction == 3)
        {
            moveY = -attackSpeed;
            //Down
        }
        else if (direction == 4)
        {
            moveX = attackSpeed;
            moveY = attackSpeed;
            //Up-Right
        }
        else if (direction == 5)
        {
            moveX = -attackSpeed;
            moveY = attackSpeed;
            //Up-Left
        }
        else if (direction == 6)
        {
            moveX = attackSpeed;
            moveY = -attackSpeed;
            //Down-Right
        }
        else if (direction == 7)
        {
            moveX = -attackSpeed;
            moveY = -attackSpeed;
            //Down-Left
        }
    }

    private IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(0.75f);
        start = true;
    }

    private IEnumerator DestroyAttack()
    {
        yield return new WaitForSeconds(3f);
        var finished = Instantiate(Finished, transform.position, transform.rotation);
        var finishedTwo = Instantiate(Finished2, transform.position, transform.rotation);
        finished.Play();
        finishedTwo.Play();
        Destroy(gameObject);
    }
}
