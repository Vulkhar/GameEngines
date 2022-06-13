using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool isAuto = false;
    public float moveSpeed = 2f;
    public float rotSpeed = 2f;

    private void Update()
    {
        if (isAuto)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        else
        {
            float z = Input.GetAxis("Vertical");
            float y = Input.GetAxis("Horizontal");

            //Drehe links/rechts
            if (y != 0)
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * y * rotSpeed);

            //Bewege Nach vorne/zurück
            if(z != 0)
                transform.Translate(Vector3.forward * (z * moveSpeed) * Time.deltaTime);
        }
    }
}
