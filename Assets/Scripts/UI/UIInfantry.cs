using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInfantry : NetworkBehaviour
{
    public TextMeshProUGUI text;
    TankInfantry _infantry;

    void Update()
    {
        if (_infantry == null || !_infantry.isLocalPlayer)
        {
            GameObject[] tanks = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject tank in tanks)
            {
                if (tank.GetComponent<TankInfantry>().isLocalPlayer)
                {
                    _infantry = tank.GetComponent<TankInfantry>();
                    break;
                }
            }
        }
        else
        {
            string infantry = _infantry.m_currentInfantry;
            text.text = infantry + " ($" + _infantry.mobDictionary[infantry] + ")";
        }

    }

}
