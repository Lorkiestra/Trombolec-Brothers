using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCollector : MonoBehaviour {
    public List<Prop> props;
    
    private void OnTriggerEnter(Collider other) {
        Prop prop = other.GetComponent<Prop>();
        if (!prop)
            return;
        
        if (!props.Contains(prop)) {
            props.Add(prop);
        }
    }

    private void OnTriggerExit(Collider other) {
        Prop prop = other.GetComponent<Prop>();
        if (!prop)
            return;
        
        prop.rb.useGravity = true;
        
        if (props.Contains(prop)) {
            props.Remove(prop);
        }
    }
}
