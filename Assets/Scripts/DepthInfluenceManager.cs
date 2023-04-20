using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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
            foreach(Comparator comp in dependencies.Comparators)
            {
                if(comp.sign== "=")
                {
                    if (comp.value == instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1])
                    {
                        validatedComps++;
                    }
                }
                else if (comp.sign == "<")
                {
                    if (int.Parse(comp.value) > int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1]))
                    {
                        validatedComps++;
                    }
                }
                else if(comp.sign == ">")
                {
                    if (int.Parse(comp.value) < int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1]))
                    {
                        validatedComps++;
                    }
                }  
            }  
            return validatedComps == dependencies.Comparators.Count;
        }
        else if (dependencies.type == DependenceType.Set)
        {
            if (dependencies.Comparators[0].sign == "" || dependencies.Comparators[0].sign == "=")
            {
                instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1] = dependencies.Comparators[0].value;
            }
            else if (dependencies.Comparators[0].sign == "+")
            {
                int result = int.Parse(dependencies.Comparators[0].value) + int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1]);
                instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1] = result.ToString();
            }
            else if (dependencies.Comparators[0].sign == "-")
            {
                int result = int.Parse(dependencies.Comparators[0].value) - int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1]);
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
    public List<Comparator> Comparators;
    public GameObject altPrefab;

}
[System.Serializable]
public enum InfluenceVariable
{
    Papers,
}
[System.Serializable]
public struct Comparator
{
    public string sign;
    public string value;
}

[System.Serializable]
public enum DependenceType
{
    Get,
    Set,
}
