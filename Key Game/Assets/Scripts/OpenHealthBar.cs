using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHealthBar : MonoBehaviour
{
    bool once = false;

    void FixedUpdate()
    {
        if (GameObject.Find("Level").GetComponent<LevelHold>().Level == 7)
        {
            if (GameObject.Find("Pure Keyvil").GetComponent<PureKeyvilController>().spawnHealth == true)
            {
                if (once == false)
                {
                    gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    once = true;
                }
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}