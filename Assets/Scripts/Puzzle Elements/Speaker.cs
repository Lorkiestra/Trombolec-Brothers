using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : Powerable {
    public override void PowerOn() {
        Progress = 1f;
    }

    public override void PowerOff() {
        Progress = 0f;
    }
}
