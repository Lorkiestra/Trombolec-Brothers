using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brothers : MonoBehaviour {
    protected Rigidbody rb;
    protected Movement movement;
    [SerializeField] protected PropCollector tromba;
    private Coroutine groundPounding;
    [SerializeField] private float groundPoundForce = 100f;
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
        Debug.Log("start pound");
        movement.canMove = false;
        movement.rb.isKinematic = true;
        movement.rb.useGravity = false;
        // TODO odpal animacje flipa
        for (int i = 0; i < 60; i++) {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("flip end go down");
        movement.rb.useGravity = true;
        movement.rb.isKinematic = false;
        movement.rb.AddForce(Vector3.down * 1000f, ForceMode.Impulse);
        while (!movement.grounded) {
            yield return null;
        }
        Debug.Log("jebut o ziemię");
        // TODO jebut o ziemię
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
        
        for (int i = 0; i < 20; i++) {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("end");
        movement.canMove = true;
        groundPounding = null;
    }
}
