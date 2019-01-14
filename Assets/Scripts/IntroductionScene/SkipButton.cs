using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipButton : MonoBehaviour
{
    public void onClick_SkipButton()
    {
        SceneManager.LoadScene("MainScene");
    }
}
