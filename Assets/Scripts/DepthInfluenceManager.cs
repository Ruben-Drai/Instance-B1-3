using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class DepthInfluenceManager : MonoBehaviour
{

    public List<string> Memory;
    public static GameObject instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
        Memory = new List<string>()
        {
            "Papers","0",

        };
    }
    public static bool? checkDependencies(Dependencies dependencies)
    {
        if (dependencies.type == DependenceType.Get)
        {
            int validatedComps = 0;
            foreach(string comp in dependencies.Operators)
            {
                if(comp== "=")
                {
                    if (dependencies.value == instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1])
                    {
                        validatedComps++;
                    }
                }
                else if (comp== "<")
                {
                    if (int.Parse(dependencies.value) > int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1]))
                    {
                        validatedComps++;
                    }
                }
                else if(comp== ">")
                {
                    if (int.Parse(dependencies.value) < int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1]))
                    {
                        validatedComps++;
                    }
                }  
            }  
            return dependencies.Operators.Count==0 ?false: validatedComps == dependencies.Operators.Count;
        }
        else if (dependencies.type == DependenceType.Set)
        {
            if (dependencies.Operators.Count == 0 || dependencies.Operators?[0] == "=")
            {
                instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1] = dependencies.value;
            }
            else if (dependencies.Operators?[0] == "+")
            {
                int result = int.Parse(dependencies.value) + int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1]);
                instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1] = result.ToString();
            }
            else if (dependencies.Operators?[0] == "-")
            {
                int result = int.Parse(dependencies.value) - int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1]);
                instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1] = result.ToString();
            }
            
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
[System.Serializable]
public struct Dependencies
{
    public DependenceType type;
    public InfluenceVariable variable;
    public List<string> Operators;
    public string value;
    public GameObject altPrefab;

}
[System.Serializable]
public enum InfluenceVariable
{
    Papers,
}
[System.Serializable]
public enum DependenceType
{
    Get,
    Set,
}