using Mirror;
using UnityEngine;

public class Shell: Ammo
{
    public float m_ExplosionForce;
    public float m_ExplosionRadius;

    
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