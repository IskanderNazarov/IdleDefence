public class FireRateUpgrade : Upgrader {
    private const int MAX_LEVEL = 100;

    public override bool IsMaxLevelReached => UpgradeLevel >= MAX_LEVEL;
    
    public FireRateUpgrade(int upgradeLevel) : base(upgradeLevel) {
    }

    public override float GetValue() {
//note: just hardcoded values
        return 0.8f - UpgradeLevel * 0.1f;
    }

    public override int GetUpgradePrice() {
        return UpgradeLevel * 7;
    }
}