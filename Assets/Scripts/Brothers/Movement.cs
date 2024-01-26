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

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        Debug.DrawLine(transform.position + Vector3.up * 0.2f, transform.position + Vector3.down * groundCheckRayLength, Color.blue);
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

        Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, groundCheckRayLength);
        if (!hit.collider)
            return;
        
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}