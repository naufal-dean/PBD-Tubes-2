using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class TankInfantry : NetworkBehaviour
{
    public Transform m_MobTransform;
    public float m_fireRate = 1f;
    public int m_SoldierPrice;
    public int m_MobPrice;
    [HideInInspector] public string m_currentInfantry;
    [HideInInspector] public Dictionary<string, int> mobDictionary;

    private string m_MobButton;
    private string m_SwapButton;
    private bool m_Mobbed;
    private float m_timer;
    private TankBehaviour m_owner;

    MobFactory mobFactory;

    private void OnEnable()
    {
        m_owner = gameObject.GetComponent<TankBehaviour>();
        
    }


    private void Start()
    {
       
        mobDictionary = m_owner.mobDictionary;
        mobDictionary["Soldier"] = m_SoldierPrice;
        mobDictionary["MobBear"] = m_MobPrice;
        m_MobButton = "DeployInfantry";
        m_SwapButton = "SwapInfantry";

        m_currentInfantry = "Soldier";

        mobFactory = MobFactory.Instance;
    }


    #region Client

    [ClientCallback]
    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown(m_MobButton) && CanDeploy())
        {
            m_timer = 0;
            CmdDeployInfantry();

        }

        if (Input.GetButtonDown(m_SwapButton))
        {
            if (m_currentInfantry == "Soldier")
            {
                m_currentInfantry = "MobBear";
            } else
            {
                m_currentInfantry = "Soldier";
            }
            m_timer = 0;

        }
        m_timer += Time.deltaTime;
    }
    [Command]
    public void CmdDeployInfantry()
    {
        if (mobDictionary[m_currentInfantry] > 0)
        {
            mobFactory.CmdSpawnMob(gameObject, m_currentInfantry, m_MobTransform.transform.position, m_MobTransform.transform.rotation);
            int cash = gameObject.GetComponent<TankBehaviour>().m_cashAmount;
            Debug.Log(mobDictionary[m_currentInfantry]);
            gameObject.GetComponent<TankBehaviour>().m_cashAmount = Mathf.Max(cash - mobDictionary[m_currentInfantry], 0);

        }
    }

    [Client]
    public bool CanDeploy()
    {
        return m_timer > m_fireRate && gameObject.GetComponent<TankBehaviour>().m_cashAmount >= mobDictionary[m_currentInfantry];
    }

    #endregion
}