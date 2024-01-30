using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour {
    [SerializeField]
    private FollowPlayers playersMidPoint;

    [SerializeField]
    private float zoomFactor = 10f, minZoom = 5f, maxZoom = 20f;

    [SerializeField, Range(0f, 1f)]
    float zoomOuterBound = .8f, zoomInnerBound = .2f;

    private CinemachineVirtualCamera virtualCamera;

    private CinemachineTransposer virtualCameraTransposer;

    private void Awake() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update() {
        HandleZoom();
    }

    private void HandleZoom() {
        float viewportDistance = GetViewportDistanceBetweenPoints(playersMidPoint.LeftGolec.position, playersMidPoint.RightGolec.position);
        float zoomSpeed = Time.deltaTime * zoomFactor * (1f / viewportDistance);
        float distanceToFollowPlayers = Vector3.Distance(playersMidPoint.transform.position, transform.position);

        if (viewportDistance > zoomOuterBound && distanceToFollowPlayers < maxZoom)
            virtualCameraTransposer.m_FollowOffset -= Quaternion.LookRotation(playersMidPoint.transform.position - transform.position) * Vector3.forward * zoomSpeed;
        else if (viewportDistance < zoomInnerBound && distanceToFollowPlayers > minZoom)
            virtualCameraTransposer.m_FollowOffset += Quaternion.LookRotation(playersMidPoint.transform.position - transform.position) * Vector3.forward * zoomSpeed;
    }

    private float GetViewportDistanceBetweenPoints(Vector3 pointA, Vector3 pointB) {
        Vector2 viewportPointA = Camera.main.WorldToViewportPoint(pointA);
        Vector2 viewportPointB = Camera.main.WorldToViewportPoint(pointB);
        return Vector2.Distance(viewportPointA, viewportPointB);
    }
}
