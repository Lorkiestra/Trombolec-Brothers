using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMeshCaster : MonoBehaviour {
	[SerializeField] private Transform playerRing;

	private void FixedUpdate() {
		Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f);
		playerRing.position = hit.collider ? hit.point : transform.position;
	}
}