using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tower : MonoBehaviour {
    [SerializeField] private DefenceArea defenceArea;

    [SerializeField] private BulletsPool bulletsPool;
    //[SerializeField] private UpdateDescriptor intervalUpdateDescriptor;

    public Action OnTowerAttacked;

    private UpgradesLogic upgradesLogic;
    private HashSet<Enemy> spottedEnemies;
    private Coroutine enemiesObserver;
    private float defenceAreaDefaultScale;
    private bool isInit;

    private void Init() {
        defenceArea.OnEnemySpotted += OnEnemySpotted;
        spottedEnemies = new HashSet<Enemy>();
        defenceAreaDefaultScale = defenceArea.transform.localScale.x;
    }

    public void ReinitTower(UpgradesLogic upgradesLogic) {
        if (!isInit) Init();

        this.upgradesLogic = upgradesLogic;
        upgradesLogic.OnRangeUpgraded -= OnRangeUpgraded;
        upgradesLogic.OnRangeUpgraded += OnRangeUpgraded;

        defenceArea.transform.localScale = Vector3.one * defenceAreaDefaultScale;
        defenceArea.Reinit();
        GetComponent<Collider2D>().enabled = true;
        enemiesObserver = StartCoroutine(ObserveSpace());
    }

    private void OnRangeUpgraded(int upgradeLevel) {
        //todo resize defence area

        defenceArea.transform.localScale = Vector3.one * (defenceAreaDefaultScale * upgradesLogic.RangeLevelUpgrader.GetValue());
    }

    private void OnEnemySpotted(Enemy enemy) {
        enemy.OnEnemyDestroyed += OnEnemyDestroyed;
        spottedEnemies.Add(enemy);
    }

    private void OnEnemyDestroyed(Enemy enemy) {
        ResourceManager.Shared.AddMoney(enemy.Level);
        spottedEnemies.Remove(enemy);
    }

    private bool IsNoEnemyDetected() {
        return spottedEnemies.Count == 0;
    }

    private IEnumerator ObserveSpace() {
        while (true) {
            if (spottedEnemies.Count == 0) yield return new WaitWhile(IsNoEnemyDetected);

            var enemy = ObtainClosetEnemy();
            ShootToEnemy(enemy);
            //spottedEnemies.Remove(enemy);

            //read interval from update descriptor
            //yield return new WaitForSeconds(intervalUpdateDescriptor.GetUpdateData(0).updateValue);
            yield return new WaitForSeconds(upgradesLogic.FireRateLevelUpgrader.GetValue());
        }
    }

    private void ShootToEnemy(Enemy enemy) {
        var bullet = bulletsPool.GetPooledObject();
        bullet.transform.DOKill();
        bullet.transform.position = transform.position;
        bullet.Activate((int) upgradesLogic.DamageLevelUpgrader.GetValue());

        var dir = (enemy.transform.position - transform.position).normalized;
        var dest = bullet.transform.position + dir * 100;//float.MaxValue;
        //dest.z = 0;
        bullet.transform.DOMove(dest, 6).SetEase(Ease.Linear).SetSpeedBased();
    }

    private Enemy ObtainClosetEnemy() {
        var minMagnitude = float.MaxValue;
        Enemy closestEnemy = null;
        foreach (var e in spottedEnemies) {
            var m = (e.transform.position - transform.position).sqrMagnitude;
            if (m < minMagnitude) {
                minMagnitude = m;
                closestEnemy = e;
            }
        }

        return closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            GetComponent<Collider2D>().enabled = false;
            OnTowerAttacked?.Invoke();
            
            StopCoroutine(enemiesObserver);
        }
    }
}