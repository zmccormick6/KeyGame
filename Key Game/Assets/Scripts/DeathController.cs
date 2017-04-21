using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathController : MonoBehaviour
{
    public void TryAgain()
    {
        GameObject.Find("Game Manager").GetComponent<LevelSwitch>().level--;

        GameObject.Find("Game Manager").GetComponent<LevelSwitch>().TryAgainLevel();
    }

}