using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : Powerable {
    [SerializeField]
    FidgetSpinner fidgetSpinner;

    public override void PowerOn() {
        fidgetSpinner.Stun();
        Progress = 1f;
    }

    public override void PowerOff() {
        Progress = 0f;
    }
}
