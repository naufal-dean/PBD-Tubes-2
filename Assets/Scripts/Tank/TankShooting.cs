using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : NetworkBehaviour
{
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;

    // Shell
    public float m_ShellRate;

    // MG
    public float m_BulletSpeed;
    public float m_MGRate;
    public float m_MGPrice;

    // Shotgun
    public float m_PelletSpeed;
    public float m_ShotRate;
    public float m_ShotPrice;

    [HideInInspector] public int m_SelectedWeapon;

    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;
    private bool m_Fired;
    private float m_Timer;

    ObjectPooler objectPooler;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }

    private void Start()
    {
        m_FireButton = "Fire";

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

        objectPooler = ObjectPooler.Instance;

        m_SelectedWeapon = 0;


    }


    #region Client

    [ClientCallback]
    private void Update()
    {
        if (!isLocalPlayer)
            return;

        //m_Weapons[m_SelectedWeapon].FireWeapon(m_FireTransform);

        if (Input.GetButtonDown("SwapWeapon"))
        {
            SwapWeapon();
        }

        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired && CanShootShell())
        {
            // max charged, not yet fired
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(m_FireButton))
        {
            if (m_SelectedWeapon == 0 && CanShootShell())
            {
                // have we pressed fire for the first time?
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();

            } else if (m_SelectedWeapon == 1 && CanShootMG())
            {
                FireMg();
            } else if (m_SelectedWeapon == 2 && CanShootShot())
            {
                FireShotgun();
            }

        }
        else if (Input.GetButton(m_FireButton))
        {
            if (m_SelectedWeapon == 0 && CanShootShell() && !m_Fired)
            {
                // Holding the fire button, not yet fired
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

                m_AimSlider.value = m_CurrentLaunchForce;
            }
            else if (m_SelectedWeapon == 1 && CanShootMG())
            {
                FireMg();
            }
            else if(m_SelectedWeapon == 2 && CanShootShot())
            {
                FireShotgun();
            }

        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired && CanShootShell())
        {
            // released the button, having not fired yet
            Fire();
        }

        m_Timer += Time.deltaTime;
    }

    public bool CanShootMG()
    {
        return m_Timer >= m_MGRate && gameObject.GetComponent<TankBehaviour>().m_cashAmount >= m_MGPrice;
    }

    public bool CanShootShot()
    {
        return m_Timer >= m_ShotRate && gameObject.GetComponent<TankBehaviour>().m_cashAmount >= m_ShotPrice;
    }

    public bool CanShootShell()
    {
        return m_Timer >= m_ShellRate;
    }

    [Client]
    public void SwapWeapon()
    {
        m_SelectedWeapon = (m_SelectedWeapon + 1) % 3;
    }

    [Client]
    public void Fire()
    {
        // Update fired flag
        m_Fired = true;

        // Fire from server
        CmdFire(m_FireTransform.position, m_FireTransform.rotation, m_CurrentLaunchForce * m_FireTransform.forward, "Shell");

        // Play audio
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        // Reset
        m_CurrentLaunchForce = m_MinLaunchForce;

        m_Timer = 0;
    }

    #endregion


    #region Server

    [Command]
    private void CmdFire(Vector3 position, Quaternion rotation, Vector3 velocity, string weapon)
    {
        // Launch the shell.

        GameObject shellObject = objectPooler.SpawnFromPool(weapon, position, rotation);
        
        if (shellObject != null)
        {
            Rigidbody shellInstance = shellObject.GetComponent<Rigidbody>();
            shellInstance.velocity = velocity;

            NetworkServer.Spawn(shellObject);
        }
    }

    [Client]
    public void FireMg()
    {
        CmdFire(m_FireTransform.position, m_FireTransform.rotation, m_BulletSpeed * m_FireTransform.forward, "Bullet");

        // Play audio
        if (m_ShootingAudio)
            m_ShootingAudio.Play();

        int cash = gameObject.GetComponent<TankBehaviour>().m_cashAmount;

        gameObject.GetComponent<TankBehaviour>().m_cashAmount = (int)Mathf.Max(cash - m_MGPrice, 0);
        m_Timer = 0;
    }
    [Client]
    public void FireShotgun()
    {
        CmdFire(m_FireTransform.position, m_FireTransform.rotation, m_PelletSpeed * m_FireTransform.forward, "Pellet");

        // Play audio
        if (m_ShootingAudio)
            m_ShootingAudio.Play();


        int cash = gameObject.GetComponent<TankBehaviour>().m_cashAmount;
        gameObject.GetComponent<TankBehaviour>().m_cashAmount = (int)Mathf.Max(cash - m_ShotPrice, 0);
        m_Timer = 0;
    }

    #endregion
}