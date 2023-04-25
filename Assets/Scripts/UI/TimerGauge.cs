using UnityEngine;
using UnityEngine.UI;

public class TimerGauge : MonoBehaviour
{
    private Image timerBar;
    private float maxTime;
    private float timeLeft;

    private void Start()
    {
        timerBar = GetComponent<Image>();
    }

    private void Update()
    {
        timeLeft = GameLogicManager.instance.GetComponent<GameLogicManager>().GlobalTime;
        maxTime= GameLogicManager.instance.GetComponent<GameLogicManager>().InitialGlobalTime;
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
