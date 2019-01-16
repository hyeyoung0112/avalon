using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    //얘도...튜토리얼에서 쓰려고 해서 따라하다가 엇 필요없는데..? 싶어서 하다 만 함수
    public void OnClickStartSync()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
