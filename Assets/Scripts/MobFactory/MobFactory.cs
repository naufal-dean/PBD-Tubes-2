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
    }

    #region Client
    [Server]
    public void CmdSpawnMob(GameObject tankOwner, string tag)
    {
        Vector3 ownerPosition = tankOwner.gameObject.transform.position;
        Quaternion ownerRotation = tankOwner.gameObject.transform.rotation;

        TankBehaviour tank = tankOwner.gameObject.GetComponent<TankBehaviour>();

        SpawnMob(tag, ownerPosition, ownerRotation, tank);
        tank.mobDictionary[tag] -= 1;
    }

    #endregion


    #region Server

    [Server]
    private void SpawnMob(string tag, Vector3 position, Quaternion rotation, TankBehaviour tankOwner)
    {
        GameObject mobObject = ObjectPooler.Instance.SpawnFromPool(tag, position, rotation);
    
        if (mobObject != null)
        {
            NetworkServer.Spawn(mobObject);

            mobObject.GetComponent<SoldierMovement>().m_TankOwner = tankOwner;
            mobObject.GetComponent<SoldierAttack>().m_TankOwner = tankOwner;
        }
    }

    #endregion
}
