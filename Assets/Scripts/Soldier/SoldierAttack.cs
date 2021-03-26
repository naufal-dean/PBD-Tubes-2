using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAttack : NetworkBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    //PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
    private TankBehaviour m_TankOwner;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playerHealth = player.GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        // Mendapatkan Enemy health
        //enemyHealth = GetComponent<EnemyHealth>();
    }

    // Callback jika ada suatu object masuk ke dalam trigger
    void OnTriggerEnter(Collider other)
    {
        // Set player in range
        if (other.gameObject == player && other.isTrigger == false)
        {
            playerInRange = true;

        }
    }

    // Callback jika ada object yang keluar dari trigger
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player && other.isTrigger == false)
        {
            playerInRange = false;
        }
    }


    void Update()
    {
        //timer += Time.deltaTime;

        //if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        //{
        //    Attack();
        //}

        //if (playerHealth.currentHealth <= 0)
        //{
        //    anim.SetTrigger("PlayerDead");
        //}
    }


    void Attack()
    {
        //timer = 0f;

        //// Taking damage
        //if (playerHealth.currentHealth > 0)
        //{
        //    playerHealth.TakeDamage(attackDamage);
        //}
    }


    [ClientRpc]
    public void RpcSetTankOwner(GameObject tankOwner) => m_TankOwner = tankOwner.GetComponent<TankBehaviour>();
}
