using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobFactory : NetworkBehaviour
{
    private ObjectPooler objectPooler;


    #region Singleton

    public static MobFactory Instance;

    #endregion


    private void Awake()
    {
        Instance = this;
        objectPooler = ObjectPooler.Instance;
    }


    #region Server

    [Command]
    public void CmdSpawnMob(TankBehaviour tankOwner)
    {
        Vector3 ownerPosition = tankOwner.gameObject.transform.position;
        Quaternion ownerRotation = tankOwner.gameObject.transform.rotation;

        for (int i = 0; i < tankOwner.mobDictionary["Soldier"]; i++)
        {
            SpawnSoldier(ownerPosition, ownerRotation, tankOwner);
        }

        for (int i = 0; i < tankOwner.mobDictionary["MobBear"]; i++)
        {
            SpawnMobBear(ownerPosition, ownerRotation, tankOwner);
        }
    }

    [Server]
    private void SpawnSoldier(Vector3 position, Quaternion rotation, TankBehaviour tankOwner)
    {

        GameObject soldierObject = objectPooler.SpawnFromPool("Soldier", position, rotation);

        if (soldierObject != null)
        {
            NetworkServer.Spawn(soldierObject);

            soldierObject.GetComponent<SoldierMovement>().m_TankOwner = tankOwner;
            soldierObject.GetComponent<SoldierAttack>().m_TankOwner = tankOwner;
        }
    }

    [Server]
    private void SpawnMobBear(Vector3 position, Quaternion rotation, TankBehaviour tankOwner)
    {
        GameObject mobObject = objectPooler.SpawnFromPool("MobBear", position, rotation);

        if (mobObject != null)
        {
            NetworkServer.Spawn(mobObject);

            mobObject.GetComponent<MobMovement>().m_TankOwner = tankOwner;
            mobObject.GetComponent<MobAttack>().m_TankOwner = tankOwner;
        }
    }

    #endregion
}
