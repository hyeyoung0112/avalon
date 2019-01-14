using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private string playerName;
    public string GetName() { return playerName; }

    private string role;
    public string GetRole() { return role; }
    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 이름 설정
        playerName = PlayerPrefs.GetString("UserName");
        // 서버에서 랜덤으로 배정한 역할을 role에 대입

    }
}
