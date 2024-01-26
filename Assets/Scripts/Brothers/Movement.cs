using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    private Rigidbody rb;
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform model;
    [SerializeField] private float groundCheckRayLength = 0.5f;
    private bool grounded;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        CheckGrounded();
        Debug.DrawLine(transform.position + Vector3.up * 0.2f, transform.position + Vector3.down * groundCheckRayLength, Color.blue);
        ApplyAdditionalGravity();
    }

    void ApplyAdditionalGravity() {
        if (grounded)
            return;
        if (rb.velocity.y > 0f)
            rb.velocity += -Vector3.up * 0.4f;
        else if (rb.velocity.y < 0f)
            rb.velocity += -Vector3.up * 0.2f;
    }

    public void Move(Vector2 direction) {
        Vector3 axisFix = new Vector3(direction.x, 0f, direction.y);
        transform.Translate(axisFix * (speed * Time.deltaTime));
    }

    public void Look(Vector2 direction) {
        if (!(direction.magnitude > 0.1f))
            return;
        
        float angle = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
        model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.x, angle + 90f, model.localRotation.eulerAngles.z);
    }

    public void Jump() {
        if (!grounded)
            return;

        rb.velocity = Vector3.up * jumpForce;
    }

    public void CutJump() {
        if (rb.velocity.y > 0f)
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / 2f, rb.velocity.z);
    }

    void CheckGrounded() {
        Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, groundCheckRayLength);
        grounded = hit.collider;
    }
}