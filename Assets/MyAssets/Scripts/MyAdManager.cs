using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds.Api;

public class MyAdManager : MonoBehaviour
{
    public static int count;

    public void ShowInterstitialAd()
    {
        count++;

        //Debug.Log(count + "Count Value");

        if (count == 2)
        {
            ShowInterAd();
        }
    }

    private void ShowInterAd()
    {
        //if (this.interstitial.IsLoaded())
        //{
        //    this.interstitial.Show();
        //    Debug.Log("VanshInterAds");
        //    count = 0;
        //}
        //else
        //{
        //    this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        //    Debug.Log("VanshInterAdsFailed");
        //    count = 0;
        //}
    }


    //    private InterstitialAd interstitial;
    //    private BannerView bannerView;

    //    public void Start()
    //    {
    //#if UNITY_ANDROID
    //            string appId = "";
    //#elif UNITY_IPHONE
    //                    string appId = "";
    //#else
    //                    string appId = "unexpected_platform";
    //#endif

    //            // Initialize the Google Mobile Ads SDK.
    //            MobileAds.Initialize(appId);

    //            this.RequestBanner();             
    //            this.RequestInterstitial();       
    //    }

    //    private void RequestBanner()
    //    {
    //#if UNITY_ANDROID
    //        string adUnitId = "";
    //#elif UNITY_IPHONE
    //                    string adUnitId = "";
    //#else
    //                    string adUnitId = "unexpected_platform";
    //#endif

    //        // Create a 320x50 banner at the top of the screen.
    //        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

    //        AdRequest request = new AdRequest.Builder().Build();

    //        // Load the banner with the request.
    //        this.bannerView.LoadAd(request);
    //    }

    //    private void RequestInterstitial()
    //    {
    //#if UNITY_ANDROID
    //        string adUnitId = "";
    //#elif UNITY_IPHONE
    //                        string adUnitId = "";
    //#else
    //                        string adUnitId = "unexpected_platform";
    //#endif

    //        // Initialize an InterstitialAd.
    //        this.interstitial = new InterstitialAd(adUnitId);
    //        // Create an empty ad request.
    //        AdRequest request = new AdRequest.Builder().Build();
    //        // Load the interstitial with the request.
    //        this.interstitial.LoadAd(request);
    //    }


    //                                      ////////// Rewarded Ads ///////////////////



    //    private RewardBasedVideoAd rewardedAd;
    //    private string adUnitId = "";

    //    public void Awake()
    //    {
    //        rewardedAd = RewardBasedVideoAd.Instance;

    //        RequestAdd();

    //        // Called when the user should be rewarded for interacting with the ad.
    //        rewardedAd.OnAdRewarded += HandleUserEarnedReward;
    //        // Called when the ad is closed.
    //        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    //    }

    //    public void RequestAdd()
    //    {
    //        AdRequest request = new AdRequest.Builder().Build();

    //        rewardedAd.LoadAd(request, adUnitId);
    //    }

    //    public void ShowRewardedVideoAd()
    //    {
    //        if (this.rewardedAd.IsLoaded())
    //        {
    //            this.rewardedAd.Show();
    //            Debug.Log("ShowAds");
    //        }
    //        else
    //        {
    //            SSTools.ShowMessage("Add is not available right now", SSTools.Position.top, SSTools.Time.twoSecond);
    //        }
    //    }


    //    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    //    {
    //        MonoBehaviour.print(
    //            "HandleRewardedAdFailedToShow event received with message: "
    //                             + args.Message);

    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //        Time.timeScale = 1;
    //    }

    //    public void HandleRewardedAdClosed(object sender, EventArgs args)
    //    {
    //        MonoBehaviour.print("HandleRewardedAdClosed event received");
    //        RequestAdd();
    //    }

    //    public void HandleUserEarnedReward(object sender, Reward args)
    //    {
    //        string type = args.Type;
    //        double amount = args.Amount;
    //        MonoBehaviour.print("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);

    //        Debug.Log("Rewardeddddddddddddddddddddddddddd");
    //    }
}



