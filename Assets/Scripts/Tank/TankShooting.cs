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

    public Weapon[] m_Weapons;
    private int m_SelectedWeapon;
    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;
    private bool m_Fired;

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

        m_Weapons[m_SelectedWeapon].FireWeapon(m_FireTransform);

        if (Input.GetButtonDown("SwapWeapon"))
        {
            m_SelectedWeapon = (m_SelectedWeapon + 1) % m_Weapons.Length;
        }

        //// Track the current state of the fire button and make decisions based on the current launch force.
        //m_AimSlider.value = m_MinLaunchForce;

        //if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        //{
        //    // max charged, not yet fired
        //    m_CurrentLaunchForce = m_MaxLaunchForce;
        //    Fire();
        //}
        //else if (Input.GetButtonDown(m_FireButton))
        //{
        //    // have we pressed fire for the first time?
        //    m_Fired = false;
        //    m_CurrentLaunchForce = m_MinLaunchForce;

        //    m_ShootingAudio.clip = m_ChargingClip;
        //    m_ShootingAudio.Play();

        //}
        //else if (Input.GetButton(m_FireButton) && !m_Fired)
        //{
        //    // Holding the fire button, not yet fired
        //    m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

        //    m_AimSlider.value = m_CurrentLaunchForce;

        //}
        //else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        //{
        //    // released the button, having not fired yet
        //    Fire();
        //}
    }


    [Client]
    public void Fire()
    {
        // Update fired flag
        m_Fired = true;

        // Fire from server
        CmdFire(m_FireTransform.position, m_FireTransform.rotation, m_CurrentLaunchForce * m_FireTransform.forward);

        // Play audio
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        // Reset
        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    #endregion


    #region Server

    [Command]
    private void CmdFire(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        // Launch the shell.
        GameObject shellObject = objectPooler.SpawnFromPool("Shell", position, rotation);
        
        if (shellObject != null)
        {
            Rigidbody shellInstance = shellObject.GetComponent<Rigidbody>();
            shellInstance.velocity = velocity;

            NetworkServer.Spawn(shellObject);
        }
    }

    #endregion
}