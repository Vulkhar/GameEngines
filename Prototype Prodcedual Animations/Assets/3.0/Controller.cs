using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool isAuto = false;
    public float moveSpeed = 2f;

    private void Update()
    {
        if (isAuto)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        else
        {
            float z = Input.GetAxis("Vertical");

            //Bewege Nach vorne/zurück
            if(z != 0)
                transform.Translate(Vector3.forward * (z * moveSpeed) * Time.deltaTime);
        }
    }
}
