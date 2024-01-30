using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCollector : MonoBehaviour {
    public List<Prop> Props {
        get {
            var nulls = props.Where(prop => prop == null).ToList();
            props.RemoveAll(prop => nulls.Contains(prop));
            return props;
        }

        set {
            props = value;
        }
    }
    
    private List<Prop> props = new();
    
    private void OnTriggerEnter(Collider other) {
        Prop prop = other.GetComponent<Prop>();
        if (!prop)
            return;
        
        if (!Props.Contains(prop)) {
            Props.Add(prop);
        }
    }

    private void OnTriggerExit(Collider other) {
        Prop prop = other.GetComponent<Prop>();
        if (!prop)
            return;
        
        prop.rb.useGravity = true;
        
        if (Props.Contains(prop)) {
            Props.Remove(prop);
        }
    }
}
