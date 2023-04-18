using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    [SerializeField]
    private GameObject defaultVideo;

    private bool isInQTE = false;
    private bool isInPnC = false;
    private bool HasDefault = false;

    private Coroutine Action;
    private List<KeyInputs> KeyInputs;
    private List<TouchInputs> TouchInputs;

    private float ActionDuration = 0f;
    private float ActionTime = 0f;
    private double ActionEnd = 0f;
    private double ActionStart = 0f;

    public static GameObject instance;
    void Awake()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Video.instance?.GetComponent<Video>()?.CheckAction();
        if (isInQTE && Action == null)
        {
            Action = StartCoroutine("QTE_Routine");
        }
        else if (isInPnC && Action == null)
        {
            Action = StartCoroutine("PnC_Routine");
        }
    }
    public void RequestQTE(float duration, double EndTime, double StartTime,List<KeyInputs> keys, GameObject defaultScene, bool hasDefault)
    {
        ActionTime = 0f;
        ActionDuration = duration;
        isInQTE = true;
        isInPnC = false;
        KeyInputs = keys;
        ActionEnd = EndTime;
        ActionStart = StartTime;
        defaultVideo = defaultScene;
        HasDefault = hasDefault;

    }

    public void RequestPnC(float duration, double StartTime, List<TouchInputs> Touch, GameObject defaultScene, bool hasDefault)
    {
        ActionTime = 0f;
        ActionDuration = duration;
        isInQTE = false;
        isInPnC = true;
        TouchInputs = Touch;
        ActionStart = StartTime;
        defaultVideo = defaultScene;
        HasDefault = hasDefault;

    }

    private IEnumerator QTE_Routine()
    {
        //finds correct speed so that when the QTE ends, it ends at the target time in the video
        Video.ChangeSpeed((float)(1/(ActionDuration / (ActionEnd - ActionStart))));
        while (isInQTE)
        {
            ActionTime += Time.deltaTime;
            for (int i = 0; i < KeyInputs.Count; i++)
            {
                int heldKeys = 0;
                foreach (KeyCode key in KeyInputs[i].keys)
                { 
                    if (Input.GetKey(key)) heldKeys++;  
                }
                if (heldKeys >= KeyInputs[i].keys.Count)
                {
                    //Do thing if keys are held then exits coroutine by breaking out of the while loop
                    if (KeyInputs[i].isLeading)
                    {
                        Destroy(Video.instance);
                        Video.instance = Instantiate(KeyInputs[i].prefab);
                    }
                    goto ext;
                }
                else if (ActionTime >= ActionDuration)
                {
                    //Do thing if the QTE was a failure then exits coroutine by breaking out of the while loop
                    if (HasDefault)
                    {
                        Destroy(Video.instance);
                        Video.instance = Instantiate(defaultVideo);
                    }
                    goto ext;
                }
            }
            yield return null;
        }
        //exits the coroutine
        ext:;
        Video.ChangeSpeed(1);
        Video.currentActionIndex = 0;
        yield return null;
    }

    private IEnumerator PnC_Routine()
    {
        Video.ChangeSpeed(0);
        while (isInPnC)
        {
            ActionTime += Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if (hit.collider != null)
                {
                    for (int i = 0; i < TouchInputs.Count; i++)
                    {
                        if (hit.transform.gameObject == TouchInputs[i].button.gameObject)
                        {
                            //do thing if hitbox is clicked
                            if (TouchInputs[i].isLeading)
                            {
                                Destroy(Video.instance);
                                Video.instance = Instantiate(TouchInputs[i].prefab);
                            }

                            goto ext;
                        }
                    }
                }
            }
            else if (ActionTime >= ActionDuration)
            {
                //Do thing if the PnC was a failure then exits coroutine by breaking out of the while loop
                if (HasDefault)
                {
                    Destroy(Video.instance);
                    Video.instance = Instantiate(defaultVideo);
                }
                break;
            }
            yield return null;
            
        }
        //exits the coroutine
        ext:;
        Video.ChangeSpeed(1);
        Video.currentActionIndex = 0;
        yield return null;
    }
}
