using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class MenuEnd : MonoBehaviour
{
    // Start is called before the first frame update
    private float TimeElapsed;
    private int CollectedItems;
    private string BruceReput;
    private string Wanted;
    private string Lives;
    public TextMeshProUGUI collectedItemsText;
    public TextMeshProUGUI BruceReputText;
    public TextMeshProUGUI WantedText;
    public TextMeshProUGUI LivesText;
    void Start()
    {
        

        

    }
    private void Awake()
    {
        TimeElapsed = DepthInfluenceManager.instance.GetComponent<DepthInfluenceManager>().TimeElapsed;
        if (DepthInfluenceManager.GetValue("Wallet") == "1") CollectedItems++;
        if (DepthInfluenceManager.GetValue("Charger") == "1") CollectedItems++;
        if (DepthInfluenceManager.GetValue("Pen") == "1") CollectedItems++;
        if (DepthInfluenceManager.GetValue("Laptop") == "1") CollectedItems++;
        if (DepthInfluenceManager.GetValue("Cap") == "1") CollectedItems++;


        if (DepthInfluenceManager.GetValue("BruceReput") == "3") BruceReput = "neutral";
        else if (int.Parse(DepthInfluenceManager.GetValue("BruceReput")) > 3) BruceReput = "good";
        else if (int.Parse(DepthInfluenceManager.GetValue("BruceReput")) < 3) BruceReput = "bad";

        if (DepthInfluenceManager.GetValue("Wanted") == "1") Wanted = "a criminal";
        else Wanted = "innocent";

        if (int.Parse(DepthInfluenceManager.GetValue("Lives")) > 0) Lives = "You arrived on time";
        else Lives = "You arrived late";

        collectedItemsText.text = "Items collected : " + CollectedItems.ToString() + "/5";
        BruceReputText.text = "Reputation with Bruce : " + BruceReput;
        WantedText.text = "You are " + Wanted;
        LivesText.text = Lives;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
