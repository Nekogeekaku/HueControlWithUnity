using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HueChangeLightState : MonoBehaviour
{
    public void Request(int lightNumber, string newState, Action<ChangeLightStateResponse> callback = null)
    {

        StartCoroutine(ChangeLightStateRoutine(lightNumber, newState, Parameters.instance.BaseUrlWithUser, callback));

    }





    private IEnumerator ChangeLightStateRoutine(int lightNumber, string newState, string url, Action<ChangeLightStateResponse> callback = null)
    {
       
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(newState);
        Debug.Log(url + "lights/" + lightNumber + "/state");
        using (UnityWebRequest www = UnityWebRequest.Put(url + "lights/"+lightNumber+"/state", myData))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError|| www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Status Code: " + www.responseCode);
                Debug.Log("response: " + www.downloadHandler.text);



                ChangeLightStateResponse response = null;


                if (!ChangeLightStateResponse.CreateFromJSON(www.downloadHandler.text, out response))
                {
                    Debug.Log("Data not in JSON format");
                    //Nothing more as I create the object an fill in the details. 
                    //But you could like adding some extra processing in your own code
                }



              
                if (callback != null)
                callback(response);


            }
        }




       
        ////JSONUtility does not support list as in Hue format.
        //// For now removing the [] and later will parse JSON with simpleJSON


        //CreateUserResponse response = null;


        //if (!CreateUserResponse.CreateFromJSON(request.downloadHandler.text, out response))
        //{
        //    Debug.Log("Data not in JSON format");
        //    //Nothing more as I create the object an fill in the details. 
        //    //But you could like adding some extra processing in your own code
        //}



        ////HueCreateUserResponse response = HueCreateUserResponse.CreateFromJSon(data);
        //if (callback != null)
            //callback(response);
    }

}
