using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public bool isAuto = false;
    public float speed;

    private void Update()
    {
        if(isAuto)
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        else
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            transform.Translate(new Vector3(x, 0f, y) * speed * Time.deltaTime);
        }
    }
}
