using UnityEngine;

public class PracticeDDOL : MonoBehaviour
{ 
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

}
