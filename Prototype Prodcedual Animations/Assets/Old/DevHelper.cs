using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevHelper : MonoBehaviour
{
    public SimpleIK[] allIKEndScripts;
    public int chainLength = 3;

    private void Awake()
    {
        for (int i = 0; i < allIKEndScripts.Length; i++)
            allIKEndScripts[i].ChainLength = chainLength;
    }

    void Update()
    {
        if(Input.GetButton("Jump"))
        {
            for (int i = 0; i < allIKEndScripts.Length; i++)
                allIKEndScripts[i].ChainLength = chainLength;
        }
    }
}
