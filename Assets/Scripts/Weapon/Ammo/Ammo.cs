using UnityEngine;
using Mirror;

public class Ammo : NetworkBehaviour, IPooledObject
{

    public LayerMask m_TankMask;
    public AudioSource m_ExplosionAudio;
    public float m_MaxDamage;
    public float m_MaxLifeTime;
    public ParticleSystem m_ExplosionParticles;

    [Server]
    public void OnObjectSpawn()
    {
        Invoke(nameof(Deactivate), m_MaxLifeTime);
    }

    [Server]
    protected void Deactivate()
    {
        gameObject.SetActive(false);
        NetworkServer.UnSpawn(gameObject);
    }

    [Command]
    protected void CmdDeactivate()
    {
        Deactivate();
    }
}
