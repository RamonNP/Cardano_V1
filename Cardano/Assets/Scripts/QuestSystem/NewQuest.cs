using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    public class NewQuest : MonoBehaviour
    {
        public static event Action<Quest> OnQuestSelecting = delegate {  };
        public static event Action OnQuestUnselecting = delegate {  };

        [Header("Parameters")]
        [SerializeField] private string questName;
        [SerializeField] private string questDescription;
        
        [Header("Events")]
        [SerializeField] private UnityEvent onQuestDisabled;
        [SerializeField] private UnityEvent onQuestEnabled;
        
        [SerializeField] private Quest newQuest;
        private bool questIsAvailable;

        private void OnEnable()
        {
            QuestInputManager.OnQuestAbandoned += RegenerateOnAbandon;
            GenerateQuest();
        }

        private void OnDisable()
        {
            QuestInputManager.OnQuestAbandoned -= RegenerateOnAbandon;
        }
        
        public void SelectNewQuest()
        {
            if(!questIsAvailable) return;
            OnQuestSelecting(newQuest);
            Debug.Log("SelectNewQuest"+newQuest.QuestName);
        }

        public void UnselectNewQuest()
        {
            Debug.Log("UnselectNewQuest");
            if(!questIsAvailable) return;
            OnQuestUnselecting();
        }
        
        private void GenerateQuest()
        {
            List<QuestGoal> goals = GetComponentsInChildren<QuestGoal>().ToList();
            List<QuestReward> rewards = GetComponentsInChildren<QuestReward>().ToList();
            
            newQuest = new Quest(questName, questDescription, goals, rewards);
            questIsAvailable = true;
            onQuestEnabled.Invoke();
            
            QuestInputManager.OnQuestAccepted += DisableQuest;
        }
        
        private void RegenerateOnAbandon(Quest quest)
        {
            bool isTheWrongQuest = quest != newQuest;
            bool questIsNotValid = isTheWrongQuest || newQuest == null;
            
            if (questIsNotValid) return;
            newQuest.DisableQuest();
            GenerateQuest();
        }

        private void DisableQuest(Quest quest)
        {
            if (quest != newQuest) return;
            
            QuestInputManager.OnQuestAccepted -= DisableQuest;
            questIsAvailable = false;
            onQuestDisabled.Invoke();
        }
    }
}