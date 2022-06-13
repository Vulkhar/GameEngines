using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public bool isAuto = false;
    public float speed;
    public float rotSpeed = 0.1f;

    private void Update()
    {
        if(isAuto)
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        else
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxis("Vertical");

            if (x != 0)
            {
                transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + (rotSpeed * x) * Time.deltaTime, transform.rotation.z));
                Debug.Log("ROT:" + x);
            }

            if (y != 0)
                transform.Translate((-Vector3.forward * (y * speed)) * rotSpeed * Time.deltaTime);
        }
    }
}
