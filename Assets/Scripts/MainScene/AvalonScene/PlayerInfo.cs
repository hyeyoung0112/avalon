using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance;

    private string playerName;
    public string GetName() { return playerName; }

    private string role = "";
    public string GetRole() { return role; }
    public void SetRole(string mrole) { role = mrole; }
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 이름 설정
        playerName = PhotonNetwork.player.NickName;
        // 서버에서 랜덤으로 배정한 역할을 role에 대입

    }
}