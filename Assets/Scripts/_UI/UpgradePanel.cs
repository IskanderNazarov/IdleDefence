using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour {
    [SerializeField] private Button fireRateBtn;
    [SerializeField] private Button damageBtn;
    [SerializeField] private Button rangeBtn;

    [SerializeField] private TextMeshProUGUI fireRatePrice;
    [SerializeField] private TextMeshProUGUI damagePrice;
    [SerializeField] private TextMeshProUGUI rangePrice;
    [Space(15)] [SerializeField] private TextMeshProUGUI fireRateLevel;
    [SerializeField] private TextMeshProUGUI damageLevel;
    [SerializeField] private TextMeshProUGUI rangeLevel;
    [Space(15)] [SerializeField] private TextMeshProUGUI fireRateUpgradeInfo;
    [SerializeField] private TextMeshProUGUI damageUpgradeInfo;
    [SerializeField] private TextMeshProUGUI rangeUpgradeInfo;

    private UpgradesLogic upgradesLogic;

    private void Start() {
        ResourceManager.Shared.OnMoneyCountChanged += delegate {
            var money = ResourceManager.Shared.MoneyCount;
            UpdateValues();
        };
    }

    public void ShowPanel(UpgradesLogic upgradesLogic) {
        this.upgradesLogic = upgradesLogic;

        UpdateValues();
    }

    private void UpdateValues() {
        fireRateBtn.interactable = upgradesLogic.CanUpgradeFireRate();
        damageBtn.interactable = upgradesLogic.CanUpgradeDamage();
        rangeBtn.interactable = upgradesLogic.CanUpgradeRange();

        var money = ResourceManager.Shared.MoneyCount;
        fireRatePrice.text = $"${money}/${upgradesLogic.FireRateLevelUpgrader.GetUpgradePrice()}";
        damagePrice.text = $"${money}/${upgradesLogic.DamageLevelUpgrader.GetUpgradePrice()}";
        rangePrice.text = $"${money}/${upgradesLogic.RangeLevelUpgrader.GetUpgradePrice()}";

        fireRatePrice.gameObject.SetActive(!upgradesLogic.FireRateLevelUpgrader.IsMaxLevelReached);
        damagePrice.gameObject.SetActive(!upgradesLogic.DamageLevelUpgrader.IsMaxLevelReached);
        rangePrice.gameObject.SetActive(!upgradesLogic.RangeLevelUpgrader.IsMaxLevelReached);

        fireRateLevel.text = upgradesLogic.FireRateLevelUpgrader.IsMaxLevelReached
            ? "Max level"
            : $"Level: {upgradesLogic.FireRateLevelUpgrader.UpgradeLevel + 1}";
        damageLevel.text = upgradesLogic.DamageLevelUpgrader.IsMaxLevelReached
            ? "Max level"
            : $"Level: {upgradesLogic.DamageLevelUpgrader.UpgradeLevel + 1}";
        rangeLevel.text = upgradesLogic.RangeLevelUpgrader.IsMaxLevelReached
            ? "Max level"
            : $"Level: {upgradesLogic.RangeLevelUpgrader.UpgradeLevel + 1}";

        /*fireRateUpgradeInfo.text = $"Level: {upgradesLogic.FireRateLevelUpgrader.UpgradeLevel + 1}";
        damageUpgradeInfo.text = $"Level: {upgradesLogic.DamageLevelUpgrader.UpgradeLevel + 1}";
        rangeUpgradeInfo.text = $"Level: {upgradesLogic.RangeLevelUpgrader.UpgradeLevel + 1}";*/
    }

    public void UpgradeFireRate() {
        if (upgradesLogic.CanUpgradeFireRate()) {
            upgradesLogic.UpgradeFireRate();
            UpdateValues();
        }
    }

    public void UpgradeDamage() {
        
        if (upgradesLogic.CanUpgradeDamage()) {
            upgradesLogic.UpgradeDamage();
            UpdateValues();
        }
    }

    public void UpgradeRange() {
        if (upgradesLogic.CanUpgradeRange()) {
            upgradesLogic.UpgradeRange();
            UpdateValues();
        }
    }
}