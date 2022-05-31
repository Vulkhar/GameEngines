using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAdaption : MonoBehaviour
{
    [Header("Body Height")]
    public float divider = 25f; //Das Raw Average hat einen zu großen Einfluss auf die Body position, deswegen wird es durch diesen wert dividiert
    private float startBodyHeight;

    [Header("Body Rotation")]
    [SerializeField] private Transform leftBackLeg, rightBackLeg, leftFrontLeg, rightFrontLeg;
    [SerializeField] private float rotationMultiplier = 20f;
    public float zStartAngle; //Tilt nach vorne/hinten
    public float yStartAngle; //Tilt seitwärts

    private void Start()
    {
        startBodyHeight = transform.localPosition.z;
        zStartAngle = transform.localEulerAngles.z;
        yStartAngle = transform.localEulerAngles.y;
    }

    private void Update()
    {
        AdjustBodyPosition();

        Rotate();
    }

    private void AdjustBodyPosition()
    {
        float legAverage = (leftBackLeg.position.y + rightBackLeg.position.y + leftFrontLeg.position.y + rightFrontLeg.position.y) / 4;
        //Debug.Log("average: " + legAverage);
        legAverage = legAverage / divider;

        if (startBodyHeight + legAverage != transform.localPosition.z)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.02f + legAverage);
    }

    //TODO: Lerping wie bei Fußpositions
    private void Rotate()
    {
        //Tilt nach vorne/hinten (RootAchse = Z)
        float frontAverage = leftFrontLeg.position.y + rightFrontLeg.position.y;
        frontAverage = frontAverage / 2f;

        float backAverage = leftBackLeg.position.y + rightBackLeg.position.y;
        backAverage = backAverage / 2f;

        Debug.Log("frontAverage: " + frontAverage);
        Debug.Log("backAverage: " + backAverage);

        float currentZ = transform.localEulerAngles.z;
        if(frontAverage > backAverage)
        {
            if (currentZ != zStartAngle + frontAverage * rotationMultiplier)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zStartAngle + frontAverage * rotationMultiplier);
        }
        else if(backAverage > frontAverage)
        {
            if (currentZ != zStartAngle - backAverage * rotationMultiplier)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zStartAngle - backAverage * rotationMultiplier);
        }
        else
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zStartAngle);


        //Tilt seitwärts (RootAchse = Y)
        float rightAverage = rightFrontLeg.position.y + rightBackLeg.position.y;
        rightAverage = rightAverage / 2;

        float leftAverage = leftFrontLeg.position.y + leftBackLeg.position.y;
        leftAverage = leftAverage / 2;

        float currentY = transform.localEulerAngles.y;
        if(rightAverage > leftAverage)
        {
            if (currentY != yStartAngle - rightAverage * rotationMultiplier)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yStartAngle - rightAverage * rotationMultiplier, transform.localEulerAngles.z);
        }
        else if(leftAverage > rightAverage)
        {
            if (currentY != yStartAngle + leftAverage * rotationMultiplier)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yStartAngle + leftAverage * rotationMultiplier, transform.localEulerAngles.z);
        }
        else
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yStartAngle, transform.localEulerAngles.z);
    }

    //private Vector3 endPosition, startPosition;
    //private float elapsedTime;
    //public float desiredDuration = .5f;
    //public bool isRotating = false;
    //private void RotateSmooth()
    //{
    //    float frontAverage = leftFrontLeg.position.y + rightFrontLeg.position.y;
    //    frontAverage = frontAverage / 2f;

    //    float backAverage = leftBackLeg.position.y + rightBackLeg.position.y;
    //    backAverage = backAverage / 2f;

    //    float currentZ = transform.localEulerAngles.z;
    //    if (frontAverage > backAverage)
    //    {
    //        if (!isRotating)
    //        {
    //            startPosition = transform.localEulerAngles;
    //            endPosition = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zStartAngle + frontAverage * rotationMultiplier);
    //            isRotating = true;
    //        }

    //        if (currentZ != zStartAngle + frontAverage * rotationMultiplier && isRotating)
    //        {
    //            elapsedTime += Time.deltaTime;
    //            float percent = elapsedTime / desiredDuration;
    //            transform.localEulerAngles = Vector3.Lerp(startPosition, endPosition, percent);
    //        }

    //        if (transform.localEulerAngles == endPosition)
    //            isRotating = false;
    //    }
    //    else if (backAverage > frontAverage)
    //    {
    //        if(!isRotating)
    //        {
    //            startPosition = transform.localEulerAngles;
    //            endPosition = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zStartAngle - backAverage * rotationMultiplier);
    //            isRotating = true;
    //        }

    //        if (currentZ != zStartAngle - backAverage * rotationMultiplier && isRotating)
    //        {
    //            elapsedTime += Time.deltaTime;
    //            float percent = elapsedTime / desiredDuration;
    //            transform.localEulerAngles = Vector3.Lerp(startPosition, endPosition, percent);
    //        }

    //        if (transform.localEulerAngles == endPosition)
    //            isRotating = false;
    //    }
    //    else
    //        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zStartAngle);
    //}

    #region Alt
    //private void RaycastAdjustBodyHeightPosition()
    //{
    //    //TODO: Körper statt mit Raycast -> Anhand der "average leg position + offset" berechnen (Step 9) https://www.youtube.com/watch?v=e6Gjhr1IP6w
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 5f, groundMask))
    //    {
    //        float currentDistance = Vector3.Distance(transform.position, hit.point);

    //        //Wenn er hoch geht -> Körper höher machen
    //        if (currentDistance < initialDistanceToGround)
    //        {
    //            Vector3 adjustedPosition = new Vector3(transform.position.x, transform.position.y + (initialDistanceToGround - currentDistance), transform.position.z);
    //            transform.position = adjustedPosition;
    //        }
    //        //Wenn er runter geht -> Körper runter machen
    //        else if (currentDistance > initialDistanceToGround)
    //        {
    //            Vector3 adjustedPosition = new Vector3(transform.position.x, transform.position.y - (currentDistance - initialDistanceToGround), transform.position.z);
    //            transform.position = adjustedPosition;
    //        }
    //    }
    //}
    #endregion
}
