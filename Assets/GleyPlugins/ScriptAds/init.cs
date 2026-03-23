using System.Collections;
using System.Collections.Generic;
//using Firebase.Analytics;
using UnityEngine;

public class init : MonoBehaviour {
    private void Awake()
    {
        Advertisements.Instance.Initialize();
        //IAPManager.Instance.InitializeIAPManager(InitializeResultCallback);
        //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        //    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        //    var dependencyStatus = task.Result;
        //    if (dependencyStatus == Firebase.DependencyStatus.Available)
        //    {
        //        // Create and hold a reference to your FirebaseApp,
        //        // where app is a Firebase.FirebaseApp property of your application class.
        //        //   app = Firebase.FirebaseApp.DefaultInstance;

        //        // Set a flag here to indicate whether Firebase is ready to use by your app.
        //    }
        //    else
        //    {
        //        UnityEngine.Debug.LogError(System.String.Format(
        //          "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //        // Firebase Unity SDK is not safe to use here.
        //    }
        //});
    //}
//    private void InitializeResultCallback(IAPOperationStatus status, string message, List<StoreProduct>
//shopProducts)
    //{
        //if (status == IAPOperationStatus.Success)
        //{
        //    //IAP was successfully initialized
        //    //loop through all products
        //    for (int i = 0; i < shopProducts.Count; i++)
        //    {
        //        if (shopProducts[i].productName == "YourProductName")
        //        {
        //            //if active variable is true, means that user had bought that product
        //            //so enable access
        //            if (shopProducts[i].active)
        //            {
        //                //yourBoolVariable = true;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    //Debug.Log(“Error occurred ”+message);
        //}
    }
}
