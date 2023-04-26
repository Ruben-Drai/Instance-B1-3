using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DepthInfluenceManager : MonoBehaviour
{
    public List<string> Memory;
    public static GameObject instance;
    public float TimeElapsed=0f;
    // Start is called before the first frame update
    private void Start()
    {
        


    }
    void Awake()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

    }
    private void Update()
    {
        TimeElapsed += Time.deltaTime;
    }
    public static bool? CheckDependencies(Dependencies dependencies)
    {
        if (dependencies.type == DependenceType.Get)
        {
            int validatedComps = 0;
            foreach (Comparator comp in dependencies.Comparators)
            {
                if (comp.sign == "=")
                {
                    if (comp.value == GetValue(dependencies.variable.ToString()))
                    {
                        validatedComps++;
                    }
                }
                else if (comp.sign == "<")
                {
                    if (int.Parse(comp.value) > int.Parse(GetValue(dependencies.variable.ToString())))
                    {
                        validatedComps++;
                    }
                }
                else if (comp.sign == ">")
                {
                    if (int.Parse(comp.value) < int.Parse(GetValue(dependencies.variable.ToString())))
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
                int result = int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1])+ int.Parse(dependencies.Comparators[0].value);
                instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1] = result.ToString();
            }
            else if (dependencies.Comparators[0].sign == "-")
            {
                int result = int.Parse(instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1])- int.Parse(dependencies.Comparators[0].value);
                instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(dependencies.variable.ToString()) + 1] = result.ToString();
            }
        }
        return null;
    }
    public static string GetValue(string varName)
    {
        return instance.GetComponent<DepthInfluenceManager>().Memory[instance.GetComponent<DepthInfluenceManager>().Memory.IndexOf(varName) + 1];
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
    Wallet,
    Charger,
    Pen,
    Cap,
    Laptop,
    Wanted,
    BruceReput,
    Lives,
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
