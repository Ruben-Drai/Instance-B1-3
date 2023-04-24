using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    Image timerBar;
    public float maxTime = 20f;
    float timeLeft;

    private void Start()
    {
        timerBar = GetComponent<Image>();
    }

    private void Update()
    {
        timeLeft = GameLogicManager.instance.GetComponent<GameLogicManager>().GlobalTime;
        if (timeLeft > 0)
        {
            GetComponent<Image>().enabled = true;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else
        {
            GetComponent<Image>().enabled = false;
        }
    }
}
