using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovement : NetworkBehaviour
{
    public LayerMask m_TankMask;
    public float m_TargetRadius = 15f;

    public TankBehaviour m_TankOwner;
    private Animator anim;
    private UnityEngine.AI.NavMeshAgent nav;

    void Awake()
    {
        //anim = GetComponent<Animator>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    #region Server

    [ServerCallback]
    void Update()
    {
        if (!isServer) return;

        if (!m_TankOwner) return;

        GameObject target = DetectEnemyPosition();

        if (target)
        {
            //print("move enemy");
            //anim.SetTrigger("Run");
            nav.SetDestination(target.transform.position);
        }
        else if (m_TankOwner)
        {
            //print("move ally");

            if (Vector3.Distance(gameObject.transform.position, m_TankOwner.gameObject.transform.position) <= 10f)
            {
                //anim.SetTrigger("Stop");
                nav.SetDestination(gameObject.transform.position);
            }
            else
            {
                //anim.SetTrigger("Run");
                nav.SetDestination(m_TankOwner.gameObject.transform.position);
            }
        }
    }


    [Server]
    private GameObject DetectEnemyPosition()
    {
        GameObject target = null;
        float minDistance = float.MaxValue;

        // Find all the tanks in an area around the soldier.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_TargetRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            TankBehaviour tankBehaviour = targetRigidbody.GetComponent<TankBehaviour>();

            if (!tankBehaviour)
                continue;

            if (tankBehaviour == m_TankOwner)
                continue;

            float newDistance = Vector3.Distance(targetRigidbody.gameObject.transform.position, gameObject.transform.position);
            if (newDistance < minDistance)
            {
                target = targetRigidbody.gameObject;
                minDistance = newDistance;
            }
        }

        return target;
    }

    #endregion


    #region Client

    [ClientRpc]
    public void RpcSetTankOwner(GameObject tankOwner) => m_TankOwner = tankOwner.GetComponent<TankBehaviour>();

    #endregion
}
