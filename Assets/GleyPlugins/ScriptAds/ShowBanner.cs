using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBanner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
    }

}
