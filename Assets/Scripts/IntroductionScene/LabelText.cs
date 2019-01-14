using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class LabelText : MonoBehaviour
{
    public Text m_Text;
    int stringIndex;
    StringBuilder m_Builder;
    float ElapsedTime;
    int m_EndIndex;
    int m_ListIndex;
    string[] m_DialogList;
    string[] m_DialogList1 = { "잦은 외적의 침입으로 브리튼 왕국의 운명이 바람 앞의 촛불 같았던 암울한 시기.\n바위에 박힌 검을 뽑은 청년이 나타나면서 왕국에 한 줄기의 희망이 찾아온다.\n용맹스러운 왕이자, 전사이기도 한 아서 왕, 그리고 그 명성을 쫓아 충성을 맹세한 기사들.\n브리튼은 원탁의 기사라고 불린, 지위의 고저를 막론하고 같은 위치에 선 기사들의 활약으로 위기에서 벗어나 번영의 길로 나아가는 중이었다.\n그러나 왕국에는 불행의 씨앗이 남아있었다. 바로 아서왕에 반대하며 반역을 꿈꾸는 모드레드의 무리들.\n이들은 아서왕이 전쟁에 나선 틈을 타 세력을 불리려한다.\n아서왕의 참모인 멀린은 악의 무리를 알고 있으나 그들의 지도자 모드레드가 누군지 모르는 상황.\n반역을 막기 위해 원탁의 기사들은 성배를 찾기 위한 원정을 떠난다.\n당신은 아서왕의 충실한 신하인가, 반역을 꿈꾸는 세력인가?" };
    string[] m_DialogList2 = { "게임 방법\n아발론 레지스탕스에는 아서왕을 따르는 선의 무리와 모드레드를 따라 반역을 꿈꾸는 악의 무리가 있습니다.\n당신이 아서왕을 따르는 선의 무리라면, 성배를 찾아 떠나는 원정을 성공시켜야 하고, 악의 무리라면 이 원정을 실패시켜야 합니다.\n원정은 총 5번으로 이루어져 있습니다. 3번의 원정을 성공하면 선이, 3번의 원정이 실패하면 악이 승리하게 됩니다. 각 원정때마다 떠나는 기사의 숫자가 모두 다릅니다. 원정을 떠날 기사는 그 라운드의 리더가 정하게 됩니다.\n라운드의 리더는 각 플레이어가 돌아가며 맡게 됩니다. 리더가 원정을 떠날 기사를 모두 정하면, 기사들은 공개 투표를 합니다. 찬성이 과반수 이상일 경우 원정을 떠나게 되고, 아닐 경우 원정을 떠나지 않은 채 하루가 지나고 다음 라운드로 넘어가며 리더가 바뀌게 됩니다.\n만일 원정을 5일동안 떠나지 않게 된다면 악의 무리가 반란을 일으켜 아서왕의 기사들의 패배로 끝나게 됩니다.\n원정을 떠나는 기사들은 이 원정을 성공시킬지, 실패시킬지 선택할 수 있게 됩니다. 선한 무리는 실패를 선택할 수 없으며, 악의 무리는 성공과 실패 중 하나를 택할 수 있습니다. 이 결과는 누가 무엇을 냈는지는 밝혀지지 않은채 성공과 실패 선택이 각각 몇 명이었는지만 밝혀지게 됩니다. 한 명이라도 실패를 택할 경우 이 원정은 실패로 끝나게 됩니다. 다만 7명 이상이 플레이할 경우 4번째 원정은 두 개 이상의 실패가 나와야 실패하게 됩니다."};
    string[] m_DialogList3 = { "게임 방법\n\n기사들과 악의 무리 중 특별한 능력을 가진 이들이 있습니다.\n\n우선 선의 세력에는 멀린과 퍼시발이 있습니다. 멀린은 모드레드를 제외한 악의 무리들을 알 수 있습니다. 멀린을 지키는 퍼시발은 멀린이 누구인지 알 수 있습니다.\n\n악의 세력에는 암살자, 모드레드, 모르가나, 오베론이 있습니다. 암살자는 3번의 원정이 성공한 후, 멀린을 암살할 기회를 얻게 됩니다. 암살자가 선택한 플레이어가 멀린일 경우, 선의 무리는 패배하게 됩니다. 모드레드는 멀린에게 들키지 않는 능력이 있습니다. 모르가나는 악의 세력이지만 퍼시발이 멀린을 확인할 때 모르가나와 멀린을 둘 다 확인하기 때문에 퍼시발을 헷갈리게 게임을 이끌 수 있습니다. 오베론은 악의 무리이지만, 악의 무리는 누가 오베론인지 알지 못하고, 오베론 또한 누가 악의 무리인지 알 수 없습니다." };
    void Start()
    {
        stringIndex = 0;
        m_DialogList = m_DialogList1;
        m_Builder = new StringBuilder();
        m_Builder.Remove(0, m_Builder.Length);
        ElapsedTime = 1.0f;
        m_EndIndex = 1;
        m_ListIndex = 0;
        m_Builder.Append(m_DialogList[m_ListIndex]);
    }

    void Update()
    {
        m_Text.text = m_Builder.ToString(0, m_EndIndex);

        ElapsedTime += Time.fixedDeltaTime * 7.0f;
        m_EndIndex = (int)ElapsedTime;
        if (m_EndIndex > m_Builder.Length)
        {
            m_EndIndex = m_Builder.Length;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (m_EndIndex == m_Builder.Length)
            {
                m_ListIndex++;
                if (m_ListIndex >= m_DialogList.Length)
                {
                    //  Application.LoadLevel("IngameARScene");
                    m_ListIndex = 0;
                }
                ElapsedTime = 1.0f;
                m_EndIndex = 1;
                m_Builder.Remove(0, m_Builder.Length);
                m_Builder.Append(m_DialogList[m_ListIndex]);
            }
            else
            {
                ElapsedTime = m_Builder.Length;
                m_EndIndex = m_Builder.Length;
            }


        }
    }

    public void OnClick_Next()
    {
        if (m_DialogList[0]==m_DialogList1[0])
        {
            m_DialogList = m_DialogList2;
            return;
        }
        else if (m_DialogList[0] == m_DialogList2[0])
        {
            m_DialogList = m_DialogList3;
            return;
        }
        return;
    }
}