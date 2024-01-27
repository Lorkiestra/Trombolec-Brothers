using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brothers : MonoBehaviour {
    protected Rigidbody rb;
    protected Movement movement;
    [SerializeField] private Animator animator;
    [SerializeField] protected PropCollector tromba;
    private Coroutine groundPounding;
    [SerializeField] private float groundPoundForce = 100f;
    [SerializeField] protected Transform trombaModel;
    
    public abstract void Trombone();
    public abstract void TromboneRelease();

    private void Awake() {
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
    }

    public void GroundPound() {
        if (movement.grounded)
            return;

        groundPounding ??= StartCoroutine(GroundPoundCoroutine());
    }
    
    IEnumerator GroundPoundCoroutine() {
        movement.canMove = false;
        movement.rb.isKinematic = true;
        movement.rb.useGravity = false;
        animator.SetTrigger("ground_pound");
        float waitTime = .5f;
        while (waitTime > 0f) {
            waitTime -= Time.deltaTime;
            yield return null;
        }
        movement.rb.useGravity = true;
        movement.rb.isKinematic = false;
        movement.rb.AddForce(Vector3.down * 1000f, ForceMode.Impulse);
        while (!movement.grounded) {
            yield return null;
        }
        animator.SetTrigger("ground_pound_impact");
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 5f)) {
            // TODO fala uderzeniowa i damage
            Prop prop = collider.GetComponent<Prop>();
            if (prop) {
                collider.GetComponent<Prop>().rb.AddForce(Vector3.up * groundPoundForce + (collider.transform.position - transform.position) * groundPoundForce / 7f, ForceMode.Impulse);
            }
            /*
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy) {
                enemy.Die();
            }
            */
        }

        waitTime = 0.5f;
        while (waitTime > 0f) {
            waitTime -= Time.deltaTime;
            yield return null;
        }
        movement.canMove = true;
        groundPounding = null;
    }
}
