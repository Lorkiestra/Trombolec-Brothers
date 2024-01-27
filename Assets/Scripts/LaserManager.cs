using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LaserManager : MonoBehaviour {
    enum LaserShowMode {
        HueRotate,
        ColorAlternate,
        ColorAlternateRandom,
    }

    [SerializeField]
    private Transform laserTempalte;

    [SerializeField, Min(1)]
    private int numberOfLasers;

    [SerializeField, Min(0.1f)]
    private float laserSpacing = 0.5f;

    [SerializeField]
    private Vector3 laserIterativeRotation;

    [SerializeField, Space]
    private LaserShowMode laserShowMode;

    [SerializeField]
    private float laserShowDuration = 6f;

    [SerializeField]
    private float hueRotateShowOffset = 1f;

    [SerializeField, Space]
    private float colorAlternateShowInterval = 1f;

    [SerializeField, Space]
    private Color laserNormalColor = Color.red;

    [SerializeField]
    private Color laserAltcolor = Color.white;

    private List<Transform> lasers = new();

    private float hueRotateShowProgress = 0f, colorAlternateShowProgress = 0f;

    private Color randomColor1, randomColor2;

    private void Start() {
        laserTempalte.gameObject.SetActive(false);
        ClearChildren();

        for (int i = 0; i < numberOfLasers; i++) {
            Transform laser = Instantiate(laserTempalte, transform, transform);
            lasers.Add(laser);

            laser.gameObject.SetActive(true);

            laser.localPosition = new Vector3((i - (numberOfLasers / 2)) * laserSpacing, 0f, 0f);

            Vector3 laserRotation = Vector3.Lerp(-laserIterativeRotation * numberOfLasers, laserIterativeRotation * numberOfLasers, i / (float)numberOfLasers);
            laser.localRotation = Quaternion.Euler(laserRotation);
        }
    }

    private void OnValidate() {
        if (!Application.isPlaying) return;
        
        for (int i = 0; i < numberOfLasers; i++) {
            Transform laser = lasers[i];

            laser.localPosition = new Vector3((i - (numberOfLasers / 2)) * laserSpacing, 0f, 0f);

            Vector3 laserRotation = Vector3.Lerp(-laserIterativeRotation * numberOfLasers, laserIterativeRotation * numberOfLasers, i / (float)numberOfLasers);
            laser.localRotation = Quaternion.Euler(laserRotation);
        }
    }

    private void Update() {
        if (laserShowMode == LaserShowMode.HueRotate)
            HueRotateShow();
        else if (laserShowMode == LaserShowMode.ColorAlternate)
            ColorAlternateShow();
        else if (laserShowMode == LaserShowMode.ColorAlternateRandom) {
            ColorAlternateShow(true);
        }
    }

    private void ColorAlternateShow(bool random = false) {
        colorAlternateShowProgress += Time.deltaTime / colorAlternateShowInterval;
        
        if (colorAlternateShowProgress >= 1f) {
            if (random) {
                float randomHue1 = UnityEngine.Random.Range(0f, 1f);
                float randomHue2 = UnityEngine.Random.Range(0f, 1f);

                randomColor1 = Color.HSVToRGB(randomHue1, 1f, 1f);
                randomColor2 = Color.HSVToRGB(randomHue2, 1f, 1f);
            }

            colorAlternateShowProgress -= 1f;
        }

        Color color1 = random ? randomColor1 : laserNormalColor;
        Color color2 = random ? randomColor2 : laserAltcolor;

        for (int i = 0; i < numberOfLasers; i++) {
            Transform laser = lasers[i];

            if (i % 2 == 0)
                laser.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", colorAlternateShowProgress < 0.5f ? color1 : color2);
            else
                laser.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", colorAlternateShowProgress < 0.5f ? color2 : color1);
        }
    }

    private void HueRotateShow() {
        hueRotateShowProgress += Time.deltaTime / laserShowDuration;

        if (hueRotateShowProgress >= 1f)
            hueRotateShowProgress -= 1f;

        
        for (int i = 0; i < numberOfLasers; i++) {
            Transform laser = lasers[i];

            float t = hueRotateShowProgress + i * hueRotateShowOffset / numberOfLasers;
            t %= 1f;

            float hue = Mathf.Lerp(0f, 1f, t);
            float saturation = 1f;
            float value = 1f;

            Color color = Color.HSVToRGB(hue, saturation, value);
            Debug.Log(color);

            laser.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", color);
        }
    }

    private void ClearChildren() {
        foreach (Transform child in transform) {
            if (child == laserTempalte)
                continue;

            Destroy(child.gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        for (int i = 0; i < numberOfLasers; i++) {
            Vector3 position = transform.position + new Vector3((i - (numberOfLasers / 2)) * laserSpacing, 0f, 0f);

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(position, 0.1f);

            Gizmos.color = Color.red;
            Vector3 laserRotation = Vector3.Lerp(-laserIterativeRotation * numberOfLasers, laserIterativeRotation * numberOfLasers, i / (float)numberOfLasers);
            // Gizmos.DrawLine(position, position + laserRotation + Vector3.up);
        }
    }
}
