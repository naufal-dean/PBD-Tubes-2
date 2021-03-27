using UnityEngine;
using Mirror;

public class Ammo : NetworkBehaviour, IPooledObject
{

    public LayerMask m_TankMask;
    public AudioSource m_ExplosionAudio;
    public float m_MaxDamage;
    public float m_MaxLifeTime;
    public ParticleSystem m_ExplosionParticles;

    #region Server
    [Client]
    protected void Explode(Vector3 position, Quaternion rotation)
    {
        m_ExplosionParticles = ObjectPooler.Instance.SpawnFromPool("ShellExplosion", position, rotation).GetComponent<ParticleSystem>();
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        Invoke(nameof(ExplodeDeactivate), 0.5f);
    }

    [Client]
    protected void ExplodeDeactivate()
    {
        m_ExplosionParticles.gameObject.SetActive(false);
        CmdDeactivate();
    }
    #endregion
    #region Server

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

    #endregion
}
