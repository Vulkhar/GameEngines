using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    public float factor = 1f;
    private void Update()
    {
        Time.timeScale = factor;
    }
}
