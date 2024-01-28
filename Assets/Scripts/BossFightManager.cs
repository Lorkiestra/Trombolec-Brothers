using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour {
    [SerializeField]
    FidgetSpinner fidgetSpinner;

    [SerializeField]
    List<Lever> levers;

    [SerializeField]
    List<Speaker> speakers;

    List<Lever> leversToActivate;

    private bool isStunned = false;

    private void Start() {
        leversToActivate = GetLeversToActivate();
    }

    private void Update() {
        if (leversToActivate.All(lever => lever.IsOn) && !isStunned) {
            fidgetSpinner.Stun();
            isStunned = true;

            StartCoroutine(SetTimeout(() => {
                    ResetLevers();
                    leversToActivate = GetLeversToActivate();
                    isStunned = false;
                }, fidgetSpinner.StunTime)
            );
        }

        if (fidgetSpinner.IsDead) {
            Debug.Log("You won!");
        }
    }

    public IEnumerator SetTimeout(Action callback, float time) {
        yield return new WaitForSeconds(time);
        callback();
    }

    private void ResetLevers() {
        foreach (Lever lever in levers) {
            lever.StaysActivated = true;
            lever.Powerable = null;
        }
    }

    private List<Lever> GetLeversToActivate() {
        List<Lever> levers = new();
        List<Lever> leversPool = new(this.levers);

        for (int i = 0; i < speakers.Count; i++) {
            Lever lever = leversPool[UnityEngine.Random.Range(0, leversPool.Count)];
            leversPool.Remove(lever);
            lever.StaysActivated = false;
            lever.Powerable = speakers[i];
            levers.Add(lever);
        }

        return levers;
    }
}