using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static string username = "N/A";

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SetUsername(string name)
    {
        username = name;
    }

    public void QuitGame(int value)
    {
        if (value == 1)
        {
            Application.Quit();
        }
    }
}
