using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour {
    [SerializeField] private Animator animator;
    void Start()
    {
        animator.SetTrigger($"rave{Random.Range(1, 5)}");
    }
}
