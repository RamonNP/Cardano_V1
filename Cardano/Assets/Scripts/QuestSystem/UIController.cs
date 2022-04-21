
using QuestSystem;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private StatsUI statsUI;
        [SerializeField] private NewQuestUI newQuestUI;
        [SerializeField] private QuestListUI questListUI;
        
        private void Awake()
        {
            if (statsUI != null)
                Player.OnStatsChanged += HandleStatsChanged;

            if (newQuestUI != null)
                NewQuest.OnQuestSelecting += HandleQuestSelecting;

            if (questListUI != null)
            {
                PlayerQuests.OnNewQuestAdded += HandleQuestAdded;
                QuestInputManager.OnQuestAbandoned += HandleAbandonButtonPressed;
            }
        }

        private void OnDestroy()
        {
            if (statsUI != null)
                Player.OnStatsChanged -= HandleStatsChanged;
            
            if (newQuestUI != null)
                NewQuest.OnQuestSelecting -= HandleQuestSelecting;
            
            if (questListUI != null)
            {
                PlayerQuests.OnNewQuestAdded -= HandleQuestAdded;
                QuestInputManager.OnQuestAbandoned -= HandleAbandonButtonPressed;
            }
        }
        
        private void HandleAbandonButtonPressed(Quest quest)
        {
            questListUI.RemoveQuest(quest);
        }
        
        private void HandleQuestAdded(Quest quest)
        {
            questListUI.AddQuest(quest);
        }
        
        private void HandleQuestSelecting(Quest quest)
        {
            newQuestUI.Bind(quest);
        }

        private void HandleStatsChanged(Player stats)
        {
            print("Comentado Implementar");
            //statsUI.Bind(stats);
        }
    }
}