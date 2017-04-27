using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHold : MonoBehaviour
{
    public int Level;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}