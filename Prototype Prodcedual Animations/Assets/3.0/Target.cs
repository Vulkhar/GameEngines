using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skript für den Zielpunkt des Beines. Dieses bewegt sich mit dem Körper während
/// das Bein auf einer Stelle bleibt bis die Distanz zu dem Zielpunkt zu groß wird.
/// </summary>
/// 
public class Target : MonoBehaviour
{
    [Header("Handle Height")]
    public float rayLength = 0.05f; //.22f -> 3f
    public LayerMask groundLayer;
    public Color rayColor = Color.green;
    public Vector3 offset; //Offset wo der Ray starten soll
    public Vector3 lastCorrectPosition; //Letzte Position die richtig war - um Bodenglitch zu vermeiden

    [Header("Handle Leg Position")]
    public Transform legRig; //Referenz auf BL Target, FL Target,... im Rig
    private Leg leg;
    public float maxDistanceToMove = 2f; //Distance wann sich das Bein bewegen muss
    public Leg[] oppositeLegs; //Referenz zu den Beinen bei dem sich dieses Bein nicht bewegen darf wenn sie sich bewegen

    [Header("Fix X-Z Positions")]
    public Vector3 initialPosition; //Lokal X,Z werden benutzt damit sich die Punkte nur nach oben und unten verschieben

    private void Start()
    {
        leg = legRig.GetComponent<Leg>();
        leg.endPos = transform.position - offset;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        HandleHeight();
        MoveLeg();
        FixXY();
    }

    /// <summary>
    /// Schießt einen Raycast nach unten um zu schauen ob die Fläche uneben ist.
    /// Basierend darauf wird die Transform von dem Target überschrieben.
    /// </summary>
    private void HandleHeight()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(transform.position + offset, Vector3.down, out hit, rayLength, groundLayer);
        
        if(isHit)
        {
            Debug.DrawRay(transform.position + offset, Vector3.down * rayLength, rayColor);

            //Passe Y-Position an wenn der Boden getroffen wurde
            Vector3 newPos = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            transform.position = newPos + offset; //Merke das der Offset von der eigentlichen Beinposition abgezogen werden muss!
            lastCorrectPosition = transform.position;
        }
        else
        {
            Debug.DrawRay(transform.position + offset, Vector3.down * rayLength, Color.red);

            Debug.LogWarning("Use Last Correct Position");
            transform.position = lastCorrectPosition;
        }
    }

    /// <summary>
    /// Bewege das Target XX Target vom Rig damit das "sichtbare Bein", also das Mesh
    /// die neue Position annimmt
    /// </summary>
    private void MoveLeg()
    {
        float currentDistance = Vector3.Distance(transform.position, legRig.position);
        bool canMove = true;

        if(oppositeLegs.Length != 0)
        {
            foreach (Leg l in oppositeLegs)
            {
                if (l.isMoving)
                {
                    canMove = false;
                    break;
                }
            }
        }
        else
            Debug.LogWarning("Opposite Legs not defined");

        /*Bewege das Bein nur wenn:
         *Die Distanz zu diesem Target überschritten wird
         *Das Bein sich gerade nicht bewegt
          Die gegenüberliegende Beine sich gerade nicht bewegen*/
        if(currentDistance > maxDistanceToMove && !leg.isMoving && canMove)
        {
            leg.MoveLeg(transform.position - offset);

            #region Backup - Snappy Movement
            //Snappy Movement - Backup (falls das benutzt wird muss Leg.cs angepasst werden)
            //leg.currentPos = transform.position-offset;
            #endregion
        }
    }

    /// <summary>
    /// Sorgt dafür das sich die Punkte nur nach oben bewegen können.
    /// In manchen Fällen haben sie sich verschoben was den Laufcycle
    /// zerstört hat.
    /// </summary>
    private void FixXY()
    {
        transform.localPosition = new Vector3(initialPosition.x, transform.localPosition.y, initialPosition.z);
    }
}
