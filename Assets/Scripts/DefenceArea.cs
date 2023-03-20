using System;
using DG.Tweening;
using UnityEngine;

public class DefenceArea : MonoBehaviour {
    public Action<Enemy> OnEnemySpotted;

    private Collider2D col;

    public void Reinit() {
        col = GetComponent<Collider2D>();
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            OnEnemySpotted?.Invoke(enemy);
        }
    }
}