using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Summary
 * 채팅 내용을 보여주는 Prefab을 Inspector에서 끌어다놔줘야 한다.
 * 받은 메시지를 인수로 OnGetChatMessage()를 실행해주면 채팅 내용이 추가되어 나타난다.
 * 채팅 내용을 보여주는 Vertical Layout Group을 가지는 게임오브젝트에 이 스크립트를 붙여줘야 한다.
 */

public class ChatLayoutGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject _chatListingPrefab;
    public GameObject ChatListingPrefab { get{ return _chatListingPrefab; } }

    private List<ChatListing> _chatListings = new List<ChatListing>();
    private List<ChatListing> ChatListings
    {
        get { return _chatListings; }
    }

    public void OnGetChatMessage(string chatMsg)
    {
        GameObject chatListingObj = Instantiate(ChatListingPrefab);
        chatListingObj.transform.SetParent(transform, false);

        ChatListing chatListing = chatListingObj.GetComponent<ChatListing>();
        chatListing.message = chatMsg;
        chatListing.MakeChat(chatListing);

        ChatListings.Add(chatListing);
    }

    public void OnLeaveRoom()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
