using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private string serverUrl;
    private string username, password;
    [SerializeField] TMP_InputField userInput,passwordInput;
    [SerializeField] GameObject loginPanel;
    private void Start()
    {
        serverUrl = SocketManager.Instance.serverUrl;
    }

    public void Login()
    {
        username = userInput.text;
        password = passwordInput.text;
        if (username.Trim().Length <= 0 || password.Trim().Length <= 0)
        {
            Debug.LogError("Username or password is empty");
            return;
        }
        loginPanel.SetActive(false);
        StartCoroutine(LoginRoutine(username, password));
    }

    private IEnumerator LoginRoutine(string username, string password)
    {
        // Prepare form data
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post($"{serverUrl}/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login success: " + www.downloadHandler.text);

                // Example: Parse response and store token/userId
                string response = www.downloadHandler.text;
                PlayerPrefs.SetString("PlayerUserId", response); // store it for later use
                SocketManager.Instance.GetSpawnHandler().HandlePlayerSpawn(response);
            }
            else
            {
                Debug.LogError("Login failed: " + www.error);
                loginPanel.SetActive(true);
            }
        }
    }
}