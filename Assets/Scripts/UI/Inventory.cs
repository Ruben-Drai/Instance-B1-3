using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Image Icon;

    public bool isTaken = false;
    public bool isInventoryBar = false;
    public static bool isInRoomPnC = false;
    private void Start()
    {
        Icon= GetComponent<Image>(); 
    }

    public void Update()
    {
        isTaken = DepthInfluenceManager.GetValue(gameObject.name)=="1";
        if (isInRoomPnC == true) 
        {
            GetComponent<Image>().enabled = true;
            //If the object is take, changes its icon to normal icon
            if (isTaken == true)
            {
                Icon.color = Color.white;
            }
            else if (isInventoryBar == true)
            {
                return;
            }
            //if not taken, changes to darkened icon
            else
            {
                Icon.color = Color.black;
            }
        }
        else
        {
            GetComponent <Image>().enabled = false;
        }
    }
}
