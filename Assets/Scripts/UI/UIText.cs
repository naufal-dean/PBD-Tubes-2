using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : NetworkBehaviour
{
    #region Client

    [ClientRpc]
    public void RpcSetMessageText(string message)
    {
        GetComponent<Text>().text = message;
    }

    #endregion
}
