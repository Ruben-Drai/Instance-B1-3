using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI : MonoBehaviour
{
    public Button Pausebutton;
    public Sprite pauseSprite;
    public Sprite playSprite;
    public Sprite skipSprite;
    public bool isPlaying = true;

    public void Start()
    {
    }

    public void PauseButton()
    {
        //TODO: pause timers in QTE and PnC
        //Changes the button's sprite to the play sprite and pauses the video
        if (Video.instance.GetComponent<Video>().IsPlaying())
        {
            Pausebutton.image.sprite = playSprite;
            Video.instance.GetComponent<Video>().PauseVideo();
        }
        //Changes the button's sprite to the pause sprite and resumes the video
        else
        {
            Pausebutton.image.sprite = pauseSprite;
            Video.instance.GetComponent<Video>().PlayVideo();
        }
    }

    public void SkipButton()
    {
        if (Video.instance.GetComponent<Video>().IsPlaying() && !Video.isInAction)
        {
            Video.instance.GetComponent<VideoPlayer>().time = Video.instance.GetComponent<Video>().actionList[Video.currentActionIndex].ActionStart-1;
        }
    }
}
