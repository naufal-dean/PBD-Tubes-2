using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class MachineGun : Weapon, IWeapon
{
    public float m_BulletSpeed = 1;

    #region Client

    [Client]
    public override void FireWeapon(Transform fireTransform)
    {
        if (Input.GetButton(m_FireButton) && CanShoot())
        {
            Debug.Log("Harusnya shooting");
            Fire(fireTransform);
        }
        m_Timer += Time.deltaTime;
    }

    [Client]
    public void Fire(Transform fireTransform)
    {
        // Fire from server
        Debug.Log("Firing MG");
        Debug.Log(fireTransform);

        CmdFire(fireTransform.position, fireTransform.rotation, m_BulletSpeed * fireTransform.forward);

        // Play audio
        if (m_ShootingAudio)
            m_ShootingAudio.Play();

        m_Timer = 0;
    }

    #endregion

}
