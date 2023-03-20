public class RangeUpgrader : Upgrader {
    private const int MAX_LEVEL = 5;
    public override bool IsMaxLevelReached => UpgradeLevel >= MAX_LEVEL;

    public RangeUpgrader(int upgradeLevel) : base(upgradeLevel) {
    }

    public override float GetValue() {
        return 1 + UpgradeLevel * 0.1f;
    }

    public override int GetUpgradePrice() {
        return UpgradeLevel * 5;
    }
}