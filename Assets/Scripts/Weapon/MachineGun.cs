using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class MachineGun : Weapon, IWeapon
{
    public float m_BulletSpeed = 1;

    #region Client

    protected override void Start()
    {
        m_Timer = 0;
        m_FireButton = "Fire";
        objectPooler = ObjectPooler.Instance;
    }

    [Client]
    public override void FireWeapon(Transform fireTransform)
    {
        if (Input.GetButton(m_FireButton) && CanShoot())
        {
            Fire(fireTransform);
        }
        m_Timer += Time.deltaTime;
    }

    [Client]
    public void Fire(Transform fireTransform)
    {
        //// Fire from server
        //Debug.Log("isClient");
        //Debug.Log(isClient);

        CmdFire(fireTransform.position, fireTransform.rotation, m_BulletSpeed * fireTransform.forward);

        // Play audio
        if (m_ShootingAudio)
            m_ShootingAudio.Play();

        m_Timer = 0;
    }

    #endregion

}
