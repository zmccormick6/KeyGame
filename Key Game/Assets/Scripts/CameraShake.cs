using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //Code from: http://unitytipsandtricks.blogspot.com/2013/05/camera-shake.html

    float duration = 0.25f;
    //0.15 for boss
    float magnitude = 0.025f;

    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    public void ShakeCameraBoss()
    {
        duration = 5;
        magnitude = 0.05f;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {

        float elapsed = 0.0f;

        Vector3 originalCamPos = Camera.main.transform.position;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;

            Camera.main.transform.position = new Vector3(x, y, originalCamPos.z);

            yield return null;
        }

        Camera.main.transform.position = originalCamPos;
    }
}