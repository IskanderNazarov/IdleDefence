using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour {
    [SerializeField] private Tower tower;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private UpgradePanel upgradePanel;
    [SerializeField] private SpriteRenderer redScreen;
    [SerializeField] private TextMeshProUGUI levelText;


    public Action OnGameFinished;

    private LevelDescriptor[] levelDescriptors;
    private int levelDescriptorIndex;
    private bool isInit;

    private void Init() {
        isInit = true;
        tower.OnTowerAttacked += OnTowerAttacked;
    }

    public void StartGame(LevelDescriptor[] descriptors, UpgradesLogic upgradesLogic) {
        if (!isInit) Init();
        
        levelText.gameObject.SetActive(false);
        redScreen.gameObject.SetActive(false);

        levelDescriptors = descriptors;
        StartCoroutine(StartLevel(levelDescriptorIndex));

        tower.ReinitTower(upgradesLogic);
        upgradePanel.ShowPanel(upgradesLogic);
    }

    private IEnumerator StartLevel(int descriptorIndex) {
        levelText.gameObject.SetActive(true);
        levelText.color = new Color(1, 1, 1, 1);
        levelText.text = "Level " + (descriptorIndex + 1);
        yield return new WaitForSeconds(2);
        yield return levelText.DOFade(0, 1).WaitForCompletion();
        levelText.gameObject.SetActive(false);
        
        enemySpawner.StartSpawning(levelDescriptors[descriptorIndex], delegate {

            levelDescriptorIndex++;
            StartCoroutine(StartLevel(levelDescriptorIndex));
        });
    }

    private void OnTowerAttacked() {
        redScreen.gameObject.SetActive(true);

        var c = redScreen.color;
        c.a = 0;
        redScreen.color = c;

        enemySpawner.StopSpawning();
        redScreen.DOFade(0.5f, 0.4f).SetLoops(8, LoopType.Yoyo).SetEase(Ease.Linear)
            .OnComplete(delegate {
                redScreen.gameObject.SetActive(false);
                enemySpawner.ResetEnemies();
                OnGameFinished?.Invoke();
            });
    }
}