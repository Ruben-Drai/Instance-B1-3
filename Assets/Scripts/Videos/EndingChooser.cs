using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingChooser : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject goodEnding;
    public GameObject badEnding;
    public GameObject goodEndingTwist;
    void Start()
    {
        if (int.Parse(DepthInfluenceManager.GetValue("Lives")) <= 0) {
            Destroy(gameObject);
            Video.instance=null;
            Instantiate(badEnding);    
        }
        else if (DepthInfluenceManager.GetValue("Laptop") == "0")
        {
            Destroy(gameObject);
            Video.instance = null;
            Instantiate(goodEndingTwist);
        }
        else
        {
            Destroy(gameObject);
            Video.instance = null;
            Instantiate(goodEnding);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
