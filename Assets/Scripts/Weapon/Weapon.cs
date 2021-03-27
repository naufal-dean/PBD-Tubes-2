using Mirror;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : NetworkBehaviour
{
    public AudioSource m_ShootingAudio;
    public float m_FireRate;
    public string m_AmmoType;

    protected string m_FireButton;
    protected float m_Timer;

    protected ObjectPooler objectPooler;

    protected virtual void OnEnable()
    {
        m_Timer = 0;
    }

    public abstract void FireWeapon(Transform m_FireTransform);

    public Weapon()
    {
        Debug.Log("Starto");
        m_FireButton = "Fire";
        objectPooler = ObjectPooler.Instance;
        m_FireRate = 1f;
        m_AmmoType = "Bullet";
    }

    protected bool CanShoot()
    {
        return m_Timer > m_FireRate;
    }

    protected void CmdFire(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        // Launch the shell.

        Debug.Log(objectPooler);
        GameObject ammoObject = objectPooler.SpawnFromPool(m_AmmoType, position, rotation);
        if (ammoObject != null)
        {
            Rigidbody ammoInstance = ammoObject.GetComponent<Rigidbody>();
            ammoInstance.velocity = velocity;

            NetworkServer.Spawn(ammoObject);
        }
    }

}