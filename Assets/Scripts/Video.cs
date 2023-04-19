using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public static int currentActionIndex = 0;
    public static bool isInAction = false;
    public static GameObject instance = null;

    public List<Action> actionList;    


    private VideoPlayer player;

    private void Awake()
    {
       
        //Singleton stuff
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
        //Get Video
        player = GetComponent<VideoPlayer>();
    }
    private void Start()
    {
        player.targetCamera = Camera.main;
    }

    void Update()
    {

    }
    public void PlayVideo()
    {
        player.Play();
    }

    public void PauseVideo()
    {
        player.Pause();
    }

    public bool IsPlaying()
    {
        return player.isPlaying;
    }
    public void CheckAction()
    {
        //Checks when the action needs to be launched, Caution: Put actions in chronological order in the list
        if (player!=null && actionList?.Count > currentActionIndex && player.time == actionList[currentActionIndex].ActionStart)
        {
            //checks action type and requests appropriate action to be launched to the Game
            if (actionList[currentActionIndex].type == ActionType.QTE)
            {
                GameLogicManager.instance.GetComponent<GameLogicManager>().RequestQTE(
                    actionList[currentActionIndex].ActionDuration,
                    actionList[currentActionIndex].ActionEnd,
                    actionList[currentActionIndex].ActionStart,
                    actionList[currentActionIndex].KeyInputs,
                    actionList[currentActionIndex].defaultVideo,
                    actionList[currentActionIndex].HasDefault
                    );
            }
            else
            {
                GameLogicManager.instance.GetComponent<GameLogicManager>().RequestPnC(
                    actionList[currentActionIndex].ActionDuration, 
                    actionList[currentActionIndex].ActionStart, 
                    actionList[currentActionIndex].TouchInputs,
                    actionList[currentActionIndex].defaultVideo,
                    actionList[currentActionIndex].HasDefault,
                    actionList[currentActionIndex].setGlobalTimer,
                    actionList[currentActionIndex].isUsingGlogalTime,
                    actionList[currentActionIndex].globalTimeSet
                    );
            }
            //increases index to read next action in list next time
            currentActionIndex++;
        }
    }
    [System.Serializable]
    public struct Action
    {
        public ActionType type;
        public double ActionStart;
        public double ActionEnd;
        public float ActionDuration;

        public bool HasDefault; //trigger a video if the player fails the sequence 
        public GameObject defaultVideo;

        public bool isUsingGlogalTime;
        public bool setGlobalTimer; //if this action changes the global timer
        public float globalTimeSet;

        public List<KeyInputs> KeyInputs;
        public List<TouchInputs> TouchInputs;

    }
    public enum ActionType
    {
        QTE,
        PnC,
    }
    
    public static void ChangeSpeed(float playbackSpeed)
    {
        VideoPlayer player = instance.GetComponent<VideoPlayer>();
        if(player!=null)
            player.playbackSpeed = playbackSpeed;

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
public struct KeyInputs
{
    //TODO: make it so that you can add "dependencies", meaning doing this action will affect the reputation system or collect an object
    //or on the contrary, that the prefab that gets loaded is different depending on current rep or collected items
    public bool isLeading; // bool that trigger a video if the point and click is a sucess
    public GameObject prefab;
    
    public List<KeyCode> keys;

}

[System.Serializable]
public struct TouchInputs
{
    //TODO: make it so that you can add "dependencies", meaning doing this action will affect the reputation system or collect an object
    //or on the contrary, that the prefab that gets loaded is different depending on current rep or collected items
    public bool isLeading;
    public GameObject prefab;

    public PolygonCollider2D button;

}
