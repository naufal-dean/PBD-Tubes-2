using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAttack : InfantryAttack
{
    public LayerMask m_TankMask;
    public float m_TimeBetweenAttacks = 0.5f;
    public int m_AttackDamage = 10;
    public float m_AttackRange = 15;

    private Animator anim;
    private float m_Timer;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    [ServerCallback]
    void Update()
    {
        if (!isServer) return;

        m_Timer += Time.deltaTime;

        if (m_Timer >= m_TimeBetweenAttacks)
        {
            Attack();
        }
    }


    [Server]
    void Attack()
    {
        // Find all the tanks in an area around the soldier.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_AttackRange, m_TankMask);

        // Damage all enemy tanks in area
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (!targetHealth)
                continue;

            if (targetHealth.gameObject.GetComponent<TankBehaviour>() == m_TankOwner)
                continue;

            Shoot(targetHealth.gameObject.transform.position);
            break;
        }

        // Reset timer
        m_Timer = 0f;
    }


    [Server]
    void Shoot(Vector3 targetPosition)
    {
        // Launch the bullet.
        transform.forward = Vector3.Normalize(targetPosition - transform.position);
        GameObject shellObject = ObjectPooler.Instance.SpawnFromPool("Bullet", transform.position + transform.forward * 5, transform.rotation);


        if (shellObject != null)
        {
            Rigidbody shellInstance = shellObject.GetComponent<Rigidbody>();
            shellInstance.velocity = Vector3.Normalize(targetPosition - transform.position) * 20f;

            NetworkServer.Spawn(shellObject);
        }
    }


    [ClientRpc]
    public void RpcSetTankOwner(GameObject tankOwner) => m_TankOwner = tankOwner.GetComponent<TankBehaviour>();
}
