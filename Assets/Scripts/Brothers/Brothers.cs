using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] public float stunnedTime;
    [SerializeField] protected int brotherType;
    
    [SerializeField] float knockbackForce = 30f;

    [SerializeField] private Vector3 lastKnownValidLocation;

    public abstract void Trombone();

    public abstract void TromboneRelease();

    protected virtual void Awake() {
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        StartCoroutine(TrackLastKnownLocation());
    }

    public virtual void Update() {
        if (transform.position.y < -10f) {
            transform.position = lastKnownValidLocation;
            movement.rb.velocity = Vector3.zero;
            Stun();
        }
        
        if (stunnedTime > 0f) {
            stunnedTime -= Time.deltaTime;
            if (stunnedTime <= 0f) {
                animator.SetTrigger("stun_end");
                movement.canMove = true;
            }
        }
    }

    IEnumerator TrackLastKnownLocation() {
        while (true) {
            if (movement.grounded) {
                lastKnownValidLocation = transform.position;
            }
            yield return new WaitForSeconds(0.4f);
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
            // groundpoundWave
            TrombaInjector[] distorts = collider.GetComponentsInChildren<TrombaInjector>();
            for (int i = 0; i < distorts.Length; i++)
            {
                //is first brother
                if (brotherType == 1)
                {
                    distorts[i].poundDist1 = 0;
                }
                else
                {
                    distorts[i].poundDist2 = 0;
                }
            }

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
        if (other.transform.CompareTag("Enemy")) {
            TakeHit(other.transform.position);
        }
    }

    public void TakeHit(Vector3 enemyPosition) {
        if (stunnedTime > 0f)
            return;
        
        hitPoints--;
        rb.AddForce(Vector3.Normalize(enemyPosition - transform.position) * knockbackForce, ForceMode.Impulse);
        StartCoroutine(HitFlashing());
        if (hitPoints <= 0) {
            Stun();
        }
    }

    public void Stun() {
        hitPoints = 3;
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
