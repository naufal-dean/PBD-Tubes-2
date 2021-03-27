using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public NetworkManagerTank networkManagerTank;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (networkManagerTank.m_GameRunning)
        {
            gameObject.SetActive(false);
        }
    }
}
