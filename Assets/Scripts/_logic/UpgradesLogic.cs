using System;
using UnityEngine;

public class UpgradesLogic {
    public Action<int> OnFireRateUpgraded;
    public Action<int> OnDamageUpgraded;
    public Action<int> OnRangeUpgraded;

    public Upgrader FireRateLevelUpgrader { get; }
    public Upgrader DamageLevelUpgrader { get; }
    public Upgrader RangeLevelUpgrader { get; }

    public UpgradesLogic() {
        FireRateLevelUpgrader = new FireRateUpgrade(1);
        DamageLevelUpgrader = new DamageUpgrader(1);
        RangeLevelUpgrader = new RangeUpgrader(1);
    }

    private void UpgradeEntry(Upgrader upgrader, int resourceCount, Action<int> onUpgradedCallback) {
        if (!upgrader.CanUpgrade(resourceCount)) return;

        upgrader.SetUpgradeLevel(upgrader.UpgradeLevel + 1);
        
        //todo change this to use abstract resource not money
        ResourceManager.Shared.ConsumeMoney(upgrader.GetUpgradePrice());
        
        onUpgradedCallback?.Invoke(upgrader.UpgradeLevel);
    }

    public void UpgradeFireRate() {
        UpgradeEntry(FireRateLevelUpgrader, ResourceManager.Shared.MoneyCount, OnFireRateUpgraded);
    }

    public void UpgradeDamage() {
        UpgradeEntry(DamageLevelUpgrader, ResourceManager.Shared.MoneyCount, OnDamageUpgraded);
    }

    public void UpgradeRange() {
        UpgradeEntry(RangeLevelUpgrader, ResourceManager.Shared.MoneyCount, OnRangeUpgraded);
    }

    public bool CanUpgradeFireRate() {
        return FireRateLevelUpgrader.CanUpgrade(ResourceManager.Shared.MoneyCount);
    }

    public bool CanUpgradeDamage() {
        return DamageLevelUpgrader.CanUpgrade(ResourceManager.Shared.MoneyCount);
    }

    public bool CanUpgradeRange() {
        return RangeLevelUpgrader.CanUpgrade(ResourceManager.Shared.MoneyCount);
    }
}