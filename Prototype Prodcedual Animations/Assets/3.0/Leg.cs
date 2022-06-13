using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    public Vector3 currentPos; //StartPos - Stelle an der das Bein fixiert wird
    public bool isMoving = false;

    private void Start()
    {
        currentPos = transform.position;
    }

    private void Update()
    {
        //if (!isMoving)
        //{
            //Fixiere das Bein damit sich nur der Körper bewegt
            transform.position = currentPos;
        //}
        //else
        //    MoveLeg(targetSlerpPos);
    }

    public Vector3 startSlerpPos;
    public Vector3 targetSlerpPos;
    
    public void MoveLeg(Vector3 targetPos)
    {
        //Setze bool auf ICH BEWEGE JETZT und disable UPDATE
        if (!isMoving)
        {
            isMoving = true;
            targetSlerpPos = targetPos;
        }

        //LERPE DAS BEIN AUF die neue currentPOS
        transform.position = targetSlerpPos;

        //wenn CURRENT POS ERREICHT IST -> switche den bool
        if (transform.position == targetPos)
            isMoving = false;
    }
}
