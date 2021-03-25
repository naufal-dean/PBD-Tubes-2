using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    Transform player;
    //PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;


    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;

        //playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Update()
    {
        Debug.Log("ok");
        // TODO: remove
        nav.SetDestination(new Vector3(-3f, 0f, 30f));

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
