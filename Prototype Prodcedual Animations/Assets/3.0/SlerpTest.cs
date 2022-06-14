using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpTest : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float journeyTime = 1f;
    public float speed;
    public bool repeatable;

    private float startTime;
    private Vector3 centerPoint;
    private Vector3 startRelCenter;
    private Vector3 endRelCenter;

    private void Update()
    {
        GetCenter(Vector3.up);

        if(!repeatable)
        {
            float fracComplete = (Time.time - startTime) / journeyTime * speed;
            transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
            transform.position += centerPoint;
        }
        else
        {
            float fracComplete = Mathf.PingPong(Time.time - startTime, journeyTime / speed);
            transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
            transform.position += centerPoint;

            if(fracComplete >= 1f)
                startTime = Time.time;
        }
    }

    public void GetCenter(Vector3 direction)
    {
        centerPoint = (startPos.position + endPos.position) * .5f;
        centerPoint -= direction;
        startRelCenter = startPos.position - centerPoint;
        endRelCenter = endPos.position - centerPoint;
    }
}
