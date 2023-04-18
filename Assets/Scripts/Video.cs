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
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CheckAction()
    {
        if (actionList?.Count > currentActionIndex && Mathf.Abs((float)(player.time - actionList[currentActionIndex].ActionStart)) < 1f)
        {
            if (actionList[currentActionIndex].type == ActionType.QTE)
            {
                GameLogicManager.instance.GetComponent<GameLogicManager>().RequestQTE(
                    actionList[currentActionIndex].ActionDuration,
                    actionList[currentActionIndex].ActionEnd,
                    actionList[currentActionIndex].KeyInputs,
                    actionList[currentActionIndex].defaultVideo
                    );
            }
            else
            {
                GameLogicManager.instance.GetComponent<GameLogicManager>().RequestPnC(
                    actionList[currentActionIndex].ActionDuration, 
                    actionList[currentActionIndex].TouchInputs,
                    actionList[currentActionIndex].defaultVideo
                    );
            }
            currentActionIndex++;
        }
    }
    [System.Serializable]
    public struct Action
    {
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