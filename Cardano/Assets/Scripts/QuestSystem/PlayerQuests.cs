using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class PlayerQuests : MonoBehaviour
    {
        public static event Action<Quest> OnNewQuestAdded = delegate {  };
        private readonly List<Quest> activeQuests = new List<Quest>();

        private void OnEnable()
        {
            QuestInputManager.OnQuestAccepted += AddQuest;
            QuestInputManager.OnQuestAbandoned += RemoveQuest;
        }

        private void OnDisable()
        {
            QuestInputManager.OnQuestAccepted -= AddQuest;
            QuestInputManager.OnQuestAbandoned -= RemoveQuest;
        }

        private void AddQuest(Quest quest)
        {
            activeQuests.Add(quest);
            quest.EnableQuest();
            
            OnNewQuestAdded?.Invoke(quest);
        }

        private void RemoveQuest(Quest quest)
        {
            if(!activeQuests.Contains(quest))
                return;
            
            quest.DisableQuest();
            activeQuests.Remove(quest);
        }
    }
}