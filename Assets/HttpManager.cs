using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager : MonoBehaviour
{
    private string fakeApiUrl = "";

    IEnumerator GetUserData (int uid)
    {
        UnityWebRequest request = UnityWebRequest.Get(fakeApiUrl + "/users/" + uid);
        yield return request;

        if(request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }

    }
}

[System.Serializable]
public class UserData
{
    public int id;
    public string username;
    public int[] deck;
}