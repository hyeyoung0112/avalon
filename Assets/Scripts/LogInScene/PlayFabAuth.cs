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
    private string _playFabPlayerIdCache;

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
            RequestPhotonToken(result);
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
                Login();
            }, error =>
            {
                Debug.Log("Error: " + error.ErrorMessage);
            }, null);
        }
    }

    private void RequestPhotonToken(LoginResult obj)
    {
        LogMessage("PlayFab authenticated. Requesting photon token...");

        //We can player PlayFabId. This will come in handy during next step
        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppID
        }, AuthenticateWithPhoton, OnPlayFabError);
    }

    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        LogMessage("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + "  Authentication complete.");

        //We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };

        //We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID (!) and not username.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);    // expected by PlayFab custom auth service

        //We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issues to your during previous step.
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        //We finally tell Photon to use this authentication parameters throughout the entire application.
        PhotonNetwork.AuthValues = customAuth;
    }    

    private void OnPlayFabError(PlayFabError obj)
    {
        LogMessage(obj.GenerateErrorReport());
    }

    public void LogMessage(string message)
    {
        Debug.Log("PlayFab + Photon Example: " + message);
    }
}
