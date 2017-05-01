using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource MainMusic;
    [SerializeField] private AudioSource BossMusic;

    void FixedUpdate()
    {
        if (GameObject.Find("Level").GetComponent<LevelHold>().Level == 7)
            StartCoroutine(MusicStop());
    }

    public void TurnOffMusic()
    {
        StartCoroutine(MusicStop());
    }

    private IEnumerator MusicStop()
    {
        yield return new WaitForSeconds(0);

        for (float i = 0.5f; i > 0; i -= 0.1f)
        {
            yield return new WaitForSeconds(0.15f);
            MainMusic.volume = i;
        }

        MainMusic.enabled = false;
    }

    public void TurnOnMusic()
    {
        StartCoroutine(MusicStart());
        BossMusic.Play();
    }

    private IEnumerator MusicStart()
    {
        yield return new WaitForSeconds(0);

        for (float i = 0; i < 0.55; i += 0.1f)
        {
            BossMusic.volume = i;
            yield return new WaitForSeconds(0.25f);
        }
    }
}