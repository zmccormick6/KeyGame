using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private ParticleSystem Finished;
    [SerializeField] private ParticleSystem Finished2;


    void Awake()
    {
        StartCoroutine(Death());
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * -5);
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(3.4f);
        var finished = Instantiate(Finished, transform.position, transform.rotation);
        var finishedTwo = Instantiate(Finished2, transform.position, transform.rotation);
        finished.Play();
        finishedTwo.Play();
    }
}