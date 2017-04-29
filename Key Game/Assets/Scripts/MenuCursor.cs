using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCursor : MonoBehaviour
{
    [SerializeField] private GameObject StartCursor;
    [SerializeField] private GameObject OptionsCursor;
    [SerializeField] private GameObject CreditCursor;
    [SerializeField] private GameObject QuitCursor;

    int position = 0;

    public void FirstArea()
    {
        position = 1;
        ChooseArea();
    }

    public void SecondArea()
    {
        position = 2;
        ChooseArea();
    }

    public void ThirdArea()
    {
        position = 3;
        ChooseArea();
    }

    public void Choose()
    {
        if (Mathf.Round(Input.GetAxisRaw("Vertical")) == -1)
        {
            position = 4;
        }
        else if (Mathf.Round(Input.GetAxisRaw("Vertical")) == 1)
        {
            position = 1;
        }


        ChooseArea();
    }

    public void FourthArea()
    {
        position = 4;
        ChooseArea();
    }

    public void ChooseArea()
    {
        if (position == 1)
        {
            StartCursor.SetActive(true);
            OptionsCursor.SetActive(false);
            CreditCursor.SetActive(false);
            QuitCursor.SetActive(false);
        }
        else if (position == 2)
        {
            StartCursor.SetActive(false);
            OptionsCursor.SetActive(true);
            CreditCursor.SetActive(false);
            QuitCursor.SetActive(false);
        }
        else if (position == 3)
        {
            StartCursor.SetActive(false);
            OptionsCursor.SetActive(false);
            CreditCursor.SetActive(true);
            QuitCursor.SetActive(false);
        }
        else if (position == 4)
        {
            StartCursor.SetActive(false);
            OptionsCursor.SetActive(false);
            CreditCursor.SetActive(false);
            QuitCursor.SetActive(true);
        }
    }
}