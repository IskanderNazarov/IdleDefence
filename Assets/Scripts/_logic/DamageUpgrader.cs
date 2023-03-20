public class DamageUpgrader : Upgrader {
    private const int MAX_LEVEL = 6;
    
    public DamageUpgrader(int upgradeLevel) : base(upgradeLevel) {
    }

    public override bool IsMaxLevelReached => UpgradeLevel >= MAX_LEVEL;

    public override float GetValue() {
        return UpgradeLevel;
    }

    public override int GetUpgradePrice() {
        return UpgradeLevel * UpgradeLevel;
    }

    
}