using System.Collections;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{

    private bool isInQTE = false;
    private bool isInPnC = false;

    private Action action;
    private Coroutine Action;

    private float ActionTime = 0f;
    private float GlobalTime = 0f;

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
        if (Action == null && action.setTimer && GlobalTime <= 0)
        {
            GlobalTime = action.globalTimeSet;
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
        ActionTime = 0f;
        isInQTE = true;
        isInPnC = false;
        Video.isInAction = true;
    }

    public void RequestPnC(Action _Action)
    {
        action = _Action;
        ActionTime = 0f;
        isInQTE = false;
        isInPnC = true;
        Video.isInAction = true;

    }

    private IEnumerator QTE_Routine()
    {
        //finds correct speed so that when the QTE ends, it ends at the target time in the video
        Video.ChangeSpeed((float)(1/(action.ActionDuration / (action.ActionEnd - action.ActionStart))));
        while (isInQTE)
        {
            ActionTime += Time.deltaTime;
            for (int i = 0; i < action.KeyInputs.Count; i++)
            {
                int heldKeys = 0;
                foreach (KeyCode key in action.KeyInputs[i].keys)
                { 
                    if (Input.GetKey(key)) heldKeys++;  
                }
                if (heldKeys >= action.KeyInputs[i].keys.Count)
                {
                    bool? isAlt = false;
                    int altIndex = 0;
                    //Do thing if keys are held then exits coroutine by breaking out of the while loop
                    for(int u =0;u< action.KeyInputs[i].dependencies.Count; u++)
                    {
                        if (DepthInfluenceManager.checkDependencies(action.KeyInputs[i].dependencies[u]) != null)
                        {
                            isAlt = DepthInfluenceManager.checkDependencies(action.KeyInputs[i].dependencies[u]);
                            altIndex = u;
                        }

                    }
                    if (action.KeyInputs[i].isLeading)
                    {
                        Destroy(Video.instance);
                        Video.instance = null;
                        Instantiate(isAlt == false ? action.KeyInputs[i].prefab: action.KeyInputs[i].dependencies[altIndex].altPrefab);
                    }
                    goto ext;
                }
                else if (ActionTime >= action.ActionDuration)
                {
                    //Do thing if the QTE was a failure then exits coroutine by breaking out of the while loop
                    if (action.HasDefault)
                    {
                        Destroy(Video.instance);
                        Video.instance = null;
                        Instantiate(action.defaultVideo);
                    }
                    goto ext;
                }
            }
            yield return null;
        }
        //exits the coroutine
        ext:;
        Action = null;
        isInQTE = false;
        Video.ChangeSpeed(1);
        Video.currentActionIndex = 0;
        Video.isInAction = false;
        yield return null;
    }

    private IEnumerator PnC_Routine()
    {
        Video.ChangeSpeed(0);
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
                                if (DepthInfluenceManager.checkDependencies(action.TouchInputs[i].dependencies[u]) != null)
                                {
                                    isAlt = DepthInfluenceManager.checkDependencies(action.TouchInputs[i].dependencies[u]);
                                    altIndex = u;
                                }

                            }
                            //do thing if hitbox is clicked
                            if (action.TouchInputs[i].isLeading)
                            {
                                Destroy(Video.instance);
                                Video.instance = null;
                                Instantiate(isAlt == false ? action.TouchInputs[i].prefab : action.TouchInputs[i].dependencies[altIndex].altPrefab);
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
                    Video.instance = null;                   
                    Instantiate(action.defaultVideo);
                }
                break;
            }
            yield return null;
            
        }
    //exits the coroutine
        ext:;
        isInPnC = false;
        Action = null;
        Video.ChangeSpeed(1);
        Video.currentActionIndex = 0;
        Video.isInAction = false;
        yield return null;
    }
}