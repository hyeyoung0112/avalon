using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public GameInfo Instance;

    bool isGoingOn;
    private string RoomName;
    private int maxPlayer;
    private List<PlayerInfo> Players;
    private string kingName;
    private List<PlayerInfo> Knights;
    private int battleIndex;
    private int dayIndex;

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
    void Start()
    {
        isGoingOn = true;
        // RoomName = 
        // maxPlayer = 
        //Players.Add(player);
        kingName = Players[0].GetName();
        battleIndex = 1;
        dayIndex = 1;
        Knights = new List<PlayerInfo>();
        InitializeBattles(maxPlayer);
        InitializeDays();
        GameObject.Find("KnightPopUp").SetActive(false);
        GameObject.Find("BattlePopUp").SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
    }

    //하루에 해당하는 해 UI를 초기화.
    void InitializeDays()
    {
        dayImages[0].sprite = days[1];
        for (int i = 1; i < 5; i++)
        {
            dayImages[i].sprite = days[2];
        }
    }

    //각 원정에 해당하는 UI를 초기화
    void InitializeBattles(int max_player)
    {
        for (int i = 0; i<5; i++)
        {
            battleImages[i].sprite = battles[2];
        }
        switch (max_player) {
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
    void DayPassed()
    {
        if (dayIndex < 5)
        {
            dayImages[dayIndex - 1].sprite = days[0];
            dayIndex++;
            dayImages[dayIndex - 1].sprite = days[1];
        }
        else
        {
            OnEndGame(false);
            return;
        }
        
    }

    //원정단이 선택되었을 때 기사 UI를 수정한다.
    //모든 플레이어에게 찬성/반대 버튼을 띄운다.
    void KnightsSelected()
    {
        //선택된 플레이어들을 Knights List에 추가한다.


        //Knights에 있는 플레이어 이름을 UI에 띄운다.
        Text KnightText = GameObject.Find("KnightNamesText").GetComponent<Text>();
        string knightText = "";
        foreach (PlayerInfo knight in Knights)
        {
            knightText = knightText + knight.GetName() + " ";
        }
        KnightText.text = knightText;

        //찬성, 반대 창을 띄운다.
        GameObject.Find("KnightPopUp").SetActive(true);
    }

    //원정단 멤버에 동의가 과반수 이상일 때 실행된다.
    //원정단 멤버에게 성공/실패 팝업창이 보여진다.
    void OnBattle()
    {
        
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
    }

    //원정이 실패로 끝났을 때
    void OnBattleFailure()
    {
        //상황에 맞게 원정 이미지를 바꾼다.
        battleImages[battleIndex - 1].sprite = battles[1];
        battleIndex++;
        battleImages[battleIndex - 1].sprite = battles[2];
        //악팀의 승리조건을 확인한다.

    }

    //게임이 끝났을 때 호출됨.
    //선한 팀이 이겼으면 true, 악한 팀이 이겼으면 false를 인자로 받는다.
    void OnEndGame(bool ArthurWon)
    {
        isGoingOn = false;
        //플레이어 역할에 따라 승/패 이미지를 보여주고 그 전 씬으로 돌아간다.

        SceneManager.LoadScene("MainScene");
    }
}
