using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class DepthInfluenceManager : MonoBehaviour
{

    public Dictionary<string, string> Memory;
    public static GameObject instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
        Memory = new Dictionary<string, string>()
        {
            {"Papers","1"}
        };
    }
    public static bool? checkDependencies(dependencies dependencies)
    {
        if (dependencies.type == dependenceType.Get)
        {
            string varName = Enum.GetName(typeof(influenceVariable), dependencies.variable);
            return instance.GetComponent<DepthInfluenceManager>().Memory.TryGetValue(varName, out var value) && value == dependencies.value;
        }
        else if (dependencies.type == dependenceType.Set)
        {
            string varName = Enum.GetName(typeof(influenceVariable), dependencies.variable);
            int index = instance.GetComponent<DepthInfluenceManager>().Memory.Keys.ToList().IndexOf(varName);
            if(dependencies.value.Contains("-") || dependencies.value.Contains("+"))
            {
                instance.GetComponent<DepthInfluenceManager>().Memory;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public struct dependencies
{
    public dependenceType type;
    public influenceVariable variable;
    public string value;
}

public enum influenceVariable
{
    Papers,
}

public enum dependenceType
{
    Get,
    Set,
}