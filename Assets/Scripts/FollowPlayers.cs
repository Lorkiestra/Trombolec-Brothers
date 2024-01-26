using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayers : MonoBehaviour {
    [field: SerializeField]
    public Transform LeftGolec { get; private set; }

    [field: SerializeField]
    public Transform RightGolec { get; private set; }

    private void Update() {
        Vector3 middle = (LeftGolec.position + RightGolec.position) * 0.5f;
        transform.position = middle;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Vector3 middle = (LeftGolec.position + RightGolec.position) * 0.5f;
        Gizmos.DrawSphere(middle, 0.25f);
    }
}
