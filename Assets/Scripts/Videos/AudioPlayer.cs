using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AudioPlayer : MonoBehaviour
{
    public static GameObject instance;
    public bool IsPlaying = false;
    private AudioSource player;
    public Image pauseButton;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<AudioSource>();
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying && !player.isPlaying) player.Play();
        else if(!IsPlaying && player.isPlaying) player.Stop();
        if(pauseButton.sprite.name != "pausebutton") player.Pause();
        else if(!player.isPlaying) player.UnPause();
    }
}
