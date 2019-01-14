using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class ChatData
{
    public string id;
    public string msg;
};

public class Server : MonoBehaviour
{
    private Server server;
    public Server GetServer() { return server; }
    public SocketIOComponent socket;
    private string serverURL = "http://52.231.25.18:3000";

    protected List<string> chatLog = new List<string>();

    void Awake()
    {
        server = this;

        print("socket connected");

        
        socket.On("connection", (string data)=>{
            print("socket On");

            // Emit raw string data
            socket.Emit("test1234");

            // Emit json-formatted string data
            socket.EmitJson("my other event", @"{ ""my"": ""data"" }");
        });
    }
    
    /*
    void DoOpen()
    {
        if (socket == null)
        {
            socket = IO.Socket(serverURL);
            socket.On(Socket.EVENT_CONNECT, () => {
                lock (chatLog)
                {
                    // Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
                    chatLog.Add("Socket.IO connected.");
                }
            });
            socket.On("chat", (data) => {
                string str = data.ToString();

                ChatData chat = JsonConvert.DeserializeObject<ChatData>(str);
                string strChatLog = "user#" + chat.id + ": " + chat.msg;

                // Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
                lock (chatLog)
                {
                    chatLog.Add(strChatLog);
                }
            });
        }
    }*/
}
