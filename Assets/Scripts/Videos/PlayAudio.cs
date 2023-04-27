using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public bool play;
    // Start is called before the first frame update
    void Start()
    {
        AudioPlayer.instance.GetComponent<AudioPlayer>().IsPlaying = play;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
