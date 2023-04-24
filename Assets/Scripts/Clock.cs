using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public static GameObject instance;
    public Transform minuteHand;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null) instance = gameObject;
        else Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        float rotation = -77 +(float.Parse(DepthInfluenceManager.GetValue("Lives")) * 5f*5.85f) - (float.Parse(DepthInfluenceManager.GetValue("Advancement"))*5.85f);
        instance.GetComponent<Clock>().minuteHand.rotation= Quaternion.Euler(0,0,rotation);
    }
}
