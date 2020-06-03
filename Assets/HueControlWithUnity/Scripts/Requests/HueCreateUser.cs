using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HueCreateUser : MonoBehaviour
{




   
    public void Request(Action<CreateUserResponse> callback = null)
    {

        StartCoroutine(CreateUserRoutine(Parameters.instance.BaseUrl, callback));

    }





    private IEnumerator CreateUserRoutine(string url, Action<CreateUserResponse> callback = null)
    {
        string bodyJsonString = "{\"devicetype\": \"my_hue_app#mydevice_local\"}";


        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("response: " + request.downloadHandler.text);
 
        CreateUserResponse response=null; 


        if (!CreateUserResponse.CreateFromJSON(request.downloadHandler.text, out response))
        {
            Debug.Log("Data not in JSON format"); 
            //Nothing more as I create the object and fill in the details. 
            //But you could like adding some extra processing in your own code
        }

        if (callback != null)
            callback(response);
    }

}
