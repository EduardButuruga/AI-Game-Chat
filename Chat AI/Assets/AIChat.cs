using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

[System.Serializable]
public class Message
{
    public string session_id;
    public string message;
}

public class AIChat : MonoBehaviour
{
    private string apiUrl = "http://127.0.0.1:5000/chat";
    private string sessionId;
    public ChatManager chatManager;
    private void Start()
    {
        sessionId = Guid.NewGuid().ToString(); // Generează un ID unic pentru sesiune
    }

    public IEnumerator SendMessageToAI(string message)
    {
        Message msg = new Message { session_id = sessionId, message = message };
        string jsonData = JsonUtility.ToJson(msg);
        Debug.Log("Sending data: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string response = request.downloadHandler.text;
            Debug.Log("Received response: " + response);
            ProcessResponse(response);
        }
    }

    public void ProcessResponse(string response)
    {
        var responseObject = JsonUtility.FromJson<Response>(response);
        Debug.Log(responseObject.response);
        chatManager.ProcessResponse(responseObject.response);
    }

    [System.Serializable]
    private class Response
    {
        public string response;
    }
}