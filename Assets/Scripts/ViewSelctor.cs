using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSelctor : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject FP;
    public GameObject TP;
    public levelGen levelGeneration;

    public void ChooseFirst()
    {
        FP.SetActive(true);
        TurnOffUI();
    }

    public void ChooseThird()
    {
        TP.SetActive(true);
        TurnOffUI();
    }

    void TurnOffUI()
    {
        levelGeneration.StartGeneration();
        buttons[0].SetActive(false);
        buttons[1].SetActive(false);
    }
}
