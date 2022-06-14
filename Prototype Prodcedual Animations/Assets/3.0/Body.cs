using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <c>Body</c> sitzt auf dem Hauptkörper des Spielers und ist in zwei Teile aufgeteilt
/// 
/// Einmal die Anpassung der Höhe in Abhängigkeit von den Beinen. Es wird ein Average von den
/// Beinen bestimmt und darauf ein Offset addiert um den Körper nie am Boden schleifen zu lassen
/// 
/// Anhand der Vorder/Hinter und Seitenbeine wird eine Rotation errechnet (ebenfalls durch das Average)
/// um herauszufinden welche Seite des Körpers höher ist. Das Average der jeweiligen Seite wird mit einem
/// Rotationsmultiplier multipliziert um einen stärkeren Effekt zu erzielen.
/// 
/// </summary>
public class Body : MonoBehaviour
{

    private void Update()
    {
        HandleBodyHeight();
        HandleBodyRotation();
    }

    [Header("Body Height")]
    public Transform[] legs; //Referenz der RIG Beine (BL Target, FL Target,...)
    public float offset; //Um den Ray höher anzusetzen
    public LayerMask groundMask;
    public float rayLength = 3f;

    /// <summary>
    /// Setze die Y-Pos des Körpers anhand der durchschnittlichen
    /// Leg Posiitons.
    /// </summary>
    private void HandleBodyHeight()
    {
        Vector3 average = LegAverage(legs);
        //Debug.Log("Average Vector: " + average);
        transform.position = new Vector3(transform.position.x, average.y + offset, transform.position.z);
    }

    private float OldLegAverage(Transform[] allLegs)
    {
        float sumLegs = 0f;
        for (int i = 0; i < allLegs.Length; i++)
            sumLegs += allLegs[i].position.y;

        return sumLegs / (allLegs.Length + 1);
    }

    /// <summary>
    /// Berechne das Average der Array Elemente
    /// </summary>
    /// <param name="allLegs">Array der Beine von denen das Average bestimmt werden soll</param>
    /// <returns>Average Vector</returns>
    private Vector3 LegAverage(Transform[] allLegs)
    {
        Vector3 average = Vector3.zero;

        if (allLegs.Length == 0)
            return average;

        foreach (var leg in allLegs)
        {
            average.x += leg.position.x;
            average.y += leg.position.y;
            average.z += leg.position.z;
        }

        return average / allLegs.Length;
    }

    [Header("Body Rotation")]
    public float rotationMultiplier = 20f; //Wert zum verstärken der Rotation

    /// <summary>
    /// Rotiert den Körper/Torso abhängig zu der Höhe der Beine
    /// Beim Setup darauf achten das die richtigen Beine aus dem Array benutzt werden.
    /// </summary>
    private void HandleBodyRotation()
    {
        #region Rotate Vorne/Hinten

        Vector3 frontAverage, backAverage;
        frontAverage = LegAverage(new Transform[] { legs[1], legs[3] });
        backAverage = LegAverage(new Transform[] { legs[0], legs[2] });
        //Debug.Log("FrontAverage: " + frontAverage + "     BackAverage: " + backAverage+"      Calc: "+(frontAverage-backAverage));

        float xA = 0f;

        if (frontAverage.y > backAverage.y)
            xA = (Mathf.Abs(frontAverage.y - backAverage.y)) * -1f * rotationMultiplier;
        else if (frontAverage.y < backAverage.y)
            xA = Mathf.Abs(frontAverage.y - backAverage.y) * rotationMultiplier;
        else
            xA = 0f;

        #endregion

        #region Rotate Links/Rechts

        Vector3 rightAverage, leftAverage;
        rightAverage = LegAverage(new Transform[] { legs[2], legs[3] });
        leftAverage = LegAverage(new Transform[] { legs[0], legs[1] });
        //Debug.Log("rightAverage: " + rightAverage + "     leftAverage: " + leftAverage + "      Calc: " + (leftAverage-rightAverage));

        float zA = 0f;
        if (leftAverage.y > rightAverage.y)
            zA = (Mathf.Abs(leftAverage.y - rightAverage.y)) * -1f * rotationMultiplier;
        else if (leftAverage.y < rightAverage.y)
            zA = Mathf.Abs(leftAverage.y - rightAverage.y) * rotationMultiplier;
        else
            zA = 0f;

        #endregion

        //Apply Rotation
        transform.eulerAngles = new Vector3(xA, transform.eulerAngles.y, zA);
    }


}
