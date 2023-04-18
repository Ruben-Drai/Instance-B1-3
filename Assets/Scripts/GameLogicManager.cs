using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    [SerializeField]
    private GameObject defaultVideo;

    private bool isInQTE = false;
    private bool isInPnC = false;

    private Coroutine Action;
    private KeyInputs KeyInputs;
    private List<TouchInputs> TouchInputs;

    private float ActionDuration = 0f;
    private float ActionTime = 0f;
    private double ActionEnd = 0f;

    // Start is called before the first frame update

    public static GameObject instance;
    void Start()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Video.instance?.GetComponent<Video>().CheckAction();
        if (isInQTE && Action == null)
        {
            Action = StartCoroutine("QTE_Routine");
        }
        else if (isInPnC && Action == null)
        {
            Action = StartCoroutine("PnC_Routine");
        }
    }
    public void RequestQTE(float duration, double EndTime, KeyInputs keys, GameObject defaultScene)
    {
        ActionTime = 0f;
        ActionDuration = duration;
        isInQTE = true;
        isInPnC = false;
        KeyInputs = keys;
        ActionEnd = EndTime;
        defaultVideo = defaultScene;

    }

    public void RequestPnC(float duration, List<TouchInputs> Touch, GameObject defaultScene)
    {
        ActionTime = 0f;
        ActionDuration = duration;
        isInQTE = false;
        isInPnC = true;
        TouchInputs = Touch;
        defaultVideo = defaultScene;
    }

    private IEnumerator QTE_Routine()
    {
        //finds correct speed so that when the QTE ends, it ends at the target time in the video
        Video.ChangeSpeed((float)(ActionDuration / (ActionEnd - Video.GetCurrentTime())));
        while (isInQTE)
        {
            ActionTime += Time.deltaTime;
            int heldKeys = 0;
            foreach (KeyCode key in KeyInputs.keys)
            {
                if (Input.GetKey(key)) heldKeys++;
            }
            if (heldKeys >= KeyInputs.keys.Count)
            {
                //Do thing if keys are held then exits coroutine by breaking out of the while loop
                if(KeyInputs.isLeading)
                {
                    Destroy(Video.instance);
                    Video.instance = Instantiate(KeyInputs.prefab);
                }
                break;
            }
            else if (ActionTime >= ActionDuration)
            {
                //Do thing if the QTE was a failure then exits coroutine by breaking out of the while loop
                Destroy(Video.instance);
                Video.instance = Instantiate(defaultVideo);
                break;
            }
        }
        //exits the coroutine
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    for(int i = 0; i < TouchInputs.Count; i++)
                    {
                        if (hit.transform.gameObject == TouchInputs[i].button.gameObject)
                        {
                            //do thing if hitbox is clicked
                            if(TouchInputs[i].isLeading)
                            {
                                Debug.Log("clicked");
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
                Destroy(Video.instance);
                Video.instance = Instantiate(defaultVideo);
                break;
            }
            
        }
        //exits the coroutine
        ext:;
        Video.ChangeSpeed(1);
        Video.currentActionIndex = 0;
        yield return null;
    }
}
