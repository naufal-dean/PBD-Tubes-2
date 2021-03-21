using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit game!");
        // TO DO: Log off from connection here
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
