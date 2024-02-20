using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpManager : MonoBehaviour
{
    private string fakeApiUrl = "https://my-json-server.typicode.com/JU4NP4260/Actividad2_SistDistribuidos";
    private string RickAndMoryApiUrl = "https://rickandmortyapi.com/api";
    private int imagesDownloadsCounter;

    public List<RawImage> cardsImages = new List<RawImage>();
    public List<TextMeshProUGUI> cardsNames = new List<TextMeshProUGUI>();

    Coroutine sendRequest_GetCharacters;

    public void SendRequest(int userID)
    {

        imagesDownloadsCounter = 0;
        sendRequest_GetCharacters = StartCoroutine(GetUserData(userID));

        //if (sendRequest_GetCharacters == null)
        //{
        //    sendRequest_GetCharacters = StartCoroutine(GetUserData(userID));
        //}

    }

    //public void GetCharatersCorutine()
    //{
    //    StartCoroutine(GetCharacters(1,1));
    //}

    IEnumerator GetUserData (int uid)
    {
        Debug.Log(fakeApiUrl + "/users/" + uid);


        UnityWebRequest request = UnityWebRequest.Get(fakeApiUrl + "/users/" + uid);
        Debug.Log("Getting user data...");
        yield return request.SendWebRequest();

        Debug.Log("Response code: " + request.responseCode);


        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if(request.responseCode == 200)
            {
                UserData user = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                Debug.Log(user.username);
                foreach(int cardID in user.deck)
                {
                    StartCoroutine(GetCharacters(cardID, imagesDownloadsCounter));
                    imagesDownloadsCounter++;
                }
            }
            else
            {
                //Debug.Log(request.downloadHandler.text);
                //Debug.Log("ERROR:" + request.error);
            }
        }
    }

    IEnumerator GetCharacters(int cardId, int deckIndex)
    {
        UnityWebRequest request = UnityWebRequest.Get(RickAndMoryApiUrl + "/character/" + cardId);

        Debug.Log("Getting Characters...");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.responseCode);

            if (request.responseCode == 200)
            {
                Character character = JsonUtility.FromJson<Character>(request.downloadHandler.text);
                cardsNames[deckIndex].text = character.name;
                StartCoroutine(DownloadImage(character.image, deckIndex));              
            }
        }
    }

    IEnumerator DownloadImage(string url, int deckIndex)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            cardsImages[deckIndex].texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
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

[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public string image;
}