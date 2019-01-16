﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField]
    private Text _roomName;

    private Text RoomName
    {
        get { return _roomName;}
    }

    public void onClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 10
        };

        if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))
        {
            print("LOG>>>>> Create room successfully sent.");
            Chat chat = GameObject.Find("ChatClient").GetComponent<Chat>();
            chat.StartChat(RoomName.text);
        }
        else
        {
            print("LOG>>>>> Create room failed to send");
        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("LOG>>>>> create room failed: " + codeAndMessage[1]);
    }

    private void OnCreateRoom(){
        print("LOG>>>>> Room created successfully.");
    }
}
