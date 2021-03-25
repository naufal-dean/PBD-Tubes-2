using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIName : MonoBehaviour
{
    void Start()
    {
        string name = PlayerPrefs.GetString("name");
        if (name != "")
        {
            GetComponent<TextMesh>().text = name;
        }
        else
        {
            GetComponent<TextMesh>().text = "player";
        }
    }

    private void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform.position);
        gameObject.transform.Rotate(0, 180, 0);
    }

}
