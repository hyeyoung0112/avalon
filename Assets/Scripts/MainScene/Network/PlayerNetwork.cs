using UnityEngine;
using UnityEngine.UI;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }

    [SerializeField]
    private Text userID;

    //초기화 될 때 플레이어의 이름을 가져온다.
    private void Awake()
    {
        Instance = this;
        PhotonNetwork.playerName = PlayerPrefs.GetString("UserName");
        PhotonNetwork.player.NickName = PlayerPrefs.GetString("UserName");
        PlayerName = PhotonNetwork.player.NickName;
        print(PlayerName + " accessed to the game.");
        userID.text = PlayerPrefs.GetString("UserName");
    }
}
