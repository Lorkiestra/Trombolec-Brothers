using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMeshCaster : MonoBehaviour
{
	[SerializeField] GameObject shadowObject;

	private void Start() {
		shadowObject = Instantiate(shadowObject, transform.position, Quaternion.identity);
		shadowObject.SetActive(false);
	}

	private void Update() {
		Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f);
		if (hit.collider) {
			shadowObject.SetActive(true);
			shadowObject.transform.position = hit.point + Vector3.up * 0.1f;
			shadowObject.transform.rotation = Quaternion.LookRotation(Vector3.down, hit.normal);
		}
		else {
			shadowObject.SetActive(false);
		}
	}
}
