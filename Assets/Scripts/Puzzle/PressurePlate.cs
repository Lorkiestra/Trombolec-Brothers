using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {
	[SerializeField] private Powerable connectedPlatform;
	private void OnTriggerEnter(Collider other) {
		if (!other.GetComponent<Prop>())
			return;
		Debug.Log("power on");
		connectedPlatform.PowerOn();
	}
	
	private void OnTriggerExit(Collider other) {
		if (!other.GetComponent<Prop>())
			return;
		Debug.Log("power off");
		connectedPlatform.PowerOff();
	}
}
