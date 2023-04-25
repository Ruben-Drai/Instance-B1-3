using UnityEngine;

public class buttonAnim : MonoBehaviour
{

    public float rythme;

    private Vector3 originPosition;
    private float timeAnchor;
    private bool isAtOrigin;

    void Start()
    {
        originPosition = transform.position;
        timeAnchor = Time.realtimeSinceStartup;
        isAtOrigin = true;
    }

    void Update()
    {
        if (Time.realtimeSinceStartup > timeAnchor + rythme)
        {
            timeAnchor = Time.realtimeSinceStartup;
            ChangePos();
        }
    }
    private void ChangePos()
    {
        if (isAtOrigin)
        {
            isAtOrigin = false;
            transform.position = new Vector3(originPosition.x, originPosition.y - 0.2f, originPosition.z);
        }
        else
        {
            isAtOrigin = true;
            transform.position = new Vector3(originPosition.x, originPosition.y, originPosition.z);
        }
    }
}