using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMeshCaster : MonoBehaviour {
	[SerializeField] private Transform playerRing;

	private void FixedUpdate() {
		Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 100f, LayerMask.GetMask("Terrain"));
		playerRing.position = hit.collider ? hit.point : transform.position;
	}
}