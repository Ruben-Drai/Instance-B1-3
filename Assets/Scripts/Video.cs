using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Action> actionList;

    public static GameObject instance;

    private VideoPlayer player;
    public static int currentActionIndex = 0;
    void Start()
    {
        player = GetComponent<VideoPlayer>();
    }
    private void Awake()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CheckAction()
    {
        if (actionList?.Count > currentActionIndex && player.time == actionList[currentActionIndex].ActionStart)
        {
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
                    actionList[currentActionIndex].HasDefault
                    );
            }
            currentActionIndex++;
        }
    }
    [System.Serializable]
    public struct Action
    {
        public bool HasDefault;
        public GameObject defaultVideo;
        public ActionType type;
        public double ActionStart;
        public double ActionEnd;
        public float ActionDuration;
        public KeyInputs KeyInputs;
        public List<TouchInputs> TouchInputs;
    }
    
    
    public enum ActionType
    {
        QTE,
        PnC,
    }
    
    public static void ChangeSpeed(float playackSpeed)
    {
        instance.GetComponent<VideoPlayer>().playbackSpeed = playackSpeed;
    }
    public static double GetCurrentTime()
    {
        return instance.GetComponent<VideoPlayer>().time;
    }
}
[System.Serializable]
public struct KeyInputs
{
    //TODO: make it so that you can add "dependencies", meaning doing this action will affect the reputation system or collect an object
    //or on the contrary, that the prefab that gets loaded is different depending on current rep or collected items
    public List<KeyCode> keys;
    public bool isLeading;
    public GameObject prefab;
}
[System.Serializable]
public struct TouchInputs
{
    //TODO: make it so that you can add "dependencies", meaning doing this action will affect the reputation system or collect an object
    //or on the contrary, that the prefab that gets loaded is different depending on current rep or collected items
    public PolygonCollider2D button;
    public bool isLeading;
    public GameObject prefab;
}