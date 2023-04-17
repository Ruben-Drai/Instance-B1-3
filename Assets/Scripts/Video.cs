using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Action> actionList;

    public static GameObject instance;

    private VideoPlayer player;
    private int currentActionIndex = 0;
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
        if (Mathf.Abs((float)(player.time - actionList[currentActionIndex].ActionStart)) < 1f)
        {
            if (actionList[currentActionIndex].type == ActionType.QTE)
            {
                GameLogicManager.instance.GetComponent<GameLogicManager>().RequestQTE(
                    actionList[currentActionIndex].ActionDuration,
                    actionList[currentActionIndex].ActionEnd,
                    actionList[currentActionIndex].KeyInputs
                    );
            }
            else
            {
                GameLogicManager.instance.GetComponent<GameLogicManager>().RequestPnC(actionList[currentActionIndex].ActionDuration, actionList[currentActionIndex].TouchInputs);
            }
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
    public List<KeyCode> keys;
    public bool isLeading;
    public GameObject prefab;
}
[System.Serializable]
public struct TouchInputs
{
    public PolygonCollider2D button;
    public bool isLeading;
    public GameObject prefab;
}