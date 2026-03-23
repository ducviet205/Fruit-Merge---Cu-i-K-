using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRateKTA : MonoBehaviour
{
    public int numberShowRate = 1;
    public bool autoShowRate = false;
    private int indexNumberShow;
    private void OnEnable()
    {
        if (autoShowRate)
        {
            indexNumberShow++;
            if (indexNumberShow % numberShowRate == 0)
            {
                BtnShowRate();
            }
        }
    }

    public void BtnShowRate()
    {
        AppraterScript.ShowRaterPopup();
    }
}
