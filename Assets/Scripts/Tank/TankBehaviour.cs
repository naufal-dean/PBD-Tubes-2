using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBehaviour : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetColor))]
    public Color m_PlayerColor;
    
    public Vector3 m_SpawnPointPosition;
    public Quaternion m_SpawnPointRotation;

    [HideInInspector] [SyncVar(hook = nameof(Setup))]
    public int m_PlayerNumber;
    
    [HideInInspector] [SyncVar(hook = nameof(SetControl))]
    public bool m_Control;
    
    [HideInInspector]
    public string m_ColoredPlayerText;
    
    [HideInInspector]
    public int m_Wins;


    private TankMovement m_Movement;
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;


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
    public void SetCameraTarget()
    {
        if (isLocalPlayer)
        {
            ((NetworkManagerTank) NetworkManager.singleton).m_CameraControl.m_Targets =
                new Transform[] { transform };
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
    public void SetSpawnPoint(Vector3 position, Quaternion rotation)
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
    public void Reset()
    {
        transform.position = m_SpawnPointPosition;
        transform.rotation = m_SpawnPointRotation;

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}