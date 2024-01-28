using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    public Transform model;
    [SerializeField] private float groundCheckRayLength = 0.5f;
    public bool grounded;
    public bool canMove = true;
    public Vector2 look;
    [SerializeField] ParticleSystem moveParticleSystem;
    [SerializeField] private float coyoteJumpDuration = 0.2f;
    private Coroutine coyote;

    private Brothers brother;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        brother = GetComponent<Brothers>();
    }

    private void Update() {
        CheckGrounded();

        if (grounded && !moveParticleSystem.isPlaying)
            moveParticleSystem.Play();
        else if (!grounded && moveParticleSystem.isPlaying)
            moveParticleSystem.Stop();

        Debug.DrawLine(transform.position + Vector3.up * 0.2f, transform.position + Vector3.down * groundCheckRayLength, Color.blue);
    }

    private void FixedUpdate() {
        ApplyAdditionalGravity();
    }

    void ApplyAdditionalGravity() {
        if (!canMove || !rb.useGravity || grounded)
            return;
        if (rb.velocity.y > 0f)
            rb.velocity += -Vector3.up * 0.4f;
        else if (rb.velocity.y < 0f)
            rb.velocity += -Vector3.up * 0.2f;
    }

    public void Move(Vector2 direction) {
        if (!canMove)
            return;
        Vector3 axisFix = new Vector3(direction.x, 0f, direction.y);
        animator.SetFloat("velocity", axisFix.magnitude);
        animator.SetFloat("x", axisFix.x);
        animator.SetFloat("y", axisFix.y);
        transform.Translate(axisFix * (speed * Time.deltaTime));

        if (direction == Vector2.zero)
            return;
        
        if (look.magnitude < 0.3f) {
            float angle = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
            model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.x, angle + 90f,
                model.localRotation.eulerAngles.z);
        }
    }

    public void Look(Vector2 lookDir) {
        look = lookDir;
        if (!canMove)
            return;

        if (look.magnitude < 0.3f) {
            look = Vector3.zero;
            return;
        }

        float angle = Mathf.Atan2(-look.y, look.x) * Mathf.Rad2Deg;
        model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.x, angle + 90f, model.localRotation.eulerAngles.z);
    }

    public void Jump() {
        if (!grounded || !canMove)
            return;

        rb.velocity = Vector3.up * jumpForce;
        brother.audioSourceVoice.PlayOneShot(brother.jump);
        animator.SetTrigger("jump");
    }

    public void CutJump() {
        if (rb.velocity.y > 0f)
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / 2f, rb.velocity.z);
    }

    void CheckGrounded() {
        Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, groundCheckRayLength);
        if (hit.collider) {
            if (coyote != null)
                StopCoroutine(coyote);
            grounded = true;
        }
        else {
            StartCoroutine(CoyoteJump());
        }
    }

    IEnumerator CoyoteJump() {
        for (float i = 0f; i < coyoteJumpDuration; i += Time.deltaTime) {
            yield return null;
        }
        grounded = false;
    }
}