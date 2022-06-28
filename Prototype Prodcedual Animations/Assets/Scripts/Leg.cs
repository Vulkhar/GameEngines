using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Erfüllt den Zweck damit das Bein sich nicht mit den Körper bewegt,
/// da es als Child am Körper hängt welches das <class>Controller</class>
/// Skript besitzt. 
/// 
/// Gute Werte:
/// JourneyTime = 0.5f
/// Speed = 1f
/// 
/// Body -> MoveSpeed -> 1
/// </summary>
public class Leg : MonoBehaviour
{
    [Header("Standard Positioning")]
    public Vector3 currentPos; //StartPos - Stelle an der das Bein fixiert wird
    public bool isMoving = false;

    [Header("Smooth Positioning")]
    public Transform startPos; //Start wenn ein neuer Lerp beginnt
    public Vector3 endPos; //Ziel während des Lerps
    public float journeyTime = .5f; //Dauer des Lerps
    public float speed = 1f; //Schnelligkeit des Lerps
    public float arcValue = 0.5f; //0.5 -> Richtiger Step im Halbkreis

    private float startTime;
    private Vector3 centerPoint;
    private Vector3 startRelCenter;
    private Vector3 endRelCenter;

    private void Start()
    {
        currentPos = transform.position;
    }

    private void Update()
    {
        if (!isMoving)
        {
            //Fixiere das Bein damit sich nur der Körper bewegt
            transform.position = currentPos;
        }
        else
            MoveLeg(endPos);
    }

    /// <summary>
    /// Bewege das Object smooth und nicht linear von der startPos zur endPos
    /// </summary>
    /// <param name="targetPos">= endPos => da wo man hin möchte</param>
    public void MoveLeg(Vector3 targetPos)
    {
        //Initialisiere die Daten einmal - toggle bool damit sich das Bein bewegen darf
        if (!isMoving)
        {
            isMoving = true;
            endPos = targetPos;
            startPos = transform;
            startTime = Time.time;
            SetCenter(startPos.position, endPos, transform.up);
        }

        //Bewege das Bein von der startPosition smoothly auf die endPosition
        float fracComplete = (Time.time - startTime) / journeyTime * speed;
        transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
        transform.position += centerPoint;

        //Wenn die Iteration 100% erreicht hat - toggle bool und setze die neue currentPos
        if (fracComplete >= 1f)
        {
            //startTime = Time.time;
            currentPos = endPos;
            isMoving = false;
        }
    }

    /// <summary>
    /// Setze das Zentrum der bewegung um eine schrittartige Bewegung zu erhalten
    /// und nicht über den Boden zu schleifen.
    /// </summary>
    /// <param name="startPos">Da wo sich das Object gerade befindet</param>
    /// <param name="endPos">Ziel wo das Object hinbewegt werden soll</param>
    /// <param name="direction">Richtung in die der Step ausgeführt wird (Bsp: transform.up -> macht einen Schritt nach oben)</param>
    private void SetCenter(Vector3 startPos, Vector3 endPos, Vector3 direction)
    {
        centerPoint = (startPos + endPos) * arcValue; //.5f -> halbkreis
        centerPoint -= direction;
        startRelCenter = startPos - centerPoint;
        endRelCenter = endPos - centerPoint;
    }
}
