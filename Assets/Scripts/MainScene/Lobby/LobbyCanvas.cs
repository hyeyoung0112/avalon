using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour
{
    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }

    public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            Chat chat = GameObject.Find("ChatClient").GetComponent<Chat>();
            print("StartChat: " + roomName);
            chat.StartChat(roomName);
            return;
        }
        else
        {
            print("Join room failed.");
        }
        
    }
}
