using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : Weapon, IWeapon
{
    public float m_PelletSpeed;

    #region Client

    protected override void Start()
    {
        m_Timer = 0;
        m_FireButton = "Fire";
        objectPooler = ObjectPooler.Instance;
    }

    [Client]
    public override void FireWeapon(Transform m_FireTransform)
    {
        if (Input.GetButton(m_FireButton) && CanShoot())
        {
            Fire(m_FireTransform);
        }
        m_Timer += Time.deltaTime;
    }

    [Client]
    public void Fire(Transform m_FireTransform)
    {
        // Fire from server
        CmdFire(m_FireTransform.position, m_FireTransform.rotation, m_PelletSpeed * m_FireTransform.forward);
        CmdFire(m_FireTransform.position, m_FireTransform.rotation, m_PelletSpeed * m_FireTransform.forward);
        CmdFire(m_FireTransform.position, m_FireTransform.rotation, m_PelletSpeed * m_FireTransform.forward);

        // Play audio
        if (m_ShootingAudio)
            m_ShootingAudio.Play();

        m_Timer = 0;
    }

    #endregion


}
