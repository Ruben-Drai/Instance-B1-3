using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class GameLogicManager : MonoBehaviour
{

    private bool isInQTE = false;
    private bool isInPnC = false;

    private Action action;
    private Coroutine Action;

    private float ActionTime = 0f;
    public float GlobalTime = 0f;
    public float InitialGlobalTime = 0f;
    public GameObject failScreen;

    public static GameObject instance;
    public static string checkpoint;
    void Awake()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Video.instance.GetComponent<Video>().CheckAction();
        if (Action == null && action.setTimer && GlobalTime <= 0)
        {
            GlobalTime = action.globalTimeSet;
            InitialGlobalTime = action.globalTimeSet;
        }
        if (isInQTE && Action == null)
        {
            Action = StartCoroutine("QTE_Routine");
        }
        else if (isInPnC && Action == null)
        {
            Action = StartCoroutine("PnC_Routine"); 

        }
    }
    public void RequestQTE(Action _Action)
    {
        action = _Action;
        if (!action.isUsingGlobalTime)
            GlobalTime = 0f;

        ActionTime = 0f;
        isInQTE = true;
        isInPnC = false;
        Video.isInAction = true;
        Action = null;
    }

    public void RequestPnC(Action _Action)
    {
        action = _Action;
        ActionTime = 0f;
        if (!action.isUsingGlobalTime)
            GlobalTime = 0f;

        isInQTE = false;
        isInPnC = true;
        Video.isInAction = true;
        Action= null;
    }

    private IEnumerator QTE_Routine()
    {
        //finds correct speed so that when the QTE ends, it ends at the target time in the video
        Video.ChangeSpeed((float)(1 / (action.ActionDuration / (action.ActionEnd - action.ActionStart))));
        foreach (KeyInputs key in action.KeyInputs)
        {
            foreach (GameObject button in key.buttonSprites)
            {
                button.SetActive(true);
            }
        }
        while (isInQTE)
        {
            int i = 0;
            ActionTime += Time.deltaTime;
            do
            {
                int heldKeys = 0;
                if (action.KeyInputs.Count != 0)
                {
                    foreach (KeyCode key in action.KeyInputs[i].keys)
                    {
                        if (Input.GetKey(key)) heldKeys++;
                    }
                }
               
                if ( action.KeyInputs.Count != 0 && i < action.KeyInputs[i].keys.Count && heldKeys >= action.KeyInputs[i].keys.Count && action.KeyInputs[i].keys.Count != 0)
                {
                    
                    bool? isAlt = false;
                    int altIndex = 0;
                    //Do thing if keys are held then exits coroutine by breaking out of the while loop
                    for (int u = 0; u < action.KeyInputs[i].dependencies.Count; u++)
                    {
                        if (DepthInfluenceManager.CheckDependencies(action.KeyInputs[i].dependencies[u]) != null)
                        {
                            isAlt = DepthInfluenceManager.CheckDependencies(action.KeyInputs[i].dependencies[u]);
                            altIndex = u;
                        }

                    }
                    if (action.KeyInputs[i].isLeading)
                    {
                        Destroy(Video.instance);
                        Video.currentActionIndex = 0;
                        Video.instance = null;
                        GameObject temp = Instantiate(isAlt == false ? action.KeyInputs[i].prefab : action.KeyInputs[i].dependencies[altIndex].altPrefab);
                        temp.name = isAlt == false ? action.KeyInputs[i].prefab.name : action.KeyInputs[i].dependencies[altIndex].altPrefab.name;

                    }
                    goto ext;
                }
                else if (ActionTime >= action.ActionDuration)
                {
                    //Do thing if the QTE was a failure then exits coroutine by breaking out of the while loop
                    if (action.HasDefault)
                    {
                        Destroy(Video.instance);
                        Video.currentActionIndex = 0;
                        Video.instance = null;
                        GameObject temp = Instantiate(action.defaultVideo);
                        temp.name= action.defaultVideo.name;
                    }
                    else if (action.isFail)
                    {
                        Instantiate(failScreen, UI.instance.transform);
                        Video.Pause(true);
                        goto extFail;
                    }
                    goto ext;
                }
                i++;
            } while (i < action.KeyInputs.Count);
            yield return null;
        }
    //exits the coroutine
    ext:;
        Video.Pause(false);
    extFail:;
        Video.ChangeSpeed(1);
        foreach (KeyInputs key in action.KeyInputs)
        {
            foreach (GameObject button in key.buttonSprites)
            {
                button.SetActive(false);
            }
        }
        isInQTE = false;
        action = new();
        Video.isInAction = false;
        yield return null;
    }

    private IEnumerator PnC_Routine()
    {
        Inventory.isInRoomPnC = action.ShowInventory;
        if (!Video.instance.GetComponent<VideoPlayer>().isLooping)
            Video.Pause(true);

        foreach (TouchInputs touchIn in action.TouchInputs)
        {
            touchIn.button.gameObject.SetActive(true);
        }

        while (isInPnC)
        {
            //decreases only during the time the image is stopped, as such global timer doesn't decrease when the video is playing
            if (action.isUsingGlobalTime)
            {
                GlobalTime -= Time.deltaTime;
            }
            else
            {
                ActionTime += Time.deltaTime;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if (hit.collider != null)
                {
                    for (int i = 0; i < action.TouchInputs.Count; i++)
                    {
                        if (hit.transform.gameObject == action.TouchInputs[i].button.gameObject)
                        {
                            bool? isAlt = false;
                            int altIndex = 0;
                            //Do thing if keys are held then exits coroutine by breaking out of the while loop
                            for (int u = 0; u < action.TouchInputs[i].dependencies.Count; u++)
                            {
                                if (DepthInfluenceManager.CheckDependencies(action.TouchInputs[i].dependencies[u]) != null)
                                {
                                    isAlt = DepthInfluenceManager.CheckDependencies(action.TouchInputs[i].dependencies[u]);
                                    altIndex = u;
                                }

                            }
                            //do thing if hitbox is clicked
                            if (action.TouchInputs[i].isLeading)
                            {
                                Destroy(Video.instance);
                                Video.currentActionIndex = 0;
                                Video.instance = null;
                                GameObject temp = Instantiate(isAlt == false ? action.TouchInputs[i].prefab : action.TouchInputs[i].dependencies[altIndex].altPrefab);
                                temp.name = isAlt == false ? action.TouchInputs[i].prefab.name : action.TouchInputs[i].dependencies[altIndex].altPrefab.name;
                            }

                            goto ext;
                        }
                    }
                }
            }
            else if ((action.isUsingGlobalTime && GlobalTime <= 0f) || (ActionTime >= action.ActionDuration && !action.isUsingGlobalTime)) //ajouter ici la condition de fin de timer 
            {
                //Do thing if the PnC was a failure then exits coroutine by breaking out of the while loop
                if (action.HasDefault)
                {
                    Destroy(Video.instance);
                    Video.currentActionIndex = 0;
                    Video.instance = null;
                    Instantiate(action.defaultVideo);
                }
                else if (action.isFail)
                {
                    Instantiate(failScreen,UI.instance.transform);
                    Video.Pause(true);
                    goto extFail;
                }
                goto ext;
            }
            yield return null;

        }
    //exits the coroutine
    ext:;
        Video.Pause(false);
    extFail:;
        foreach (TouchInputs touchIn in action.TouchInputs)
        {
            touchIn.button.gameObject.SetActive(false);
        }
        isInPnC = false;
        action = new();
        Video.isInAction = false;
        yield return null;
        Inventory.isInRoomPnC = false;
    }
}