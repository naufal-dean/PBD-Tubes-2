using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerTank : NetworkManager
{
    public int m_NumRoundsToWin = 5;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public int m_MaxNumPlayers = 2;
    public CameraControl m_CameraControl;
    public Text m_MessageText;
    public List<TankBehaviour> m_Tanks;

    // TODO: remove after spawn point is randomized
    public Transform m_SpawnPoint;


    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    private TankBehaviour m_RoundWinner;
    private TankBehaviour m_GameWinner;


    public override void Start()
    {
        base.Start();

        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
    }


    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        SpawnTank(conn);

        // Start game if player number is enough
        if (numPlayers == m_MaxNumPlayers)
        {
            StartCoroutine(GameLoop());
        }
    }


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        TankBehaviour player = conn.identity.gameObject.GetComponent<TankBehaviour>();
        m_Tanks.Remove(player);

        base.OnServerDisconnect(conn);
    }


    private void SpawnTank(NetworkConnection conn)
    {
        // Instanstiate player
        Transform startPos = GetTankSpawnPoint();
        TankBehaviour player =
            Instantiate(playerPrefab, startPos.position, startPos.rotation).GetComponent<TankBehaviour>();
        
        // Add player
        NetworkServer.AddPlayerForConnection(conn, player.gameObject);

        // Set player attribute
        player.m_PlayerNumber = numPlayers;
        player.m_PlayerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        player.m_Control = false;
        player.RpcSetSpawnPoint(startPos.position, startPos.rotation);

        // Set camera to point player
        player.RpcSetCameraTarget();

        // Save player to list
        m_Tanks.Add(player);
    }


    private Transform GetTankSpawnPoint()
    {
        // TODO: randomize spawn point

        return m_SpawnPoint;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_MessageText.text = string.Empty;

        while (!OneTankLeft())
        {
            yield return null;
        }

    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        m_RoundWinner = null;

        m_RoundWinner = GetRoundWinner();

        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Count; i++)
        {
            if (m_Tanks[i].gameObject.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankBehaviour GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            if (m_Tanks[i].gameObject.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankBehaviour GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Count; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    [Server]
    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            m_Tanks[i].RpcReset();
        }
    }


    [Server]
    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            m_Tanks[i].m_Control = true;
        }
    }


    [Server]
    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            m_Tanks[i].m_Control = false;
        }
    }
}
