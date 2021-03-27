using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBehaviour : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetColor))]
    public Color m_PlayerColor;

    [SyncVar(hook = nameof(Setup))]
    [HideInInspector] public int m_PlayerNumber;

    [SyncVar(hook = nameof(SetControl))]
    [HideInInspector] public bool m_Control;

    public int m_cashAmount;

    public Vector3 m_SpawnPointPosition;
    public Quaternion m_SpawnPointRotation;
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public int m_Wins;
    [HideInInspector] public Dictionary<string, int> mobDictionary;


    private TankMovement m_Movement;
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;
    private ObjectPooler objectPooler;


    private void Awake()
    {
        objectPooler = ObjectPooler.Instance;
        mobDictionary = new Dictionary<string, int>();
        mobDictionary["Soldier"] = 0;
        mobDictionary["MobBear"] = 0;
    }


    #region Server

    // TODO: change to command and call from client
    [Server]
    public void CmdSpawnSoldier(Vector3 position, Quaternion rotation)
    {
        GameObject soldierObject = objectPooler.SpawnFromPool("Soldier", position, rotation);

        if (soldierObject != null)
        {
            NetworkServer.Spawn(soldierObject);

            soldierObject.GetComponent<SoldierMovement>().m_TankOwner = this;
            soldierObject.GetComponent<SoldierAttack>().m_TankOwner = this;

            //soldierObject.GetComponent<SoldierMovement>().RpcSetTankOwner(gameObject);
            //soldierObject.GetComponent<SoldierAttack>().RpcSetTankOwner(gameObject);
        }
    }

    // TODO: change to command and call from client
    [Server]
    public void CmdSpawnMob(Vector3 position, Quaternion rotation)
    {
        GameObject mobObject = objectPooler.SpawnFromPool("MobBear", position, rotation);

        if (mobObject != null)
        {
            NetworkServer.Spawn(mobObject);

            mobObject.GetComponent<MobMovement>().m_TankOwner = this;
            mobObject.GetComponent<MobAttack>().m_TankOwner = this;

            //mobObject.GetComponent<MobMovement>().RpcSetTankOwner(gameObject);
            //mobObject.GetComponent<MobAttack>().RpcSetTankOwner(gameObject);
        }
    }

    #endregion


    #region Client

    [Client]
    public void Setup(int oldPlayerNumber, int newPlayerNumber)
    {
        m_Movement = GetComponent<TankMovement>();
        m_Shooting = GetComponent<TankShooting>();
        m_CanvasGameObject = GetComponentInChildren<Canvas>().gameObject;

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;
    }


    [ClientRpc]
    public void IGotMoney()
    {
        Debug.Log("My money now is: " + m_cashAmount);
    }

    [ClientRpc]
    public void RpcSetCameraTarget()
    {
        if (isLocalPlayer)
        {
            ((NetworkManagerTank)NetworkManager.singleton).m_CameraControl.m_Target = transform;
        }
    }


    [Client]
    public void SetColor(Color oldPlayerColor, Color newPlayerColor)
    {
        // TODO: assert that player number is ready
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(newPlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = newPlayerColor;
        }
    }


    [ClientRpc]
    public void RpcSetSpawnPoint(Vector3 position, Quaternion rotation)
    {
        m_SpawnPointPosition = position;
        m_SpawnPointRotation = rotation;
    }


    [Client]
    public void SetControl(bool oldControl, bool newControl)
    {
        if (newControl) EnableControl();
        else DisableControl();
    }


    [Client]
    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    [Client]
    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    [ClientRpc]
    public void RpcReset()
    {
        transform.position = m_SpawnPointPosition;
        transform.rotation = m_SpawnPointRotation;

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    #endregion
}
