using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlaneTracker : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Rigidbody rb;
    [SerializeField] Vector3 lastKnownValidLocation;
    [SerializeField] private bool grounded;
    [SerializeField] private float groundCheckRayLength = 0.5f;
    
    private void Start() {
        StartCoroutine(TrackLastKnownLocation());
    }
    
    void Update()
    {
        Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, groundCheckRayLength);
        grounded = hit.collider;
        
        if (transform.position.y < -10f) {
            transform.position = lastKnownValidLocation;
            if (rb) {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Brothers brothers = GetComponent<Brothers>();
            if (brothers)
                brothers.Stun();
            else
                StartCoroutine(HitFlashing());
        }
    }
    
    IEnumerator TrackLastKnownLocation() {
        while (true) {
            if (grounded) {
                lastKnownValidLocation = transform.position;
            }
            yield return new WaitForSeconds(0.4f);
        }
    }
    
    IEnumerator HitFlashing() {
        for (float i = 0; i < 3f; i += Time.deltaTime) {
            if (i % 0.2f < 0.1f) {
                if (meshRenderer)
                    meshRenderer.enabled = false;
                if (skinnedMeshRenderer)
                    skinnedMeshRenderer.enabled = false;
            }
            else {
                if (meshRenderer)
                    meshRenderer.enabled = true;
                if (skinnedMeshRenderer)
                    skinnedMeshRenderer.enabled = true;
            }
            yield return null;
        }
        if (meshRenderer)
            meshRenderer.enabled = true;
        if (skinnedMeshRenderer)
            skinnedMeshRenderer.enabled = true;
    }
}
