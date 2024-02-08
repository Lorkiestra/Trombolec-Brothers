using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LaserManager : MonoBehaviour {
    
    enum LaserShowColorMode {
        HueRotate,
        ColorAlternate,
        ColorAlternateRandom,
    }

    enum LaserShowMode {
        AllEnabled,
        AlternateEveryOther,
        Wave
    }

    [SerializeField] private Transform laserTempalte;
    [SerializeField, Min(1)] private int numberOfLasers;
    [SerializeField, Min(0.1f)] private float laserSpacing = 0.5f;
    [SerializeField] private Vector3 laserIterativeRotationOffset;
    [SerializeField] private bool symmetryX, symmetryY, symmetryZ;
    [SerializeField, Space] private LaserShowColorMode laserShowColorMode;
    private LaserShowMode laserShowMode;
    [SerializeField] private float laserShowDuration = 6f;
    [SerializeField] private float hueRotateShowOffset = 1f;
    [SerializeField, Space] private float colorAlternateShowInterval = 1f;
    [SerializeField] private float laserShowModeInterval = 2f;
    [SerializeField] private float animationRadius = 1f;
    [SerializeField, Space] private Color laserNormalColor = Color.red;
    [SerializeField] private Color laserAltcolor = Color.white;
    
    private readonly List<Transform> lasers = new();
    private float hueRotateShowProgress = 0f, colorAlternateShowProgress = 0f, rotationProgress = 0f, laserShowModeProgress = 0f;
    private Color randomColor1 = Color.red, randomColor2 = Color.white;
    private Vector3 laserIterativeRotation;

    private void Awake() {
        laserShowColorMode = UnityEngine.Random.Range(0, 3) switch {
            0 => LaserShowColorMode.HueRotate,
            1 => LaserShowColorMode.ColorAlternate,
            2 => LaserShowColorMode.ColorAlternateRandom,
            _ => LaserShowColorMode.HueRotate
        };

        laserShowMode = UnityEngine.Random.Range(0, 3) switch {
            0 => LaserShowMode.AllEnabled,
            1 => LaserShowMode.AlternateEveryOther,
            2 => LaserShowMode.Wave,
            _ => LaserShowMode.AllEnabled
        };
    }

    private void Start() {
        laserTempalte.gameObject.SetActive(false);
        ClearChildren();

        for (int i = 0; i < numberOfLasers; i++) {
            Transform laser = Instantiate(laserTempalte, transform, transform);
            lasers.Add(laser);

            laser.gameObject.SetActive(true);

            laser.localPosition = new Vector3((i - (numberOfLasers / 2)) * laserSpacing, 0f, 0f);

            Vector3 laserRotation = Vector3.Lerp(-laserIterativeRotationOffset * numberOfLasers, laserIterativeRotationOffset * numberOfLasers, i / (float)numberOfLasers);

            float x = (symmetryX && i > numberOfLasers / 2) ? laserRotation.x : -laserRotation.x;
            float y = (symmetryY && i > numberOfLasers / 2) ? laserRotation.y : -laserRotation.y;
            float z = (symmetryZ && i > numberOfLasers / 2) ? laserRotation.z : -laserRotation.z;

            laser.localRotation = Quaternion.Euler(x, y, z);
        }
    }

    private void OnValidate() {
        if (!Application.isPlaying || lasers.Count < numberOfLasers) return;
        
        for (int i = 0; i < numberOfLasers; i++) {
            Transform laser = lasers[i];

            laser.localPosition = new Vector3((i - (numberOfLasers / 2)) * laserSpacing, 0f, 0f);

            Vector3 laserRotation = Vector3.Lerp(-laserIterativeRotationOffset * numberOfLasers, laserIterativeRotationOffset * numberOfLasers, i / (float)numberOfLasers);

            float x = (symmetryX && i > numberOfLasers / 2) ? laserRotation.x : -laserRotation.x;
            float y = (symmetryY && i > numberOfLasers / 2) ? laserRotation.y : -laserRotation.y;
            float z = (symmetryZ && i > numberOfLasers / 2) ? laserRotation.z : -laserRotation.z;

            laser.localRotation = Quaternion.Euler(x, y, z);
        }
    }

    private void Update() {
        if (laserShowColorMode == LaserShowColorMode.HueRotate)
            HueRotateShow();
        else if (laserShowColorMode == LaserShowColorMode.ColorAlternate)
            ColorAlternateShow();
        else if (laserShowColorMode == LaserShowColorMode.ColorAlternateRandom)
            ColorAlternateShow(random: true);

        HandleRotation();
        // HandleAlternateEveryOhterModeShow();
    }

    private void HandleAlternateEveryOhterModeShow() {

    }

    private void HandleRotation() {
        rotationProgress += Time.deltaTime / laserShowDuration;

        if (rotationProgress >= 1f)
            rotationProgress -= 1f;

        float sin = Mathf.Sin(rotationProgress * Mathf.PI * 2f);
        float cos = Mathf.Cos(rotationProgress * Mathf.PI * 2f);

        laserIterativeRotation = laserIterativeRotationOffset + (new Vector3(sin, 0f, cos) * animationRadius);

        for (int i = 0; i < numberOfLasers; i++) {
            Transform laser = lasers[i];

            laser.localPosition = new Vector3((i - (numberOfLasers / 2)) * laserSpacing, 0f, 0f);

            Vector3 laserRotation = Vector3.Lerp(-laserIterativeRotation * numberOfLasers, laserIterativeRotation * numberOfLasers, i / (float)numberOfLasers);

            float x = (symmetryX && i > numberOfLasers / 2) ? laserRotation.x : -laserRotation.x;
            float y = (symmetryY && i > numberOfLasers / 2) ? laserRotation.y : -laserRotation.y;
            float z = (symmetryZ && i > numberOfLasers / 2) ? laserRotation.z : -laserRotation.z;

            laser.localRotation = Quaternion.Euler(x, y, z);
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
            // Debug.Log(color);

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
        }
    }
}
