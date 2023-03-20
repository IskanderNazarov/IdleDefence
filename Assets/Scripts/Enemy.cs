using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour {
    public Action<Enemy> OnEnemyDestroyed;

    public int Level { get; private set; }
    private int healthPoints;
    private float defaultScale;

    private bool isInit;

    private void Init() {
        isInit = true;
        defaultScale = transform.localScale.x;
    }

    public int ValueAfterDeath() {
        return Level;
    }

    public void Activate(int enemyLevel) {
        if (!isInit) Init();

        this.Level = enemyLevel;
        healthPoints = Level;

        //reset scale
        var scaleFactor = 1 + enemyLevel * 0.4f; //temp hardcoded value
        transform.localScale = Vector3.one * (scaleFactor * defaultScale);

        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var bullet = other.GetComponent<Bullet>();
        if (bullet != null) {
            bullet.gameObject.SetActive(false);

            healthPoints = Mathf.Clamp(healthPoints - bullet.Damage, 0, int.MaxValue);
            if (healthPoints != 0) return;

            gameObject.SetActive(false);
            transform.DOKill();
            transform.position = Vector3.one * 100; //somewhere very far away

            OnEnemyDestroyed?.Invoke(this);

            //remove all the listeners after call back
            ResetInvocationListeners();
        }
    }

    public void ResetInvocationListeners() {
        if (OnEnemyDestroyed != null) {
            foreach (var d in OnEnemyDestroyed.GetInvocationList()) {
                OnEnemyDestroyed -= (Action<Enemy>) d;
            }
        }
    }
}