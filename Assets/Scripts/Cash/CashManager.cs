using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CashManager : NetworkBehaviour
{
    float _timer;

    public float _spawnAreaWidth;

    NetworkManagerTank m_gameManager;
    public WaitForSeconds _spawnInterval;
    public float _intervalTime = 5.0f;

    public WaitForSeconds _spawnDelay;
    public float _delayTime = 10.0f;

    ObjectPooler objectPooler;

    private string m_FireButton;


    #region Singleton
    public static CashManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        m_FireButton = "Fire1";
        m_gameManager = ((NetworkManagerTank)NetworkManager.singleton);

        _spawnInterval = new WaitForSeconds(_intervalTime);
        _spawnDelay = new WaitForSeconds(_delayTime);
        StartCoroutine(SpawnCash());
    }

    void Update()
    {
        
    }

    [Server]
    private void DespawnCash()
    {
        Queue<GameObject> cashPool = objectPooler.poolDictionary["Cash"];
        foreach(GameObject cash in cashPool)
        {
            if (cash.activeSelf)
            {
                cash.SetActive(false);
            }
        }
    }

    [Server]
    private IEnumerator SpawnCash()
    {
        
        while (!m_gameManager.m_GameRunning)
        {
            yield return _spawnDelay;
            DespawnCash();
        }

        float derivedArea = (float)(_spawnAreaWidth * 0.9);

        float xPos = Random.Range(-derivedArea, derivedArea);
        float zPos = Random.Range(-derivedArea, derivedArea);

        CmdSpawnCash(new Vector3(xPos, 1.0f, zPos));

        yield return _spawnInterval;

        if (m_gameManager.m_GameWinner == null)
        {
            StartCoroutine(SpawnCash());
        }
    }

    [Server]
    void CmdSpawnCash(Vector3 position)
    {
        GameObject cashObject = objectPooler.SpawnFromPool("Cash", position, Quaternion.identity);
        NetworkServer.Spawn(cashObject);
    }

}
