using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NewQuestUI : MonoBehaviour
    {
        [SerializeField] private GameObject newQuestWindowCanvas;
        
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI questName;
        [SerializeField] private TextMeshProUGUI questDescription;
        [SerializeField] private TextMeshProUGUI questGoals;
        [SerializeField] private TextMeshProUGUI questRewards;
        
        [Header("Buttons")]
        [SerializeField] private Button acceptButton;
        
        private Quest selectedQuest;
        
        private void Awake()
        {
            SetCanvasState(false);
            

            NewQuest.OnQuestSelecting += ShowCanvas;
            NewQuest.OnQuestUnselecting += HideCanvas;
            QuestGoal.OnQuestGoalProgress += SetQuestGoals;
            
            acceptButton.onClick.AddListener(AcceptQuest);
        }

        private void ShowCanvas(Quest quest) => SetCanvasState(true);
        private void HideCanvas() => SetCanvasState(false);

        private void OnDestroy()
        {
            NewQuest.OnQuestSelecting -= ShowCanvas;
            NewQuest.OnQuestUnselecting -= HideCanvas;
            QuestGoal.OnQuestGoalProgress -= SetQuestGoals;
        }
        
        public void Bind(Quest quest)
        {
            if (quest == null)
                return;
            
            selectedQuest = quest;
            questName.SetText(selectedQuest.QuestName);
            questDescription.SetText(selectedQuest.QuestDescription);
            
            SetQuestGoals();
            SetQuestRewards();
        }
        
        private void SetQuestGoals()
        {
            string goalsText = "";
            foreach (QuestGoal questGoal in selectedQuest.QuestGoals)
            {
                float goal = questGoal.GoalQuantity;
                float achieved = goal - questGoal.RemainingQuantity;
                goalsText += $"{questGoal.Description} - {achieved}/{goal}\n";
            }
            questGoals.SetText(goalsText);
        }

        private void SetQuestRewards()
        {
            string rewardsText = "";
            foreach (QuestReward reward in selectedQuest.QuestRewards)
                rewardsText += $"{reward.Name} x {reward.Quantity}\n";
            
            questRewards.SetText(rewardsText);
        }

        private void SetCanvasState(bool state)
        {
            print("setando canvas state "+state);
            newQuestWindowCanvas.gameObject.SetActive(state);
        }

        private void AcceptQuest()
        {
            print("Aceitou Missão");
            if (selectedQuest == null)
                return;
            
            SetCanvasState(false);
            QuestInputManager.AcceptQuest(selectedQuest);
        }
    }
}