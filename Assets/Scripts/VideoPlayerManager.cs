using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeSpeed(float playackSpeed)
    {
        Video.instance.GetComponent<VideoPlayer>().playbackSpeed = playackSpeed;
    }
    public double GetCurrentTime()
    {
        return Video.instance.GetComponent<VideoPlayer>().time;
    }
}
