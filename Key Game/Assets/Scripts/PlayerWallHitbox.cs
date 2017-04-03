using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Wall")
        {
            //Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
