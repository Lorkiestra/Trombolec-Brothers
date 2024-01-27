using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightsRig : MonoBehaviour {
    [SerializeField]
    private Transform beamTemplate, beamConnectorTemplate;

    [SerializeField, Min(1)]
    private int height, width;

    private void Start() {
        beamTemplate.gameObject.SetActive(false);
        beamConnectorTemplate.gameObject.SetActive(false);
    }

    private void OnValidate() {
        if (!Application.isPlaying) return;
        ClearChildren();

        for (int x = 0; x < width; x++) {
            Transform beam = Instantiate(beamTemplate, transform);
            beam.gameObject.SetActive(true);
            beam.localPosition = new Vector3(x, 0, 0);

            for (int y = 1; y < height; y++) {
                Transform connector = Instantiate(beamConnectorTemplate, transform);
                connector.gameObject.SetActive(true);
                connector.localPosition = new Vector3(x, y, 0);
            }
        }
    }

    private void ClearChildren() {
        foreach (Transform child in transform) {
            if (child == beamTemplate || child == beamConnectorTemplate)
                continue;

            Destroy(child.gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        BoxCollider beamBoxCollider = beamTemplate.GetComponent<BoxCollider>();
        float beamWidth = beamBoxCollider.bounds.size.x;
        float beamHeight = beamBoxCollider.bounds.size.y;

        BoxCollider connectorBoxCollider = beamConnectorTemplate.GetComponent<BoxCollider>();
        float connectorWidth = connectorBoxCollider.bounds.size.x;
        float connectorHeight = connectorBoxCollider.bounds.size.y;

        float widthDifference = connectorWidth - beamWidth;

        float totalWidth = width * beamHeight + 2 * widthDifference;
        float totalHeight = height * beamHeight + connectorHeight;

        Vector3 leftLegPosition = transform.position + new Vector3(-totalWidth / 2f, 0f, 0f);
        Vector3 rightLegPosition = transform.position + new Vector3(totalWidth / 2f, 0f, 0f);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(leftLegPosition, 0.25f);
        Gizmos.DrawSphere(rightLegPosition, 0.25f);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(leftLegPosition + new Vector3(0f, totalHeight, 0f), 0.25f);
        Gizmos.DrawSphere(rightLegPosition + new Vector3(0f, totalHeight, 0f), 0.25f);
    }
}
