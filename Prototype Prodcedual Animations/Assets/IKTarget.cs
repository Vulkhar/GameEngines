using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IKTarget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform legTransform; //Transform vom Bone/Mesh
    [SerializeField] private Transform targetTransform; //Transform von dem Punkt wohin der legTransform positioniert werden soll
    [SerializeField] private Transform oppositeTransfom; //Das Schräg gegenüberliegende -> Vorne links mit hinten rechts...
    private GroundCheck groundCheck, groundCheckOpposite;

    [Header("Parameters")]
    [SerializeField] private LayerMask groundLayer; //Layer auf dem er stehen kann
    public Vector3 offset; //Offset damit die Detection richtig funktioniert
    [SerializeField] private float detectionRange = 1f; //Länge des Rays zum detecten des Layers
    [SerializeField] private float distanceToMove = 1f; //Wird diese Distanz überschritten bewegt sich der Punkt
    public float distanceLegToTarget; //Distanz zwischen legTransform und targetTransform
    //private bool isHit; //Trifft den Boden?

    private void Awake()
    {
        groundCheck = legTransform.transform.GetComponent<GroundCheck>();
    }

    private void Update()
    {
        if(groundCheck.isGrounded)
        {
            Vector3 adjustPosition = new Vector3(transform.position.x, groundCheck.hit.point.y, transform.position.z);
            transform.position = adjustPosition + offset;
            
            //MoveTarget();
        }
    }
}
