using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetwork : Photon.MonoBehaviour
{
    public string gameVersion;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(gameVersion);
    }

    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Fail Join Room");

        RoomOptions option = new RoomOptions();

        option.IsVisible = true;
        option.IsOpen = true;
        option.MaxPlayers = 4;

        PhotonNetwork.CreateRoom("roomName", option, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        Debug.Log("Join Room");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
