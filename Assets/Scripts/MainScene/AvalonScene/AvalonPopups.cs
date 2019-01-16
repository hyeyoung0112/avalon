using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalonPopups : MonoBehaviour
{
    public GameObject KnightPopUp;
    public GameObject BattlePopUp;
    public GameObject ResultPopUp;

    [SerializeField]
    private Chat chatClient;

    public void OnClick_Agree()
    {
        KnightPopUp.SetActive(false);
        string msg = "AvalonKnightVote>>>" + PhotonNetwork.player.NickName + "님이 원정에 찬성했습니다.";
        chatClient.SendChatMessage(msg);
    }

    public void OnClick_Disagree()
    {
        KnightPopUp.SetActive(false);
        string msg = "AvalonKnightVote>>>" +  PhotonNetwork.player.NickName + "님이 원정에 반대했습니다.";
        chatClient.SendChatMessage(msg);
    }

    public void OnCick_Success()
    {
        BattlePopUp.SetActive(false);
        string msg = "AvalonBattleVote>>>원정에 성공했습니다.";
        chatClient.SendChatMessage(msg);
    }

    public void OnClick_Failure()
    {
        BattlePopUp.SetActive(false);
        string msg = "AvalonBattleVote>>>원정에 실패했습니다.";
        chatClient.SendChatMessage(msg);
    }

    public void OnClick_Exit()
    {
        ResultPopUp.SetActive(false);
        MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling();
        ChatLayoutGroup[] chatLayoutGroups = FindObjectsOfType<ChatLayoutGroup>();
        foreach (ChatLayoutGroup chatLayoutGroup in chatLayoutGroups)
        {
            chatLayoutGroup.OnLeaveRoom();
        }
    }
}
