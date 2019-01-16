
using UnityEngine;
using UnityEngine.UI;

/* Summary
 * ChatListingPrefab에 붙어있는 함수
 * 이 message의 이름을 해당 Prefab에 속한 Text에 표시한다.
 */

public class ChatListing : MonoBehaviour
{
    public ChatListing chatListing { get; set; }
    public string message;

    public void MakeChat(ChatListing newChatListing)
    {
        chatListing = newChatListing;
        GetComponent<Text>().text = message;
    }
}
