using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Global manager will hold buttons, create request and process them for now.
/// </summary>
public class GlobalManager : MonoBehaviour
{

    [SerializeField]
    TMPro.TextMeshProUGUI logText;
    [SerializeField]
    TMPro.TMP_InputField lightNumberImput;

    [SerializeField]
    Slider rSlider;
    [SerializeField]
    Slider gSlider;
    [SerializeField]
    Slider bSlider;

    [SerializeField]
    GameObject ColorPickerPanel;


    private void Start()
    {
        ColorPickerPanel.SetActive(false);
    }


    public void DisplayColorPicker()
    {
        ColorPickerPanel.SetActive(true);

    }

    public void HideColorPicker()
    {
        ColorPickerPanel.SetActive(false);

    }

    public void locateBridge()
    {
        FindObjectOfType<BridgeLocator>().LocateBridge(OnBridgeLocated);
    }

    private void OnBridgeLocated(string ip)
    {
        logText.text = "Bridge found at the following IP : " + ip +".\n Saving address";
        PlayerPrefs.SetString("bridgeIp", ip.Substring(0, ip.IndexOf(":80", System.StringComparison.InvariantCulture)));

    }

    public void CreateUser()
    {
        FindObjectOfType<HueCreateUser>().Request(OnCreateUser);
    }

    void OnCreateUser(CreateUserResponse response)
    {
        Debug.Log("OnCreateUser Created : " + response.IsSuccess);
        Debug.Log("OnCreateUser Error Type : " + response.Error);
        Debug.Log("OnCreateUser Unformatted answer : " + response.Unformatted);
        Debug.Log("OnCreateUser exception : " + response.exception);

        if (response.IsSuccess)
        {
            logText.text = " User succesfully created with  following api key " + response.APIKey + "\n API has bee save in user preferences. You can now use controls functions";
            PlayerPrefs.SetString("APIKey", response.APIKey);
        }
        else
        {
            if (response.Error == CreateUserResponse.TypeOfError.known)
            {
                logText.text = "Cannot Create user.\n reason is : " + response.ErrorDescription; 
            }
            else
            {
                logText.text = "Cannot Create user.\n answer is : " + response.Unformatted + "\n Exception could be : "+ response.exception;
            }
        }
      

    }

    public void DisplayParameters()
    {
        logText.text = "Parameters are \nBridge url :" + Parameters.instance.BaseUrl + "\nFull address is : " + Parameters.instance.BaseUrlWithUser;
    }


    public void LightOnImmediate()
    {
        int lightNumber=0;
        int.TryParse(lightNumberImput.text, out lightNumber);
        string data = "{\"on\":true, \"transitiontime\": 1,\"bri\":60}";
        FindObjectOfType<HueChangeLightState>().Request(lightNumber,data, OnChangedState);

    }
    public void LightOnWithDelay()
    {



        int lightNumber = 0;
        int.TryParse(lightNumberImput.text, out lightNumber);
        string data = "{\"on\":true,\"bri\":60, \"transitiontime\": 50}";
        FindObjectOfType<HueChangeLightState>().Request(lightNumber, data, OnChangedState);



    }
    public void LightOff()
    {
        int lightNumber = 0;
        int.TryParse(lightNumberImput.text, out lightNumber);
        string data = "{\"on\":false, \"transitiontime\": 0}";
        FindObjectOfType<HueChangeLightState>().Request(lightNumber, data, OnChangedState);
    }


    public void OnChangedState(ChangeLightStateResponse response)
    {
        if (response.IsSuccess)
        {
            logText.text = " Light changed state succesfully.";
           
        }
        else
        {
            if (response.Error == ChangeLightStateResponse.TypeOfError.known)
            {
                logText.text = "Cannot change light state.\nReason is : " + response.ErrorDescription;
            }
            else
            {
                logText.text = "Cannot  change light state.\n answer is : " + response.Unformatted + "\n Exception could be : " + response.exception;
            }
        }


    }

    public void SetColor()
    {

        float brightness = 0;
        float hue = 0;
        float saturation = 0;
        Utilities.ConvertFromRGBTOPhilipsColors(rSlider.value, gSlider.value, bSlider.value, out brightness, out hue, out saturation);
        string data = "{\"on\":true, \"transitiontime\": 1,\"bri\":" + (int)brightness + ",\"hue\":" + (int)hue + ",\"sat\":" + (int)saturation + "}";
        int lightNumber = 0;
        int.TryParse(lightNumberImput.text, out lightNumber);
        FindObjectOfType<HueChangeLightState>().Request(lightNumber, data, OnChangedState);
    }




}
