using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("LOG>>>>> Connecting to server...");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
    }

    private void OnConnectedToMaster()
    {
        print("LOG>>>>> Connected to master");

        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.playerName = PlayerPrefs.GetString("UserName");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("LOG>>>>> Joined Lobby");

        if (!PhotonNetwork.inRoom) MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
    }
}
