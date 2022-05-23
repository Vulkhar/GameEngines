using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//TODO: Only Move leg when opposite legs are grounded!!! -> Dafür ist GroundCheck auf allen Legs drauf

public class Targetpoint : MonoBehaviour
{
    [Header("Leg Movement")]
    [SerializeField] private Transform legTransform; //Transform vom Bone/Mesh
    [SerializeField] private Transform targetTransform; //Transform von dem Punkt wohin der legTransform positioniert werden soll
    [SerializeField] private Transform oppositeTransfom; //Das Schräg gegenüberliegende -> Vorne links mit hinten rechts...
    [SerializeField] private LayerMask groundLayer; //Layer auf dem er stehen kann
    public Vector3 offset; //Offset damit die Detection richtig funktioniert
    [SerializeField] private float detectionRange = 1f; //Länge des Rays zum detecten des Layers
    public bool isHit; //Ist auf dem Boden?
    [SerializeField] private float distanceToMove = 1f; //Wird diese Distanz überschritten bewegt sich der Punkt
    public float distanceLegToTarget; //Distanz zwischen legTransform und targetTransform

    //Vorsicht hier mit Vector.up/down vielleicht Methode einbauen damit man das wechseln kann

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position + offset, transform.TransformDirection(Vector3.up), out hit, detectionRange, groundLayer))
        {
            isHit = true;
            Vector3 adjustPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            transform.position = adjustPosition + offset;
            Debug.DrawRay(transform.position + offset, transform.TransformDirection(Vector3.up) * detectionRange, Color.green);

            MoveTarget();
        }
        else
        {
            isHit = false;
            Debug.DrawRay(transform.position + offset, transform.TransformDirection(Vector3.up) * detectionRange, Color.red);
        }

        distanceLegToTarget = Vector3.Distance(transform.position, legTransform.position);
        Debug.DrawRay(transform.position, legTransform.position - transform.position, Color.yellow);
    }

    //Bewegt das Target wenn die Distanz zu groß wird zum vorherigen Punkt
    [Header("Lerping")]
    public AnimationCurve moveCurve; //curve polishing
    [Space]
    private Vector3 endPosition; //Wo wir hin möchten
    private Vector3 startPosition; //Wo wir anfangen
    private float elapsedTime;
    public float desiredDuration = 0.5f; //Wie lange er das machen soll

    public bool isMoving = false;

    private void MoveTarget()
    {
        float currentDistance = Vector3.Distance(transform.position, targetTransform.position);

        //Wenn die Distanz zu groß wird und das gegenüberliegende Target auf dem Boden ist:
        if (!isMoving && currentDistance >= distanceToMove /*&& oppositeTransfom.GetComponent<GroundCheck>().isGrounded*/)
        {
            isMoving = true;
            startPosition = targetTransform.position;
            endPosition = transform.position - offset;
        }

        //Distanz zu groß -> ich bewege mich
        if(isMoving)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / desiredDuration;
            targetTransform.position = Vector3.Lerp(startPosition, endPosition, moveCurve.Evaluate(percent)); //moveCurve.Evaluate(percent) || percent

            if (Vector3.Distance(targetTransform.position, endPosition) <= 0.0002f)
            {
                isMoving = false;
                elapsedTime = 0f;
            }
        }

        #region Snappy Movement Backup
        //float currentDistance = Vector3.Distance(transform.position, legTransform.position);
        ////Snappy laufen
        //if (currentDistance >= distanceToMove)
        //    targetTransform.position = transform.position;
        #endregion
    }

    private void OnDrawGizmos()
    {
        //Center Punkt Sphere
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, Vector3.up, 0.02f);
        Handles.DrawWireDisc(transform.position, Vector3.forward, 0.02f);
        Handles.DrawWireDisc(transform.position, Vector3.right, 0.02f);

        //legTransform Sphere
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(legTransform.position, Vector3.up, 0.05f);
        Handles.DrawWireDisc(legTransform.position, Vector3.forward, 0.05f);
        Handles.DrawWireDisc(legTransform.position, Vector3.right, 0.05f);

        //targetTransform Sphere
        Handles.color = Color.green;
        Handles.DrawWireDisc(targetTransform.position, Vector3.up, 0.03f);
        Handles.DrawWireDisc(targetTransform.position, Vector3.forward, 0.03f);
        Handles.DrawWireDisc(targetTransform.position, Vector3.right, 0.03f);
    }
}
