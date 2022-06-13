using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    private void Start()
    {
        startY = transform.position.y - LegAverage(legs);
    }

    private void Update()
    {
        HandleBodyHeight();
    }

    [Header("Body Height")]
    public Transform[] legs;
    public float startY; //Y-Pos mit der gestartet wird
    /// <summary>
    /// Setze die Y-Pos des Körpers anhand der durchschnittlichen
    /// Leg Posiitons.
    /// </summary>
    public void HandleBodyHeight()
    {
        float average = LegAverage(legs);
        Debug.Log("Average: " + average);
        //average = average / 25f;
        //Debug.Log("Average///: " + average);

        if (startY + average != transform.position.y)
            transform.position = new Vector3(transform.position.x, startY + average, transform.position.z);

        //Vector3 side1 = legs[0].position - legs[3].position;
        //Vector3 side2 = legs[2].position - legs[1].position;
        //Vector3 cross = Vector3.Cross(side1, side2);
        //Debug.Log("CROSS: " + cross);
        //Debug.DrawRay(transform.position, cross, Color.cyan);
    }

    private float LegAverage(Transform[] allLegs)
    {
        float sumLegs = 0f;
        for (int i = 0; i < allLegs.Length; i++)
            sumLegs += allLegs[i].position.y;

        return sumLegs / (allLegs.Length + 1);
    }
}
