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

    protected virtual void Start()
    {
        m_Timer = 0;
        m_FireButton = "Fire";
        objectPooler = ObjectPooler.Instance;
    }

    public abstract void FireWeapon(Transform m_FireTransform);


    protected bool CanShoot()
    {
        return m_Timer > m_FireRate;
    }

    [Command]
    protected void CmdFire(Vector3 position, Quaternion rotation, Vector3 velocity)
    {

        //Debug.Log("CMD");
        //Debug.Log("isClient");
        //Debug.Log(isClient);
        //Debug.Log("isServer");
        //Debug.Log(isServer);

        // Fire the gun in server
        GameObject ammoObject = ObjectPooler.Instance.SpawnFromPool(m_AmmoType, position, rotation);
        if (ammoObject != null)
        {
            Rigidbody ammoInstance = ammoObject.GetComponent<Rigidbody>();
            ammoInstance.velocity = velocity;

            NetworkServer.Spawn(ammoObject);
        }
    }


}