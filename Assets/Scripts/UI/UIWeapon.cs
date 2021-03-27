using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWeapon : NetworkBehaviour
{
    public TextMeshProUGUI text;
    TankShooting _shooter;

    void Update()
    {
        if (_shooter == null || !_shooter.isLocalPlayer)
        {
            GameObject[] tanks = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject tank in tanks)
            {
                if (tank.GetComponent<TankBehaviour>().isLocalPlayer)
                {
                    _shooter = tank.GetComponent<TankShooting>();
                    break;
                }
            }

        }
        else
        {
            if (_shooter.m_SelectedWeapon == 0)
                text.text = "Tank Shell";
            else if (_shooter.m_SelectedWeapon == 1)
                text.text = "Machine Gun ($" + _shooter.m_MGPrice + ")";
            else if (_shooter.m_SelectedWeapon == 2)
                text.text = "Shotgun ($" + _shooter.m_ShotPrice + ")";
        }

    }

}
