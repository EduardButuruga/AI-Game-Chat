using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatManager : MonoBehaviour
{
    public InputField inputField;
    public Text responseText; // Asigură-te că acest câmp este setat în inspectorul Unity
    public AIChat aiChat;
    public float typingSpeed = 0.05f; // Viteza de scriere

    private void Start()
    {
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnEndEdit(string input)
    {
        Debug.Log("OnEndEdit called with input: " + input);
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("Enter key pressed");
            OnSendButtonClicked();
        }
    }

    public void OnSendButtonClicked()
    {
        string message = inputField.text;
        Debug.Log("Message to send: " + message);
        if (!string.IsNullOrEmpty(message))
        {
            StartCoroutine(aiChat.SendMessageToAI(message));
        }
        else
        {
            Debug.LogError("Input message is empty.");
        }
    }

    public void ProcessResponse(string response)
    {
        Debug.Log("Updating UI with response: " + response);
        StartCoroutine(TypeResponse(response)); // Începe corutina pentru a afișa textul treptat
    }

    private IEnumerator TypeResponse(string response)
    {
        responseText.text = "";
        foreach (char letter in response.ToCharArray())
        {
            responseText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}