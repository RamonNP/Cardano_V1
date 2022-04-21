using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectedQuestUI : MonoBehaviour
    {

        [SerializeField] private GameObject selectedQuestWindow;
        
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI questName;
        [SerializeField] private TextMeshProUGUI questDescription;
        [SerializeField] private TextMeshProUGUI questGoals;
        [SerializeField] private TextMeshProUGUI questRewards;
        [SerializeField] private Button abandonButton;
        
        private Quest activeQuest;

        private void Awake()
        {
            ClearUI();
            abandonButton.onClick.AddListener(HandleAbandonButton);
            QuestGoal.OnQuestGoalProgress += SetQuestGoals;
            Quest.OnQuestCompleted += HandleQuestCompleted;

        }
        
        private void OnDestroy()
        {
            abandonButton.onClick.RemoveListener(HandleAbandonButton);
            QuestGoal.OnQuestGoalProgress -= SetQuestGoals;
            Quest.OnQuestCompleted -= HandleQuestCompleted;
        }
        
        public void Bind(Quest quest)
        {
            activeQuest = quest;
            
            questName.SetText(activeQuest.QuestName);
            questDescription.SetText(activeQuest.QuestDescription);
            
            SetQuestGoals();
            SetQuestRewards();
        }

        public void SetWindowVisibility(bool value)
        {
            selectedQuestWindow.SetActive(value);
        }
        
        private void HandleAbandonButton()
        {
            QuestInputManager.AbandonQuest(activeQuest);
            ClearUI();
        }

        private void HandleQuestCompleted(Quest quest) => ClearUI();
        
        private void SetQuestGoals()
        {
            if (activeQuest == null)
                return;
            
            string goalsText = "";
            foreach (QuestGoal questEvent in activeQuest.QuestGoals)
            {
                float goal = questEvent.GoalQuantity;
                float achieved = goal - questEvent.RemainingQuantity;
                goalsText += $"{questEvent.Description} - {achieved}/{goal}\n";
            }

            questGoals.SetText(goalsText);
        }

        private void SetQuestRewards()
        {
            if (activeQuest == null)
                return;
            
            string rewardsText = "";
            foreach (QuestReward reward in activeQuest.QuestRewards)
                rewardsText += $"{reward.Name} x {reward.Quantity}\n";
            
            questRewards.SetText(rewardsText);
        }

        private void ClearUI()
        {
            Quest.OnQuestCompleted -= HandleQuestCompleted;
            activeQuest = null;
        }
    }
}