using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class TankShell : Weapon, IWeapon
{
    public Slider m_AimSlider;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;

    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;


    protected override void Start()
    {
        m_Timer = 0;
        m_FireButton = "Fire";
        objectPooler = ObjectPooler.Instance;
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    #region Client

    // Weapon behaviour
    [Client]
    public override void FireWeapon(Transform m_FireTransform)
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;

        if (CanShoot())
        {
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                // max charged, not yet fired
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire(m_FireTransform);
            }
            else if (Input.GetButtonDown(m_FireButton))
            {
                // have we pressed fire for the first time?
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
            }
            else if (Input.GetButton(m_FireButton) && !m_Fired)
            {
                // Holding the fire button, not yet fired
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                m_AimSlider.value = m_CurrentLaunchForce;
            }
            else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
            {
                // released the button, having not fired yet
                Fire(m_FireTransform);
            }
        }
        
        m_Timer += Time.deltaTime;
    }

    [Client]
    void Fire(Transform m_FireTransform)
    {
        Debug.Log("isClient");
        Debug.Log(isClient);
        Debug.Log("isServer");
        Debug.Log(isServer);

        // Update fired flag
        m_Fired = true;

        // Fire from server
        CmdFire(m_FireTransform.position, m_FireTransform.rotation, m_CurrentLaunchForce * m_FireTransform.forward);

        // Play audio
        if (m_ShootingAudio)
        {
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();
        }

        // Reset
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_Timer = 0;
    }

    #endregion
}