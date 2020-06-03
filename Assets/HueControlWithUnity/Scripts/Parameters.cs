using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Parameters : Handles data stored
/// </summary>
public class Parameters : MonoBehaviour
{

    public static Parameters instance;


    public string BaseUrl
    {
        get
        {
            if (PlayerPrefs.HasKey("bridgeIp"))
            {
                return PlayerPrefs.GetString("bridgeIp")+":80/api/";
            }
            else
            return "";
        }
       
    }
    public string BaseUrlWithUser
    {
        get
        {
            if (PlayerPrefs.HasKey("APIKey"))
            {
                return BaseUrl+PlayerPrefs.GetString("APIKey") + "/";
            }
            else
                return "";

        }
        set { }
    }

    private void Awake()
    {
        
        int num = FindObjectsOfType<Parameters>().Length;
        if (num > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

}
