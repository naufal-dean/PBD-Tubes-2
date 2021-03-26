using Mirror;
using UnityEngine;

public class ShellExplosion : NetworkBehaviour, IPooledObject
{
    public LayerMask m_TankMask;
    public AudioSource m_ExplosionAudio;
    public float m_MaxDamage = 100f;
    public float m_ExplosionForce = 1000f;
    public float m_MaxLifeTime = 2f;
    public float m_ExplosionRadius = 5f;
    ParticleSystem m_ExplosionParticles;


    #region Server

    [Server]
    public void OnObjectSpawn()
    {
        Invoke(nameof(Deactivate), m_MaxLifeTime);
    }

    [Server]
    private void Deactivate()
    {
        gameObject.SetActive(false);
        NetworkServer.UnSpawn(gameObject);
    }

    [Command]
    private void CmdDeactivate()
    {
        Deactivate();
    }

    #endregion


    #region Client

    [Client]
    private void Explode(Vector3 position, Quaternion rotation)
    {
        m_ExplosionParticles = ObjectPooler.Instance.SpawnFromPool("ShellExplosion", position, rotation).GetComponent<ParticleSystem>();
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        Invoke(nameof(ExplodeDeactivate), 0.5f);
    }

    [Client]
    private void ExplodeDeactivate()
    {
        m_ExplosionParticles.gameObject.SetActive(false);
        CmdDeactivate();
    }

    [Client]
    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidbody.position);

            targetHealth.m_CurrentHealth = Mathf.Max(targetHealth.m_CurrentHealth - damage, 0);
        }

        Explode(gameObject.transform.position, gameObject.transform.rotation);

        gameObject.SetActive(false);
    }

    [Client]
    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        float damage = relativeDistance * m_MaxDamage;

        damage = Mathf.Max(0f, damage);

        return damage;
    }

    #endregion
}