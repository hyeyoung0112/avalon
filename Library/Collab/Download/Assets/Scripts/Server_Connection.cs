using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
    private Server server;
    public Server GetServer() { return server; }
    
    void Awake()
    {
        server = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
