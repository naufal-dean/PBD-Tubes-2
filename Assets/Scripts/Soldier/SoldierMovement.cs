using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMovement : MonoBehaviour
{
    Transform player;
    //PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;

    public LayerMask m_TankMask;
    public float m_TargetRadius = 10f;

    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;

        //playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Update()
    {
        //Debug.Log("ok");
        //// TODO: remove
        //nav.SetDestination(new Vector3(-3f, 0f, 30f));

        //// Find all the tanks in an area around the shell and damage them.
        //Collider[] targets = Physics.OverlapSphere(transform.position, m_TargetRadius, m_TankMask);

        //for (int i = 0; i < targets.Length; i++)
        //{
        //    Rigidbody targetRigidbody = targets[i].GetComponent<Rigidbody>();

        //    if (!targetRigidbody)
        //        continue;

        //    targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

        //    TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

        //    if (!targetHealth)
        //        continue;

        //    float damage = CalculateDamage(targetRigidbody.position);

        //    targetHealth.m_CurrentHealth = Mathf.Max(targetHealth.m_CurrentHealth - damage, 0);
        //}

        //Explode(gameObject.transform.position, gameObject.transform.rotation);

        //gameObject.SetActive(false);
        //Invoke(nameof(Deactivate), 0.5f);

        ////Memindahkan posisi player
        //if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        //{
        //    nav.SetDestination(player.position);
        //}
        //else //Hentikan moving
        //{
        //    nav.enabled = false;
        //}
    }
}
