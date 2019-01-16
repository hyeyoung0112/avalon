using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;
using System;
using System.Text;
using UnityEngine.EventSystems;

/* Summary
 * PhotonChat을 전체적으로 관리하는 class
 * PhotonChat을 초기화하고, 채널 접속을 관리하고, 메시지를 보낸다.
 * 씬에서 빈 GameObject를 만들어서 붙여주고 채팅 보내기 버튼 OnClick에 넣어주기만 하면 됨.
 * */


public class Chat : MonoBehaviour, IChatClientListener
{
    //Photon Chat을 관리하는 ChatClient의 초기화에 필요한 Photon Chat 앱 ID
    private string appId = "fe81b848-3a47-4372-99cb-4551b556e93b";

    //ChatClient의 초기값은 null로 지정
    private ChatClient chatClient = null;

    public StartAvalon startAvalon;

    //Photon Chat에서 들어갈 방(channel)이름
    private string selectedChannelName;

    //현재 플레이어의 ID
    private string userName;
    //현재 플레이어 동의 비동의
    private bool userAgree;

    //현재 방 game information
    GameInfo gameinfo;
   
    //채팅을 입력하는 input Field
    public InputField InputFieldChat;
    private String playerRole="";
    //채팅 내용을 보여줄 vertical layout group을 가지고 있는 걸 인스펙터에서 끌어다 놓으면 됨. (스크롤 뷰에서 Content에 해당)
    public ChatLayoutGroup chatLayoutGroup;

    public GameObject ResultPopUp;
    public Text ResultText;

    //플레이어 이름 가져오기
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        userName = PlayerPrefs.GetString("UserName");
        startAvalon = GameObject.Find("StartButton").GetComponent<StartAvalon>();
        gameinfo = GameInfo.Instance;
       
    }

    //주기적으로 chatClient가 연결되어 있도록 설정
    public void Update()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
        }
    }

    //ChatClient를 초기화하고 들어갈 방 이름을 설정한다.
    public void StartChat(string RoomName)
    {
        //chatClient 초기화
        chatClient = new ChatClient(this);
        //세번째 인수는 로그인 화면에서 PlayFab으로 로그인할 때 생긴 유저 계정 정보.
        chatClient.Connect(this.appId, "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues(userName));
        PlayerPrefs.SetString("roomName", RoomName);
        //들어갈 방 이름 설정. 인수로 받는다.
        selectedChannelName = RoomName;
        Debug.Log("roomname : " + RoomName);
    }

    //ChatClient가 연결되면 실행되는 함수.
    public void OnConnected()
    {
        print("You are now connected to server.");

        //여러 채널을 들을 수 있게 string[]형태로 접속할 채널을 넘겨주고, subscribe하고 service하게 한다.
        //StartChat에서 하지 않는 이유는 그러면 chatClient가 연결되기 전에 채널에 subscribe하려고 해서 에러가 발생함ㅠㅠ
        string[] channel = { selectedChannelName };
        chatClient.Subscribe(channel, 0);
        chatClient.Service();
        print("Connecting to chat as player " + PhotonNetwork.player.NickName);
    }

    //chatClient의 연결이 끊기면 실행되는 함수.
    public void OnDisconnected()
    {
        Debug.Log("channel : " + selectedChannelName);
        print("You are now disconnected to channel.");
    }

    /// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnApplicationQuit.</summary>
    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
        {
            string[] channels = { selectedChannelName };
            this.chatClient.Unsubscribe(channels);
            this.chatClient.Disconnect();
        }
    }

    //엔터 누르면 채팅이 보내지게 하는 함수
    //그치만 엔터를 눌러도 채팅이 보내지지 않지 왜일까ㅋㅋㅋㅋㅋ 엔터 버튼 누르면 되니까 뭐...
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            SendChatMessage(this.InputFieldChat.text);
            this.InputFieldChat.text = "";
            this.InputFieldChat.Select();
            this.InputFieldChat.ActivateInputField();
        }
    }

    //엔터 버튼을 누르면 실행되는 함수.
    //보낼 메시지를 콘솔에 출력하고, 메시지를 보낸 후 InputField의 값을 초기화한다.
    //채팅에서 보내기 버튼의 OnClick에 이 함수를 추가해야 함.
    public void OnClickSend()
    {
        print("Send message: " + this.InputFieldChat.text);
        SendChatMessage(this.InputFieldChat.text);
        this.InputFieldChat.text = "";
        this.InputFieldChat.Select();
        this.InputFieldChat.ActivateInputField();
    }

    //채팅 메시지를 현재 접속한 채널에 보낸다.
    public void SendChatMessage(string inputLine)
    {
        if (inputLine.Contains("Avalon") && inputLine.Contains(">>>"))
        {
            chatClient.PublishMessage(selectedChannelName, inputLine);
        }
        else { chatClient.PublishMessage(selectedChannelName, userName + ": " + inputLine); }
    }

    //채널에서 메시지를 받았을 때 실행되는 함수.
    //채널, 보낸 사람, 메시지들을 받는다.
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //받은 메시지들에 대해 chatLayoutGroup의 함수인 OnGetChatMessage(받은메시지)를 이용해 chatListing을 만들게 한다.
        //ChatLayoutGroup.cs 참조
        for (int i = 0; i < senders.Length; i++)
        {
            string currentString = messages[i].ToString();

            //leader player initialize setting
            if (currentString.Contains("AvalonLeader>>>"))
            {
                int startIndex = currentString.IndexOf("_");
                print("startIndex " + startIndex);
                string kingString = currentString.Substring(startIndex+1);
                print("kingString " + kingString);
                gameinfo.kingName = kingString;
                gameinfo.setKingPlayerName(kingString);
                gameinfo.knightList = new List<string>();
                setSelectedMember();
                chatLayoutGroup.OnGetChatMessage(messages[i].ToString());
                gameinfo.agree = 0;
                gameinfo.agreeCount = 0;
            }

            //get role message
            else if (currentString.Contains("Avalon시스템>>>"))
            {
                if (currentString.Contains("Avalon시스템>>>게임을 시작합니다."))
                {
                    startAvalon.StartGame();
                }
                else
                {
                    print("currentString: " + currentString);
                    
                    int index = currentString.IndexOf(":");
                    print("index: " + index);
                    string getName = currentString.Substring(0, index);
                    print("getName: " + getName);
                    
                    if (getName.Contains(userName))
                    {
                        chatLayoutGroup.OnGetChatMessage(messages[i].ToString());
                        if (playerRole.Length == 0)
                        {
                            //set role for player
                            setRole(currentString);
                        }
                    }
                }
            }

            //select
            else if (currentString.Contains("AvalonSelect>>>"))
            {
                string selectedName = currentString.Substring(15);
      
                if (gameinfo.knightList.Contains(selectedName))
                {
                    print(gameinfo.knightList.Count);
                  
                    gameinfo.knightList.Remove(selectedName);
                    print(gameinfo.knightList.Count);
                    setSelectedMember();
                }
                else
                {
                    print("no");
                    gameinfo.knightList.Add(selectedName);
                    setSelectedMember();
                    chatLayoutGroup.OnGetChatMessage(messages[i].ToString());
                }

            }
            //knight pop up
            else if (currentString.Contains("AvalonKnightPopUp"))
            {
                GameInfo.Instance.ShowKnightPopUp();
            }
            //vote for knight
            else if (currentString.Contains("AvalonKnightVote>>>"))
            {
                if (currentString.Contains("찬성")) gameinfo.agree += 1;
                gameinfo.agreeCount += 1;
                chatLayoutGroup.OnGetChatMessage(messages[i].ToString());
                gameinfo.IsBattleAccepted();
            }
            //vote for battle
            else if (currentString.Contains("AvalonBattleVote>>>"))
            {
                if (currentString.Contains("성공")) gameinfo.successCount += 1;
                gameinfo.battleCount += 1;
                gameinfo.CheckBattle();
            }
            else if (currentString.Equals( "Avalon>>>암살자의 턴입니다.")) {
                if (playerRole.Equals( "암살자"))
                {
                    SendChatMessage("AvalonResult>>>암살자는 바로! " + userName);
                }
            }
            else if (currentString.Contains("AvalonResult>>>") && currentString.Contains("암살자"))
            {
                string assassin = currentString.Substring(24);
                gameinfo.setKingPlayerName(assassin);
             
            }
            else if (currentString.Contains("AvalonAssassin>>>"))
            {
                string RUmerlin = currentString.Substring(17);
                SendChatMessage("AvalonRUMerlin>>>" + RUmerlin);
            }
            else if (currentString.Contains("AvalonRUMerlin>>>"))
            {
                if (currentString.Substring(17) .Equals( userName))
                {
                    if (playerRole.Contains("멀린"))
                    {
                        SendChatMessage("AvalonFinalResult>>>악의 승리!");
                    }
                    else
                    {
                        SendChatMessage("AvalonFinalResult>>>나는 멀린이 아니지롱!!!!!!");
                    }
                }
            }
            else if (currentString.Contains("AvalonFinalResult>>>"))
            {
                if(currentString.Contains("악"))
                {
                    gameinfo.OnEndGame(false);
                }
                else
                {
                    ResultPopUp.SetActive(true);
                    ResultText.text = "반란은 실패했고 아서왕이 무사히 귀환했습니다.";
                }
            }

            //battle pop up이 아닌 메시지는 모두 정상적으로 출력
            else if (!currentString.Contains("BattlePopUp"))
            {
                chatLayoutGroup.OnGetChatMessage(messages[i].ToString());
            }
        }
    }

    public void setRole(string fromMessage)
    {

        //기사
        if (fromMessage.Contains("기사"))
        {
            playerRole = "기사";
        }
        //assasin
        else if (fromMessage.Contains("암살자")) { playerRole = "암살자"; }
        //morgana
        else if (fromMessage.Contains("모르가나")) { playerRole = "모르가나"; }
        //modred
        else if (fromMessage.Contains("모드레드")) { playerRole = "모드레드"; }
        //overon
        else if (fromMessage.Contains("오베론")) { playerRole = "오베론"; }
        //evil
        else if (fromMessage.Contains("악의 세력")) { playerRole = "악의 세력"; }
        //percival
        else if (fromMessage.Contains("퍼시벌")) { playerRole = "퍼시벌"; }
        //merlin
        else if (fromMessage.Contains("멀린")) { playerRole = "멀린"; }

    }

    //Photon chat 실행 중 에러가 발생하면 반환
    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            UnityEngine.Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        else
        {
            UnityEngine.Debug.Log(message);
        }
    }

    //Photon Chat의 상태가 바뀌면 실행됨.
    //IChatClientListener에서 필요한 함수라고 해서 만듦... 쓸 데를 몰라서 일단 그냥 함수 실행됐다고 출력되게만 함.
    public void OnChatStateChange(ChatState state)
    {
        print("chat state changed: " + state.ToString());
    }

    //채널 전체가 아닌 자신에게만 수신된 메시지를 받았을 때 실행되는 함수.
    //메인 씬에서는 실행될 일이 없어서 그냥 프린트만 넣어둠
    //만일 역할 배분 등등을 마스터클라이언트에서 프라이빗 메시지로 보내게 된다면 쓸 일이 있을지도..?
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        print("No private messages allowed in avalon room");
    }

    //플레이어가 채널에 접속하면 실행되는 함수.
    //유저가 어떤 채널에 접속했는지 출력하게 함.
    public void OnSubscribed(string[] channels, bool[] results)
    {
        string msg = "User subscribed to channel ";
        for (int i = 0; i < channels.Length; i++)
        {
            msg += channels[i] + " ";
        }
        print(msg);
    }

    //플레이어가 채널 접속을 해제하면 실행되는 함수.
    //유저가 접속을 해제했다고 출력하게 함
    //근데 출력이 안 뜨는 걸 보니 내가 잘못 생각하고 있는 걸수도...
    public void OnUnsubscribed(string[] channels)
    {
        MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
        print("User unsubscribed to channel");
    }

    //유저의 status가 업데이트되면 실행되는 함수...?
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        print(user + " status : " + status.ToString());
    }


    public void selectMember()
    {
        if (gameinfo.isGoingOn)
        {
            int completeNumber = Int32.Parse(GameObject.Find("BattleNumber" + gameinfo.battleIndex).GetComponent<Text>().text);
            if (gameinfo.knightList.Count == completeNumber)
            {
                SendChatMessage("AvalonKnightPopUp");
            }
        }
        else
        {
            if (gameinfo.knightList.Count == 1)
            {
                SendChatMessage("AvalonAssassin>>>"+gameinfo.knightList[0]);
            }
        }
    }

    public void setSelectedMember()
    {
        Text Knightlist = GameObject.Find("KnightNamesText").GetComponent<Text>();
        Knightlist.text = "";
        for (int i = 0; i < gameinfo.knightList.Count; i++)
        {
            Knightlist.text += " " + gameinfo.knightList[i];
        }
    }

   
}