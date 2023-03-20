using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private Transform targetPosition;
    [SerializeField] private EnemiesPool pool;
    [SerializeField] private Camera cam;

    private LevelDescriptor levelDescriptor;
    private Action onAllEnemiesDestroyed;
    private int destroyedEnemiesCounter;

    public void StartSpawning(LevelDescriptor levelDescriptor, Action onAllEnemiesDestroyed) {
        this.levelDescriptor = levelDescriptor;
        this.onAllEnemiesDestroyed = onAllEnemiesDestroyed;
        destroyedEnemiesCounter = 0;
        StartCoroutine(SpawningCoroutine(levelDescriptor));
    }

    private IEnumerator SpawningCoroutine(LevelDescriptor levelDescriptor) {
        var i = 0;
        while (i < levelDescriptor.EnemiesCount) {
            var e = pool.GetPooledObject();
            if (e == null) {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            e.ResetInvocationListeners();
            e.OnEnemyDestroyed += OnEnemyDestroyed;

            e.Activate(levelDescriptor.EnemiesLevel);
            var x = Random.value * 2 - 1;
            var y = Random.value * 2 - 1;
            var h = cam.orthographicSize / 2;
            var w = h * cam.aspect;
            var pos = new Vector2(x, y).normalized * (h * 2);
            //if (Mathf.Abs(pos.x) > w / 2) pos.x = w *Mathf.Sign(pos.x);
            e.transform.position = pos;


            e.transform.DOMove(targetPosition.position, levelDescriptor.EnemySpeed).SetEase(Ease.Linear)
                .SetSpeedBased();
            var dir = Random.value > 0.5 ? 1 : -1;
            e.transform.DORotate(Vector3.forward * (180 * dir), 3, RotateMode.WorldAxisAdd).SetLoops(-1)
                .SetEase(Ease.Linear);


            i++;
            yield return new WaitForSeconds(levelDescriptor.EnemySpawnInterval);
        }
    }

    private void OnEnemyDestroyed(Enemy obj) {
        destroyedEnemiesCounter++;

        if (destroyedEnemiesCounter == levelDescriptor.EnemiesCount) {
            StopAllCoroutines();
            onAllEnemiesDestroyed?.Invoke();
        }
    }

    public void StopSpawning() {
        StopAllCoroutines();
    }
    
    public void ResetEnemies() {
        foreach (var enemy in pool.ObjectsPool) {
            enemy.gameObject.SetActive(false);
        }
    }
}