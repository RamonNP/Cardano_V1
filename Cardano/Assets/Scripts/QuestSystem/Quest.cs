using System;
using System.Collections.Generic;

namespace QuestSystem
{
    public class Quest
    {
        public static event Action<Quest> OnQuestCompleted = delegate { };

        public string QuestName { get; }
        public string QuestDescription { get; }
        
        public List<QuestGoal> QuestGoals { get; }
        public List<QuestReward> QuestRewards { get; }

        private int questGoalsUncompleted;

        public Quest(string questName, string questDescription, List<QuestGoal> questGoals, List<QuestReward> questRewards)
        {
            QuestName = questName;
            QuestDescription = questDescription;
            QuestGoals = questGoals;
            QuestRewards = questRewards;
            
            questGoalsUncompleted = QuestGoals.Count;
        }

        public void EnableQuest()
        {
            foreach (QuestGoal questGoal in QuestGoals)
            {
                questGoal.QuestGoalEnabled = true;
                questGoal.OnQuestGoalCompleted += RemoveQuestGoal;
            }
        }
        
        public void DisableQuest()
        {
            foreach (QuestGoal questGoal in QuestGoals)
            {
                questGoal.QuestGoalEnabled = false;
                questGoal.OnQuestGoalCompleted -= RemoveQuestGoal;
                questGoal.RegenerateGoal();
            }
        }
        
        private void ReceiveRewards()
        {
            foreach (QuestReward reward in QuestRewards)
                reward.GetReward();
        }
        
        private void RemoveQuestGoal(QuestGoal questGoal)
        {
            questGoal.OnQuestGoalCompleted -= RemoveQuestGoal;
            questGoalsUncompleted--;

            if (!(questGoalsUncompleted <= 0))
                return;
            
            CompleteQuest();
        }

        private void CompleteQuest()
        {
            OnQuestCompleted?.Invoke(this);
            ReceiveRewards();
            DisableQuest();
        }
    }
}