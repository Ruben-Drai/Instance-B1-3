using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Fail : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TryAgain()
    {
        Destroy(Video.instance);
        Video.instance = null;
        Video.currentActionIndex = 0;
        GameObject temp = Instantiate(GameLogicManager.checkpoint);
        temp.GetComponent<VideoPlayer>().enabled=true;
        temp.GetComponent<Video>().enabled=true;
        Destroy(gameObject);
    }
}


