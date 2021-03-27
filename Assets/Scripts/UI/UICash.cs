using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICash : NetworkBehaviour
{
    public TextMeshProUGUI text;
    TankBehaviour _player;

    void Update()
    {
        if (_player == null || !_player.isLocalPlayer)
        {
            GameObject[] tanks = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject tank in tanks)
            {
                if (tank.GetComponent<TankBehaviour>().isLocalPlayer)
                {
                    _player = tank.GetComponent<TankBehaviour>();
                    break;
                }
            }

        } else
        {
            text.text = _player.m_cashAmount + "";
        }

    }

}
