using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : Ammo
{
    [Client]
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();
        if(!targetRigidbody)
            return;

        TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
        if (!targetHealth)
            return;

        targetHealth.m_CurrentHealth = Mathf.Max(targetHealth.m_CurrentHealth - m_MaxDamage, 0);

        gameObject.SetActive(false);
    }


}
