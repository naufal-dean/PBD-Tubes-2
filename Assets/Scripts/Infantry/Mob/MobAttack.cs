using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttack : InfantryAttack
{
    public LayerMask m_TankMask;
    public float m_TimeBetweenAttacks = 0.5f;
    public int m_AttackDamage = 10;

    
    public TankBehaviour m_TankOwner;
    private Animator anim;
    private float m_Timer;
    private float m_AttackRange;
    private ParticleSystem m_HitParticles;


    void Awake()
    {
        //anim = GetComponent<Animator>();
        m_AttackRange = GetComponent<SphereCollider>().radius * 2;
        m_HitParticles = GetComponentInChildren<ParticleSystem>();
    }


    #region Server

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
    private void Attack()
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

            DamageEnemy(targetHealth.gameObject);

            RpcPlayEffect(targetHealth.transform.position);
        }

        // Reset timer
        m_Timer = 0f;
    }


    [Server]
    private void DamageEnemy(GameObject targetObject)
    {
        TankHealth targetHealth = targetObject.GetComponent<TankHealth>();

        targetHealth.m_CurrentHealth = Mathf.Max(targetHealth.m_CurrentHealth - m_AttackDamage, 0);
    }

    #endregion


    #region Client

    [ClientRpc]
    private void RpcPlayEffect(Vector3 position)
    {
        m_HitParticles.transform.position = position;
        m_HitParticles.Play();
    }


    [ClientRpc]
    public void RpcSetTankOwner(GameObject tankOwner) => m_TankOwner = tankOwner.GetComponent<TankBehaviour>();

    #endregion
}
