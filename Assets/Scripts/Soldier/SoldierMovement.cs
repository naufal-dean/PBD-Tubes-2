using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMovement : NetworkBehaviour
{
    public LayerMask m_TankMask;
    public float m_TargetRadius = 10f;

    private TankBehaviour m_TankOwner;
    private Animator anim;
    private UnityEngine.AI.NavMeshAgent nav;

    void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Update()
    {
        if (isClient && m_TankOwner)
        {
            nav.SetDestination(m_TankOwner.gameObject.transform.position);
        }
    }


    [ClientRpc]
    public void RpcSetTankOwner(GameObject tankOwner) => m_TankOwner = tankOwner.GetComponent<TankBehaviour>();
}
