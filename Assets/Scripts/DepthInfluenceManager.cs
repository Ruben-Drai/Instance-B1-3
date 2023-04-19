using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DepthInfluenceManager : MonoBehaviour
{
    public static GameObject instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }
    public static bool IsAlternative()
    {
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}