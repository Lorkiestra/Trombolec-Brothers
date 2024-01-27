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
    public Transform trombaModel;

    [SerializeField] protected AudioClip trombaPierdzenie;
    [SerializeField] protected AudioSource audioSource;

    [SerializeField] protected float powiekszSwojaTrombe = .5f;

    [SerializeField] private int hitPoints = 3;
    [SerializeField] private float stunnedTime;
    
    [SerializeField] float knockbackForce = 30f;

    public virtual void Trombone() {
        if (stunnedTime > 0f)
            return;
    }

    public virtual void TromboneRelease() {
        if (stunnedTime > 0f)
            return;
    }

    private void Awake() {
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Update() {
        if (stunnedTime > 0f) {
            stunnedTime -= Time.deltaTime;
            if (stunnedTime <= 0f) {
                animator.SetTrigger("stun_end");
                movement.canMove = true;
            }
        }
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
        }

        foreach (Collider collider in Physics.OverlapSphere(transform.position, 1f)) {
            BasicEnemy enemy = collider.GetComponent<BasicEnemy>();
            if (enemy) {
                enemy.Die();
            }
        }

        waitTime = 0.5f;
        while (waitTime > 0f) {
            waitTime -= Time.deltaTime;
            yield return null;
        }
        movement.canMove = true;
        groundPounding = null;
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log($"{name} on collision enter");
        Debug.Log(other.transform.name);
        if (other.transform.CompareTag("Enemy")) {
            TakeHit(other.transform.position);
        }
    }

    public void TakeHit(Vector3 enemyPosition) {
        Debug.Log($"{name} take hit");
        if (stunnedTime > 0f)
            return;
        
        hitPoints--;
        rb.AddForce(Vector3.Normalize(enemyPosition - transform.position) * knockbackForce, ForceMode.Impulse);
        StartCoroutine(HitFlashing());
        if (hitPoints <= 0) {
            Stun();
            hitPoints = 3;
        }
    }

    public void Stun() {
        Debug.Log($"{name} stunned");
        stunnedTime = 10f;
        animator.SetTrigger("stun");
        movement.canMove = false;
        StartCoroutine(HitFlashing());
    }
    
    IEnumerator HitFlashing() {
        for (float i = 0; i < 3f; i += Time.deltaTime) {
            if (i % 0.2f < 0.1f) {
                movement.model.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
            else {
                movement.model.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            }
            yield return null;
        }
        movement.model.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
    }
}
