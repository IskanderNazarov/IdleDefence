public abstract class Upgrader {
    protected Upgrader(int upgradeLevel) {
        UpgradeLevel = upgradeLevel;
    }

    public int UpgradeLevel { get; private set; }


    public void SetUpgradeLevel(int level) {
        UpgradeLevel = level;
    }

    public bool CanUpgrade(int availableResource) {
        return availableResource >= GetUpgradePrice() && !IsMaxLevelReached;
    }
    
    public abstract bool IsMaxLevelReached { get; }
    public abstract float GetValue();
    public abstract int GetUpgradePrice();
}