using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* Summary
 * PlayerListingPrefab에 붙어있는 함수
 * 각 프리팹에 해당하는 PhotonPlayer를 가지고 있다.
 * 이 PhotonPlayer의 이름을 해당 Prefab에 속한 Text에 표시한다. (Inspector에서 text요소를 끌어다놔줘야 한다)
 */
public class PlayerListing : MonoBehaviour
{
    public PhotonPlayer PhotonPlayer { get; private set; }
    GameInfo gameinfo;
    Chat chat;
    [SerializeField]
    private Text _playerName;
    private Text PlayerName
    {
        get { return _playerName; }
    }

    private void Start()
    {
        gameinfo = GameInfo.Instance;
    }

    //PlayerListingLayoutGroup에서 플레이어 리스트를 만들기 위해 쓰는 함수.
    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName;
        print(photonPlayer.NickName+ " entered the room.");
    }

    public void OnClick_Player()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        if (PhotonNetwork.playerName == gameinfo.kingName)
        {
            string playerName = button.GetComponentInChildren<Text>().text;
            print("playername : " + playerName);
            GameObject.FindObjectOfType<Chat>().SendChatMessage("AvalonSelect>>>" + playerName);
        }
    }
}

