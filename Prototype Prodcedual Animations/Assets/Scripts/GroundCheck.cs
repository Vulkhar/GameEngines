using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float rayLength = 0.1f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private LayerMask groundMask;
    public bool isGrounded;
    public RaycastHit hit;

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position + offset, Vector3.down, out hit, rayLength, groundMask);
        Debug.DrawRay(transform.position, Vector3.down * rayLength, isGrounded ? Color.red: Color.green);
    }
}
