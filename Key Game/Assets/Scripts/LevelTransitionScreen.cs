using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransitionScreen : MonoBehaviour
{
    [SerializeField] private Image Self;
    Color alpha;

    void Start()
    {
        Self.color = alpha;

        alpha.a = 1f;
    }

    void FixedUpdate()
    {
        Self.color = alpha;
        alpha.a -= 1f * Time.deltaTime;
    }
}