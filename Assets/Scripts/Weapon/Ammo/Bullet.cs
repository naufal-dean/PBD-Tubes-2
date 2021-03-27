using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : Ammo
{
    #region Client
    [Client]
    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1, m_TankMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (!targetHealth)
                continue;

            targetHealth.m_CurrentHealth = Mathf.Max(targetHealth.m_CurrentHealth - m_MaxDamage, 0);
        }

        Explode(gameObject.transform.position, gameObject.transform.rotation);

        gameObject.SetActive(false);
    }

    #endregion

}
