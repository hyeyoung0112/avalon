using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCustom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 10;
    }

    void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
