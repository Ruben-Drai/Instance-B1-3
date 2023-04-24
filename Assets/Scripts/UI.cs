using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI : MonoBehaviour
{
    public Button Pausebutton;
    public Sprite pauseSprite;
    public Sprite playSprite;
    public Sprite skipSprite;
    public bool isPlaying = true;
    public static GameObject instance;

    private void Start()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }

    public void PauseButton()
    {
        //TODO: pause timers in QTE and PnC
        //Changes the button's sprite to the play sprite and pauses the video
        if (Video.instance.GetComponent<Video>().IsPlaying() && !Video.isInAction)
        {
            Pausebutton.image.sprite = playSprite;
            Video.Pause(true);
        }
        //Changes the button's sprite to the pause sprite and resumes the video
        else if(!Video.isInAction)
        {
            Pausebutton.image.sprite = pauseSprite;
            Video.Pause(false);
        }
    }

    public void SkipButton()
    {
        if (Video.instance.GetComponent<Video>().IsPlaying() && !Video.isInAction)
        {
            Video.instance.GetComponent<VideoPlayer>().time = Video.instance.GetComponent<Video>().actionList[Video.currentActionIndex].ActionStart - 1;
        }
    }
}
