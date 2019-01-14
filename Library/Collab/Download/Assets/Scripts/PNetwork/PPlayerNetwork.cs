using UnityEngine;

public class PPlayerNetwork : MonoBehaviour
{

    public static PPlayerNetwork Instance;
    public string PlayerName { get; private set; }

    //use this for initialization
    private void Awake()
    {
        Instance = this;

        PlayerName = "Distul#" + Random.Range(1000, 9999);
    }

  
}
