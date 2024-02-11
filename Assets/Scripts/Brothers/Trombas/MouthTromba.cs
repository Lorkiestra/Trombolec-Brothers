using UnityEngine;
using Random = UnityEngine.Random;

public class MouthTromba : Tromba {
    
    [SerializeField] private Prop heldObject;
    
    [SerializeField] private float suctionForce = 200f;
    [SerializeField] private float throwForce = 200f;
    [SerializeField] private float brotherThrowerAdditionalForce = 50f;
    [SerializeField] private float suctionTerminalVelocity = 10f;
    [SerializeField] private float suctionHoldDistance = 0.7f;

    [SerializeField] private float suctionDistortionPower = 0.5f;
    [SerializeField] private float holdDistortionSpeed = 8.3f;
    [SerializeField] private float pushDistortionSpeed = 5.5f;
    [SerializeField] private float holdDistortionAcceleration = 24.3f;
    [SerializeField] private float holdDistortionMaxPower = 2.2f;
    [SerializeField] private float pushDistortionPower = -8f;

    protected override void Update()
    {
        base.Update();
        if (!heldObject)
            return;
        
        // update every material within object
        // FIXME duplicated code
        TrombaInjector[] distorts = heldObject.GetComponentsInChildren<TrombaInjector>();
        foreach (TrombaInjector d in distorts)
        {
            d.succPower1 = Mathf.Min(holdDistortionMaxPower,
                d.succPower1 + holdDistortionAcceleration * Time.deltaTime);
            d.succSpeed1 += holdDistortionSpeed * Time.deltaTime;
        }
    }

    public override void TromboneStart()
    {
        base.TromboneStart();
        if (!CanAttack)
            return;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public override void TromboneHold()
    {
        if (!CanAttack)
            return;
        if (heldObject)
            return;

        trombaModel.localScale = Vector3.one * Random.Range(1f, trombaModelMaxSize);
        SuckObjects();
    }

    public override void TromboneRelease() {
        base.TromboneRelease();
        if (!CanAttack)
            return;
        audioSource.Stop();
        trombaModel.localScale = Vector3.one;
        RestoreGravity();
        
        if (!heldObject)
            return;

        ThrowObject();
    }

    private void ThrowObject() {
        //update every material within objects
        // FIXME duplicated code
        TrombaInjector[] distorts = heldObject.GetComponentsInChildren<TrombaInjector>();
        foreach (TrombaInjector d in distorts)
        {
            d.succPower1 += pushDistortionPower;
            d.succSpeed1 = pushDistortionSpeed;
        }

        heldObject.transform.parent = null;
        heldObject.rb.isKinematic = false;
        if (heldObject.throwable)
            heldObject.rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        Movement brotherMovement = heldObject.GetComponent<Movement>();
        if (brotherMovement) {
            brotherMovement.canMove = true;
            brotherMovement.transform.rotation = Quaternion.identity;
            heldObject.rb.AddForce(transform.forward * throwForce * brotherThrowerAdditionalForce, ForceMode.Impulse);
        }
        heldObject = null;
    }

    private void SuckObjects() {
        foreach (Prop prop in propCollector.Props) {
            //update every material within objects
            // FIXME duplicated code
            TrombaInjector[] distorts = prop.GetComponentsInChildren<TrombaInjector>();
            foreach (TrombaInjector d in distorts)
            {
                d.succPower1 += suctionDistortionPower;
                d.succSpeed1 = pushDistortionSpeed;
            }
            prop.rb.useGravity = false;
            prop.rb.AddForce(Vector3.Normalize(propCollector.transform.position - prop.transform.position) * suctionForce);
            prop.rb.velocity = Vector3.ClampMagnitude(prop.rb.velocity, suctionTerminalVelocity);
            if (Vector3.Distance(prop.transform.position, propCollector.transform.position) >= suctionHoldDistance)
                continue;
            
            HoldObject(prop);
        }
    }

    private void RestoreGravity() {
        foreach (Prop prop in propCollector.Props) {
            prop.rb.useGravity = true;
        }
    }

    private void HoldObject(Prop prop) {
        heldObject = prop;
        heldObject.transform.parent = propCollector.transform;
        heldObject.rb.isKinematic = true;
        Movement brotherMovement = heldObject.GetComponent<Movement>();
        if (brotherMovement)
            brotherMovement.canMove = false;
        RestoreGravity();
        trombaModel.localScale = Vector3.one;
    }
}
