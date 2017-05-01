using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossAttack : MonoBehaviour
{
    //Circle code from http://answers.unity3d.com/questions/1164022/move-a-2d-item-in-a-circle-around-a-fixed-point.html
    [SerializeField] private ParticleSystem Finished;
    [SerializeField] private ParticleSystem Finished2;


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
        StartCoroutine(Destroy());
    }

    void FixedUpdate()
    {
        {
            _angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            transform.position = _centre + offset;

            transform.Rotate(Vector3.forward * -5);
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
        yield return new WaitForSeconds(4f);
        var finished = Instantiate(Finished, transform.position, transform.rotation);
        var finishedTwo = Instantiate(Finished2, transform.position, transform.rotation);
        finished.Play();
        finishedTwo.Play();
        Destroy(gameObject);
    }
}