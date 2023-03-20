using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI moneyText;
    private void Start() {
        ResourceManager.Shared.OnMoneyCountChanged += delegate { UpdateValues(); }; 
        
        UpdateValues();
    }

    private void UpdateValues() {
        moneyText.text = $"${ResourceManager.Shared.MoneyCount}";
    }
}