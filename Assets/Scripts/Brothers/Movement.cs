using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 0.8f;
    [SerializeField] private float maxSpeedCap = 10f;
    [SerializeField] private float jumpForce = 5f;
    public Transform model;
    [SerializeField] private float groundCheckRayLength = 0.5f;
    public bool canMove = true;
    [SerializeField] private ParticleSystem moveParticleSystem;
    [SerializeField] private float coyoteJumpDuration = 0.2f;

    private Brother brother;
    private Rigidbody rb;
    private Coroutine coyote;
    private Vector2 look;

    private bool isGrounded;
    private static readonly int X = Animator.StringToHash("x");
    private static readonly int Y = Animator.StringToHash("y");
    private static readonly int Velocity = Animator.StringToHash("velocity");
    private static readonly int JumpTrigger = Animator.StringToHash("jump");

    public bool IsGrounded
    {
        get => isGrounded;
        private set {
            if (isGrounded == value)
                return;
            
            if (value)
                moveParticleSystem.Play();
            else
                moveParticleSystem.Stop();
            
            isGrounded = value;
        }
    }

    private void Awake() {
        brother = GetComponent<Brother>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        CheckGrounded();
        Debug.DrawLine(transform.position + Vector3.up * 0.2f, transform.position + Vector3.down * groundCheckRayLength, Color.blue);
    }

    private void FixedUpdate() {
        ApplyAdditionalGravity();
    }

    private void ApplyAdditionalGravity() {
        if (!canMove || !rb.useGravity || IsGrounded)
            return;
        if (rb.velocity.y > 0f)
            rb.velocity += -Vector3.up * 0.4f;
        else if (rb.velocity.y < 0f)
            rb.velocity += -Vector3.up * 0.2f;
    }

    public void Move(Vector2 direction) {
        if (!canMove)
            return;
        Vector3 axisFix = new Vector3(direction.y, 0f, direction.x);
        
        animator.SetFloat(Velocity, axisFix.magnitude);
        animator.SetFloat(X, axisFix.x);
        animator.SetFloat(Y, axisFix.y);

        Transform cameraTransform = CameraController.Instance.transform;
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        
        // FIXME ignore y velocity
        if (rb.velocity.magnitude < maxSpeedCap)
            rb.velocity += (cameraForward * axisFix.x + cameraRight * axisFix.z) * speed;

        if (direction == Vector2.zero)
            return;
        
        if (look.magnitude >= 0.3f)
            return;
        
        float angle = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion localRotation = model.localRotation;
        model.localRotation = Quaternion.Euler(localRotation.eulerAngles.x, angle + 90f,
            localRotation.eulerAngles.z);
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
        Quaternion localRotation = model.localRotation;
        model.localRotation = Quaternion.Euler(localRotation.eulerAngles.x, angle + 90f, localRotation.eulerAngles.z);
    }

    public void Jump() {
        if (!IsGrounded || !canMove)
            return;

        rb.velocity += Vector3.up * jumpForce;
        brother.audioSourceVoice.PlayOneShot(brother.jump);
        animator.SetTrigger(JumpTrigger);
    }

    public void CutJump()
    {
        Vector3 velocity = rb.velocity;
        if (velocity.y > 0f)
            rb.velocity = new Vector3(velocity.x, velocity.y / 2f, velocity.z);
    }

    private void CheckGrounded() {
        Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, groundCheckRayLength);
        if (hit.collider) {
            if (coyote != null)
            {
                StopCoroutine(coyote);
                coyote = null;
            }
            IsGrounded = true;
        } else
            coyote = StartCoroutine(CoyoteJump());
    }

    private IEnumerator CoyoteJump() {
        for (float i = 0f; i < coyoteJumpDuration; i += Time.deltaTime) {
            yield return null;
        }
        IsGrounded = false;
    }
}