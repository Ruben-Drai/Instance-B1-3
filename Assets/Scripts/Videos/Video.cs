using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public bool IsEnding;
    public bool ShowClock;
    public bool IsCheckPoint;
    public AudioSource Music;
    public List<Action> actionList;
    public static int currentActionIndex = 0;
    public static bool isInAction = false;

    private VideoPlayer player;

    public static GameObject instance = null;
    private void Awake()
    {
        //Singleton stuff
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
        //Get Video
        player = GetComponent<VideoPlayer>();
        transform.position = Vector3.zero;

    }
    private void Start()
    {
        player.targetCamera = Camera.main;
        if (IsCheckPoint) GameLogicManager.checkpoint = name;
        if (ShowClock) Clock.instance.SetActive(true);
        else Clock.instance.SetActive(false);
    }

    public bool IsPlaying()
    {
        return player.isPlaying;
    }
    public void CheckAction()
    {
        if(IsEnding && player.time >= player.length-1) SceneManager.LoadScene("GameOver");

        //Checks when the action needs to be launched, Caution: Put actions in chronological order in the list
        if (player != null && actionList?.Count > currentActionIndex && player.time >= actionList[currentActionIndex].ActionStart)
        {
            //checks action type and requests appropriate action to be launched to the Game
            if (actionList[currentActionIndex].type == ActionType.QTE)
            {
                GameLogicManager.instance.GetComponent<GameLogicManager>().RequestQTE(
                        actionList[currentActionIndex]
                    );
            }
            else
            {
                GameLogicManager.instance.GetComponent<GameLogicManager>().RequestPnC(
                       actionList[currentActionIndex]
                    );
            }
            //increases index to read next action in list next time


            if (currentActionIndex<actionList.Count)
                currentActionIndex++;

            if (currentActionIndex >= actionList.Count && player.isLooping)
                currentActionIndex = 0;
        }
    }
    public static void ChangeSpeed(float playbackSpeed)
    {
        VideoPlayer player = instance.GetComponent<VideoPlayer>();
        if (player != null)
            player.playbackSpeed = playbackSpeed;

    }
    public static void Pause(bool Pause)
    {
        VideoPlayer player = instance.GetComponent<VideoPlayer>();
        if (player != null)
            if (Pause)
                player.Pause();
            else
                player.Play();

        if (instance.GetComponent<Video>().Music != null)
        {
            if(Pause)
                instance.GetComponent<Video>().Music.Pause();
            else
                instance.GetComponent<Video>().Music.UnPause();
        }

    }
    public static double? GetCurrentTime()
    {
        VideoPlayer player = instance.GetComponent<VideoPlayer>();
        if (player != null)
            return player.time;

        return null;
    }
}
[System.Serializable]
public struct Action
{
    public ActionType type;
    public bool Pause;
    public bool ShowInventory;
    public bool isFail;
    public bool HasDefault; //trigger a video if the player fails the sequence 
    public bool setTimer;
    public bool isUsingGlobalTime;
    public GameObject defaultVideo;
    public double ActionStart;
    public double ActionEnd;
    public float ActionDuration;
    public float globalTimeSet;
    public List<KeyInputs> KeyInputs;
    public List<TouchInputs> TouchInputs;
}
public enum ActionType
{
    QTE,
    PnC,
}

[System.Serializable]
public struct KeyInputs
{
    //TODO: make it so that you can add "dependencies", meaning doing this action will affect the reputation system or collect an object
    //or on the contrary, that the prefab that gets loaded is different depending on current rep or collected items
    public List<KeyCode> keys;
    public List<GameObject> buttonSprites;
    public bool isLeading; // bool that trigger a video if the point and click is a sucess
    public GameObject prefab;
    public List<Dependencies> dependencies;

}

[System.Serializable]
public struct TouchInputs
{
    //TODO: make it so that you can add "dependencies", meaning doing this action will affect the reputation system or collect an object
    //or on the contrary, that the prefab that gets loaded is different depending on current rep or collected items
    public PolygonCollider2D button;
    public bool isLeading;
    public GameObject prefab;
    public List<Dependencies> dependencies;
}
