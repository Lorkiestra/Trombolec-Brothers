using System.Collections;
using UnityEditor;
using UnityEngine;

public class Brother : MonoBehaviour {
    
    [SerializeField] public AudioClip jump;
    [SerializeField] public AudioClip fallToDeath;
    [SerializeField] public AudioSource audioSourceVoice;
    
    [SerializeField] protected int brotherType;
    
    // FIXME used by ClampDistance
    [SerializeField] private Brother otherBrother;
    
    [SerializeField] private Animator animator;
    
    [SerializeField] private int hitPoints = 3;
    [SerializeField] private float groundPoundForce = 100f;
    [SerializeField] private float groundPoundEnemyAffectRadius = 1f;
    [SerializeField] private float stunnedTime;
    [SerializeField] private float knockbackForce = 30f;
    [SerializeField] private float maxDistance = 20f;

    private Coroutine groundPounding;
    private Rigidbody rb;
    private Movement movement;
    
    private static readonly int GroundPoundTrigger = Animator.StringToHash("ground_pound");
    private static readonly int StunEndTrigger = Animator.StringToHash("stun_end");
    private static readonly int GroundPoundImpactTrigger = Animator.StringToHash("ground_pound_impact");
    private static readonly int StunTrigger = Animator.StringToHash("stun");
    private static readonly int WildDance = Animator.StringToHash("wild_dance");

    protected virtual void Awake() {
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        ClampDistance();
        
        // FIXME move to new method
        if (stunnedTime <= 0f)
            return;
            
        stunnedTime -= Time.deltaTime;
        if (stunnedTime > 0f)
            return;
        
        animator.SetTrigger(StunEndTrigger);
        movement.canMove = true;
        GameManager.Instance.DeadPlayers--;
        // FIXME move to new method
    }

    public void GroundPound() {
        if (movement.IsGrounded)
            return;

        groundPounding ??= StartCoroutine(GroundPoundCoroutine());
    }
    
    private IEnumerator GroundPoundCoroutine() {
        movement.canMove = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        animator.SetTrigger(GroundPoundTrigger);
        for (float waitTime = .5f; waitTime > 0f; waitTime -= Time.deltaTime)
            yield return null;
        
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(Vector3.down * 1000f, ForceMode.Impulse);
        while (!movement.IsGrounded)
            yield return null;
        
        animator.SetTrigger(GroundPoundImpactTrigger);
        foreach (Collider c in Physics.OverlapSphere(transform.position, 5f)) {
            // ground pound wave
            TrombaInjector[] distorts = c.GetComponentsInChildren<TrombaInjector>();
            foreach (var d in distorts)
            {
                // FIXME
                if (brotherType == 1)
                    d.poundDist1 = 0;
                else
                    d.poundDist2 = 0;
            }

            Prop prop = c.GetComponent<Prop>();
            if (prop)
                c.GetComponent<Prop>().rb.AddForce(Vector3.up * groundPoundForce + (c.transform.position - transform.position) * groundPoundForce / 7f, ForceMode.Impulse);
        }

        foreach (Collider c in Physics.OverlapSphere(transform.position, groundPoundEnemyAffectRadius)) {
            BasicEnemy enemy = c.GetComponent<BasicEnemy>();
            if (enemy)
                enemy.Die();
        }

        for (float waitTime = .5f; waitTime > 0f; waitTime -= Time.deltaTime)
            yield return null;
        
        movement.canMove = true;
        groundPounding = null;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Enemy")) {
            TakeHit(other.transform.position);
        }
    }

    private void TakeHit(Vector3 enemyPosition) {
        if (stunnedTime > 0f || groundPounding != null)
            return;
        
        hitPoints--;
        rb.AddForce(Vector3.Normalize(enemyPosition - transform.position) * knockbackForce, ForceMode.Impulse);
        StartCoroutine(HitFlashing());
        if (hitPoints <= 0) {
            Stun();
        }
    }

    public void Stun() {
        GameManager.Instance.DeadPlayers++;
        hitPoints = 3;
        stunnedTime = 3f;
        audioSourceVoice.PlayOneShot(fallToDeath);
        animator.SetTrigger(StunTrigger);
        movement.canMove = false;
        StartCoroutine(HitFlashing());
    }
    
    private IEnumerator HitFlashing() {
        for (float i = 0; i < 3f; i += Time.deltaTime)
        {
            movement.model.GetComponentInChildren<SkinnedMeshRenderer>().enabled = !(i % 0.2f < 0.1f);
            yield return null;
        }
        movement.model.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
    }

    // FIXME get rid or create own script
    private void ClampDistance()
    {
        Vector3 transformPosition = transform.position;
        float distance = Vector3.Distance(transformPosition, otherBrother.transform.position);

        if (distance > maxDistance) {
            Vector3 direction = otherBrother.transform.position - transformPosition;
            direction.Normalize();
            transform.position += direction * (distance - maxDistance);
        }
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, groundPoundEnemyAffectRadius);
    }

    public void FinishDance()
    {
        animator.SetTrigger(WildDance);
    }
}
