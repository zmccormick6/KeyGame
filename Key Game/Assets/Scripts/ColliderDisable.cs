using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDisable : MonoBehaviour
{
    [SerializeField] private Collider2D TopCollider;

    public void StartLevel()
    {
        TopCollider.enabled = true;
    }

    public void EndLevel()
    {
        TopCollider.enabled = false;
    }
}