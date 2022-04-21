
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI xpText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private Slider xpSlider;

        public void Bind(Player stats)
        {
            print("Comentado Implementar");
            /*
            levelText.SetText(stats.Level.ToString());
            xpText.SetText($"{Mathf.RoundToInt(stats.Xp)} XP");
            xpSlider.value = stats.Xp / stats.NextLevelXp;
            goldText.SetText($"{stats.Gold} PO");
            */
        }
    }
}