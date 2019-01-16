using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public static GameInfo Instance;

    public Chat chat;

    public bool isGoingOn;
    private string RoomName;
    private int maxPlayer;

    public PhotonPlayer[] Players;
    public string kingName="";

    public int leaderIndex;
    public int battleIndex;
    public int dayIndex;
    //몇번 성공했는지
    public int battleSuccessNum;
    //현재 knights list
    public List<string> knightList = new List<string>();

    //원정대 멤버 찬반 
    public int agree;
    public bool userAgree;
    public int agreeCount;
    public int battleCount;
    public int successCount;

    public GameObject KnightPopUp;
    public GameObject BattlePopUp;
    public GameObject ResultPopUp;
    public Text ResultText;

    //days[0]: 지난 날의 해
    //days[1]: 오늘의 해
    //days[2]: 미래의 해
    [SerializeField]
    private Sprite[] days;

    [SerializeField]
    private Image[] dayImages;

    //days[0]: 성공한 원정
    //days[1]: 실패한 원정
    //days[2]: 아직 가지 않은 원정
    [SerializeField]
    private Sprite[] battles;

    [SerializeField]
    private Image[] battleImages;

    [SerializeField]
    private Text[] battleTexts;

    // Start is called before the first frame update
    public void InitializeGameInfo()
    {
        Players = PhotonNetwork.playerList;
        isGoingOn = true;
        battleSuccessNum = 0;
        battleCount = 0;
        successCount = 0;
        leaderIndex = 0;
        kingName = Players[leaderIndex].NickName;
        print("kingname : " + kingName);
        battleIndex = 1;
        dayIndex = 1;
        InitializeBattles(Players.Length);
        InitializeDays();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowKnightPopUp()
    {
        KnightPopUp.SetActive(true);
    }

    public void SendKingPlayerName()
    {
        if (PhotonNetwork.masterClient.NickName == PhotonNetwork.player.NickName)
        {
            print("set leader player");
            chat.SendChatMessage("AvalonLeader>>> leader player is _" + Players[leaderIndex].NickName);
            print("leader message : " + "AvalonLeader>>> leader player is _" + Players[leaderIndex].NickName + "_");
        }
    }

    public void setKingPlayerName(string kingName)
    {
        print("setkingplayername : " + kingName);
        GameObject.Find("KingPlayerNameText").GetComponent<Text>().text = kingName;
    }

    //하루에 해당하는 해 UI를 초기화.
    void InitializeDays()
    {
        dayIndex = 1;
        dayImages[0].sprite = days[1];
        for (int i = 1; i < 5; i++)
        {
            dayImages[i].sprite = days[2];
        }
    }

    //각 원정에 해당하는 UI를 초기화
    void InitializeBattles(int playerNum)
    {
        for (int i = 0; i < 5; i++)
        {
            battleImages[i].sprite = battles[2];
        }
        switch (playerNum)
        {
            case 5:
                battleTexts[0].text = "2";
                battleTexts[1].text = "3";
                battleTexts[2].text = "2";
                battleTexts[3].text = "3";
                battleTexts[4].text = "3";
                break;
            case 6:
                battleTexts[0].text = "2";
                battleTexts[1].text = "3";
                battleTexts[2].text = "4";
                battleTexts[3].text = "3";
                battleTexts[4].text = "4";
                break;
            case 7:
                battleTexts[0].text = "2";
                battleTexts[1].text = "3";
                battleTexts[2].text = "3";
                battleTexts[3].text = "4";
                battleTexts[4].text = "4";
                break;
            default:
                battleTexts[0].text = "3";
                battleTexts[1].text = "4";
                battleTexts[2].text = "4";
                battleTexts[3].text = "5";
                battleTexts[4].text = "5";
                break;
        }
    }

    // 원정이 반대로 출발하지 못한 채 날이 지났을 때
    public void DayPassed()
    {
        if (dayIndex < 5)
        {
            dayImages[dayIndex - 1].sprite = days[0];
            dayIndex++;
            dayImages[dayIndex - 1].sprite = days[1];
            leaderIndex = (leaderIndex + 1) % Players.Length;
            SendKingPlayerName();
        }
        else
        {
            OnEndGame(false);
            return;
        }

    }

    //check battle can go on or not
    public void IsBattleAccepted()
    {
        //check whether everyone is done
        if (agreeCount == Players.Length)
        {
            if (agree >= Players.Length / 2 + 1)
            {
                OnBattle();
            }
            else
            {
                DayPassed();
            }
            GameObject.Find("KnightPopUp").SetActive(false);
        }

    }

    //원정단 멤버에 동의가 과반수 이상일 때 실행된다.
    //원정단 멤버에게 성공/실패 팝업창이 보여진다.
    void OnBattle()
    {
        chat.SendChatMessage("AvalonBattlePopUp");
        if (knightList.Contains(PhotonNetwork.player.NickName))
        {
            BattlePopUp.SetActive(true);
        }
        battleCount = 0;
        successCount = 0;
    }

    //check whether battle count should keep going or not
    public void CheckBattle()
    {
        if (battleCount == knightList.Count)
        {
            if (successCount == battleCount)
            {
                OnBattleSuccess();
            }
            else
            {
                OnBattleFailure();
            }
            BattlePopUp.SetActive(false);
        }
    }

    //원정이 성공적으로 끝났을 때
    void OnBattleSuccess()
    {
        //상황에 맞게 원정 이미지를 바꾼다.
        battleImages[battleIndex - 1].sprite = battles[0];
        battleIndex++;
        battleImages[battleIndex - 1].sprite = battles[2];
        //선팀의 승리조건을 확인한다.
        //3번의 원정이 성공한 경우 암살자의 턴으로 바꾼다.
        battleSuccessNum++;
        InitializeDays();
        if (battleSuccessNum >= 3)
        {
            OnEndGame(true);
        }
        else if (battleIndex <= 5)
        {
            leaderIndex = (leaderIndex + 1) % Players.Length;
            SendKingPlayerName();

        }
        else
        {
            OnEndGame(false);
        }
    }

    //원정이 실패로 끝났을 때
    void OnBattleFailure()
    {
        //상황에 맞게 원정 이미지를 바꾼다.
        battleImages[battleIndex - 1].sprite = battles[1];
        battleIndex++;
        battleImages[battleIndex - 1].sprite = battles[2];
        InitializeDays();
        //악팀의 승리조건을 확인한다.
        if (battleIndex - battleSuccessNum > 3)
        {
            OnEndGame(false);
        }
        else if (battleIndex <= 5)
        {
            leaderIndex = (leaderIndex + 1) % Players.Length ;
            SendKingPlayerName();
        }
        else
        {
            OnEndGame(true);
        }

    }

    //게임이 끝났을 때 호출됨.
    //선한 팀이 이겼으면 true, 악한 팀이 이겼으면 false를 인자로 받는다.
    public void OnEndGame(bool ArthurWon)
    {
        isGoingOn = false;
        if (ArthurWon)
        {
            chat.SendChatMessage("암살자는 누구야?!");
            if (PhotonNetwork.isMasterClient) { chat.SendChatMessage("Avalon>>>암살자의 턴입니다."); }
            knightList.Clear();
            chat.setSelectedMember();
        }
        //플레이어 역할에 따라 승/패 이미지를 보여주고 그 전 씬으로 돌아간다.
        else
        {
            ResultPopUp.SetActive(true);
            ResultText.text = "모드레드와 그의 수하들이 성을 점령했습니다!";
        }
    }


    
}