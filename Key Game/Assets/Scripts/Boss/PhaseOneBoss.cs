using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseOneBoss : MonoBehaviour
{
    //Circle code from http://answers.unity3d.com/questions/1164022/move-a-2d-item-in-a-circle-around-a-fixed-point.html

    private float RotateSpeed = 5f;
    private float Radius = 1f;

    private Vector2 _centre;
    private float _angle;

    private void Start()
    {
        _centre = transform.position;
    }

    private void Update()
    {

        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = _centre + offset;

        if (GameObject.Find("Game Manager").GetComponent<DoorSpawn>().enemyCount <= 0)
            Destroy(gameObject);
    }
}