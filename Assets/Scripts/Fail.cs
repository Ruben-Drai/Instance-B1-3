using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Fail : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Video.isInAction = true;
    }

    public void TryAgain()
    {
        Destroy(Video.instance);
        Video.instance = null;
        Video.currentActionIndex = 0;
        string folder = GameLogicManager.checkpoint.Split("_")[0];
        GameObject temp = Instantiate(Resources.Load<GameObject>("Prefabs/" + folder + "/" + GameLogicManager.checkpoint));
        temp.GetComponent<VideoPlayer>().enabled = true;
        temp.GetComponent<Video>().enabled = true;
        temp.name = GameLogicManager.checkpoint;
        Video.isInAction = false;
        Destroy(gameObject);
    }
}