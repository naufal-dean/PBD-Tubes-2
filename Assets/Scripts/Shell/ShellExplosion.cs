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


    [Server]
    public void OnObjectSpawn()
    {
        m_ExplosionParticles = ObjectPooler.Instance.SpawnFromPool("ShellExplosion", gameObject.transform.position).GetComponent<ParticleSystem>();
        Invoke(nameof(Deactivate), m_MaxLifeTime);
        NetworkServer.Spawn(m_ExplosionParticles.gameObject);
    }

    [Server]
    private void Deactivate()
    {
        gameObject.SetActive(false);
        m_ExplosionParticles.gameObject.SetActive(false);
        NetworkServer.UnSpawn(gameObject);
        NetworkServer.UnSpawn(m_ExplosionParticles.gameObject);
    }

    [Server]
    private void Explode(Vector3 position, Quaternion rotation)
    {
        m_ExplosionParticles.transform.position = position;
        m_ExplosionParticles.transform.rotation = rotation;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
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
        Invoke(nameof(Deactivate), 0.5f);
    }

    [Server]
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
}