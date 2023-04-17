using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    [SerializeField]
    private VideoPlayerManager videoPlayer;

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
        if (isInQTE && Action == null)
        {
            Action = StartCoroutine("QTE_Routine");
        }
        else if (isInPnC && Action == null)
        {
            Action = StartCoroutine("PnC_Routine");
        }
    }
    public void RequestQTE(float duration, double EndTime, KeyInputs keys)
    {
        ActionTime = 0f;
        ActionDuration = duration;
        isInQTE = true;
        isInPnC = false;
        KeyInputs = keys;
        ActionEnd = EndTime;
    }

    public void RequestPnC(float duration, List<TouchInputs> Touch)
    {
        ActionTime = 0f;
        ActionDuration = duration;
        isInQTE = false;
        isInPnC = true;
        TouchInputs = Touch;
    }

    private IEnumerator QTE_Routine()
    {
        //finds correct speed so that when the QTE ends, it ends at the target time in the video
        videoPlayer.ChangeSpeed((float)(ActionDuration / (ActionEnd - videoPlayer.GetCurrentTime())));
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
                break;
            }
            else if (ActionTime >= ActionDuration)
            {
                //Do thing if the QTE was a failure then exits coroutine by breaking out of the while loop
                break;
            }
        }
        //exits the coroutine
        videoPlayer.ChangeSpeed(1);
        yield return null;
    }

    private IEnumerator PnC_Routine()
    {
        //finds correct speed so that when the QTE ends, it ends at the target time in the video
        videoPlayer.ChangeSpeed(0);
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
                            Debug.Log("clicked");
                            goto ext;
                        }
                    }
                    
                }
            }
            else if (ActionTime >= ActionDuration)
            {
                //Do thing if the PnC was a failure then exits coroutine by breaking out of the while loop
                break;
            }
            
        }
        //exits the coroutine
        ext:;
        videoPlayer.ChangeSpeed(1);
        yield return null;
    }
}
