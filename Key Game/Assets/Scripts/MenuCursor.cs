using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCursor : MonoBehaviour
{
    [SerializeField] private GameObject StartCursor;
    [SerializeField] private GameObject OptionsCursor;
    [SerializeField] private GameObject QuitCursor;

    public void FirstArea()
    {
        StartCursor.SetActive(true);
        OptionsCursor.SetActive(false);
        QuitCursor.SetActive(false);
    }

    public void SecondArea()
    {
        StartCursor.SetActive(false);
        OptionsCursor.SetActive(true);
        QuitCursor.SetActive(false);
    }

    public void ThirdArea()
    {
        StartCursor.SetActive(false);
        OptionsCursor.SetActive(false);
        QuitCursor.SetActive(true);
    }
}