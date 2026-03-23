using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRewardvideo : MonoBehaviour
{
   public void ShowRewardAds()
    {
        Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
    }
   
    private void CompleteMethod(bool completed, string advertiser)
    {
        //Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            //give the reward
        }
        else
        {
            //no reward
        }
    }
}
