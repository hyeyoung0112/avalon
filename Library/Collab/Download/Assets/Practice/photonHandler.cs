using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photonHandler : MonoBehaviour
{
    public photonButtons photonB;

    private void Awake()
    {
        DontDestroyOnLoad(this.transform);
    }

    public void createNewRoom()
    {
        PhotonNetwork.CreateRoom(photonB.createRoomInput.text, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public void joinOrCreateroom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(photonB.joinRoomInput.text, roomOptions, TypedLobby.Default);
    }

    public void moveScene()
    {
        PhotonNetwork.LoadLevel("MainGame");
    }

    private void OnJoinedRoom()
    {
        moveScene();
        Debug.Log("We are connected to the room!");
    }
}
