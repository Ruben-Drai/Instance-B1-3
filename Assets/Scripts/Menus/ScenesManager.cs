using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesManager : MonoBehaviour
{
    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
        if (_sceneName != "GameOver")
        {
            DepthInfluenceManager.instance.GetComponent<DepthInfluenceManager>().Memory = new List<string>()
            {
                "Wallet","0",
                "Charger","0",
                "Pen","0",
                "Laptop","0",
                "Cap","0",
                "Wanted","0",
                "BruceReput","3",
                "Lives","3",
            };
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

}

