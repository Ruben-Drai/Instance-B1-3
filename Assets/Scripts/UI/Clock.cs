using UnityEngine;

public class Clock : MonoBehaviour
{
    public static GameObject instance;
    public Transform minuteHand;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null) instance = gameObject;
        else Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        float rotation = (float.Parse(DepthInfluenceManager.GetValue("Lives")) * 5f * 5.85f);
        instance.GetComponent<Clock>().minuteHand.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
