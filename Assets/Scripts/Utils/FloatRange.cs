using UnityEngine;

[System.Serializable]
public struct FloatRange {
    public float min, max;

    public float RandomValueInRange {
        get => Random.Range(min, max);
    }
}
