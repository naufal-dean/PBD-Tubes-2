using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CashBehaviour : NetworkBehaviour, IPooledObject
{
    CashManager _manager;
    float _timer;
    public int _CashValue = 100;

    public AudioSource _powerUpAudio;

    public Light _light;
    public BoxCollider _collider;
    public MeshRenderer[] _renderers;

    [Server]
    public void OnObjectSpawn()
    {
        _manager = CashManager.Instance;
        _light.enabled = true;
        _collider.enabled = true;
        foreach (MeshRenderer render in _renderers)
        {
            render.enabled = true;
        }
    }

    [Server]
    private void Deactivate()
    {
        gameObject.SetActive(false);
        NetworkServer.UnSpawn(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.isTrigger == false)
        {
            TankBehaviour _player = other.gameObject.GetComponent<TankBehaviour>();
            if (_player)
            {
                SendCash(_player);
                if (isLocalPlayer)
                {
                    _powerUpAudio.Play();
                }
                GetMoney();
                Invoke(nameof(Deactivate), 1f);
            }
        }
    }

    private void GetMoney()
    {
        
        _light.enabled = false;
        _collider.enabled = false;
        foreach (MeshRenderer render in _renderers)
        {
            render.enabled = false;
        }
    }

    private void SendCash(TankBehaviour player)
    {
        player.m_cashAmount += _CashValue;
    }

}
