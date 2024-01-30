using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerable : MonoBehaviour {
    [field: SerializeField] public AnimationCurve Curve { get; private set; }

    public float Progress { get; protected set; } = 0f; 

    public abstract void PowerOn();
    
    public abstract void PowerOff();
}
