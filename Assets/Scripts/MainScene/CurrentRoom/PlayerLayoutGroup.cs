using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup : MonoBehaviour
{
    //플레이어 이름을 나타내는 UI를 만들어내기 위한 프리팹
    [SerializeField]
    private GameObject _playerListingPrefab;
    private GameObject PlayerListingPrefab
    {
        get { return _playerListingPrefab; }
    }

    //방에 있는 플레이어들을 받아오기 위한 List
    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> PlayerListings
    {
        get { return _playerListings; }
    }

    //방을 처음 만든 사람이 Master Client인데 이 사람이 나가고 방의 Master Client가 바뀔 때 불리는 함수.
    //여기서는 그냥 다 팅기게 만들었다.
    private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        PhotonNetwork.DestroyAll();
        PhotonNetwork.LeaveRoom();
    }

    //플레이어가 방에 들어오면 실행되는 함수.
    private void OnJoinedRoom()
    {
        //원래 생성되어 있던 PlayerListing 프리팹 인스턴스들을 다 지운다.
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        //원래 생성되어 있던 채팅 메시지 프리팹 인스턴스들을 모두 없앤다.
        ChatLayoutGroup[] chatLayoutGroups = FindObjectsOfType<ChatLayoutGroup>();
        foreach (ChatLayoutGroup chatLayoutGroup in chatLayoutGroups)
        {
            chatLayoutGroup.OnLeaveRoom();
        }

        //방에 들어왔을 때의 UI가 맨 앞에 보이게 한다.
        MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling();

        //PhotonNetwork에서 같은 방에 접속한 플레이어 리스트를 받아와서 PlayerListingPrefab을 다시 다 만든다.
        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

    //다른 플레이어가 연결되면 불리는 함수.
    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
    {
        PlayerJoinedRoom(photonPlayer);
    }

    //다른 플레이어가 방을 떠나거나 연결이 끊기면 불리는 함수.
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }
    
    //플레이어가 방에 들어오면 불리는 함수.
    private void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        if (photonPlayer == null)
            return;

        //혹시 그 플레이어가 그 전에 있었던 데이터가 남아있다면 PlayerLeftRoom으로 지운다.
        PlayerLeftRoom(photonPlayer);

        //새로운 PlayerListing 프리팹을 만들어 자신에게 속하게 만든다.
        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        //PlayerListing을 설정하고 이를 리스트에 더한다.
        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPlayer(photonPlayer);

        PlayerListings.Add(playerListing);
    }

    //플레이어가 방을 떠나면 불리는 함수.
    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        //해당 플레이어를 플레이어리스팅 리스트에서 찾는다.
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);

        //해당 플레이어를 찾는다면 그 플레이어에 해당하는 PlayerListingPrefab을 지우고 PlayerListings에서도 삭제한다.
        if (index != -1)
        {
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }
    }

    //이건 튜토리얼에서 쓰려고 만든 거 같던데 우리한테는 안 필요해서 손대다 만 함수...ㅎ
    public void OnClickRoomState()
    {
        if (!PhotonNetwork.isMasterClient) return;

        PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
        PhotonNetwork.room.IsVisible = PhotonNetwork.room.IsOpen;
    }

    //Exit Room 누르면 플레이어가 방을 떠나게 한다.
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
