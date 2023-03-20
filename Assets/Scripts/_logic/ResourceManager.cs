using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    private const string SAVE_MONEY_KEY = "resource_money_key";
    public static ResourceManager Shared { get; private set; }

    public Action<int> OnMoneyCountChanged;
    public int MoneyCount { get; private set; }

    private void Awake() {
        if (Shared == null) {
            Shared = this;
        } else if (Shared != this) {
            Destroy(this);
        }

        Init();
        DontDestroyOnLoad(Shared);
    }

    private void Init() {
        MoneyCount = PlayerPrefs.GetInt(SAVE_MONEY_KEY);
    }

    public void AddMoney(int money) {
        MoneyCount += money;

        OnMoneyCountChanged?.Invoke(MoneyCount);
        //SaveMoney();
    }

    public void ConsumeMoney(int consumeCount) {
        if (consumeCount > MoneyCount) return;

        MoneyCount -= consumeCount;
        OnMoneyCountChanged?.Invoke(MoneyCount);
        //SaveMoney();
    }

    private void SaveMoney() {
        OnMoneyCountChanged?.Invoke(MoneyCount);
        PlayerPrefs.SetInt(SAVE_MONEY_KEY, MoneyCount);
    }
}