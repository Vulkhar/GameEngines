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
    private void Start()
    {
        leg = legRig.GetComponent<Leg>();
    }

    private void Update()
    {
        HandleHeight();
        MoveLeg();
    }

    [Header("Handle Height")]
    public float rayLength = 0.05f; //.22f
    public LayerMask groundLayer;
    public Color rayColor = Color.green;
    public Vector3 offset;
    public Vector3 lastCorrectPosition;
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
    [Header("Handle Leg Position")]
    public Transform legRig;
    private Leg leg;
    public float maxDistanceToMove = 2f; //Distance wann sich das Bein bewegen muss
    private void MoveLeg()
    {
        float currentDistance = Vector3.Distance(transform.position, legRig.position);

        if(currentDistance > maxDistanceToMove && !leg.isMoving)
        {

            leg.currentPos = transform.position;
            //leg.MoveLeg(transform.position);
        }
    }
}
