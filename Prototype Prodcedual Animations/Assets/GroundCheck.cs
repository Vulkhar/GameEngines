using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float rayLength = 0.01f;
    [SerializeField] private LayerMask groundMask;
    public bool isGrounded;
    private RaycastHit hit;

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundMask);

        Debug.DrawRay(transform.position, Vector3.down * rayLength, isGrounded ? Color.yellow: Color.red);
    }
}
