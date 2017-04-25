﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossAttack : MonoBehaviour
{
    //Circle code from http://answers.unity3d.com/questions/1164022/move-a-2d-item-in-a-circle-around-a-fixed-point.html

    private float RotateSpeed = 2f;
    private float Radius;

    private Vector2 _centre;
    private float _angle;

    GameObject PureKeyvil;

    bool move = false, stop = false;

    void Start()
    {
        PureKeyvil = GameObject.Find("Pure Keyvil");
        _centre = new Vector2(0, -1.5f);
        //StartCoroutine(Move());
        StartCoroutine(Destroy());
    }

    void FixedUpdate()
    {
        /*if (PureKeyvil.GetComponent<PureKeyvilController>().attackThree == true)
        {
            move = true;

            if (stop == false)
            {
                StartCoroutine(Destroy());
                stop = true;
            }
        }*/

        //if (move == true)
        {
            _angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            transform.position = _centre + offset;
        }
    }

    public void ChooseRadius(int i)
    {
        Radius = i;
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.75f);
        move = true;

    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}