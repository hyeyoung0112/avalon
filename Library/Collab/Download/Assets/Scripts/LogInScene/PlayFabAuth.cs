using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabAuth : MonoBehaviour
{
    [SerializeField]
    private InputField user;
    [SerializeField]
    private InputField password;
    [SerializeField]
    private InputField email;

    private bool IsAuthenticated = false;

    LoginWithPlayFabRequest loginRequest;
    // Start is called before the first frame update
    void Start()
    {
        email.gameObject.SetActive(false);
    }

    public void Login()
    {
        loginRequest = new LoginWithPlayFabRequest();
        loginRequest.Username = user.text;
        loginRequest.Password = password.text;
        PlayFabClientAPI.LoginWithPlayFab(loginRequest, result => {
            IsAuthenticated = true;
            Debug.Log("You are now logged in.");
            PlayerPrefs.SetString("UserName", user.text);
            SceneManager.LoadScene("IntroductionScene");
        }, error => {
            IsAuthenticated = false;
            email.gameObject.SetActive(true);
            Debug.Log("Error: " + error.ErrorMessage);
        }, null);
    }

    public void Register()
    {
        if (email.text != null)
        {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
            request.Email = email.text;
            request.Username = user.text;
            request.Password = password.text;
            PlayFabClientAPI.RegisterPlayFabUser(request, result =>
            {
                Debug.Log("Successfully Registered");
            }, error =>
            {
                Debug.Log("Error: " + error.ErrorMessage);
            }, null);
        }
    }
}
