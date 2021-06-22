using System;
using UnityEngine;

namespace QuestSystem
{
    public abstract class QuestGoal : MonoBehaviour
    {
        public static event Action OnQuestGoalProgress = delegate {  };
        public event Action<QuestGoal> OnQuestGoalCompleted = delegate {  };
        
        public float RemainingQuantity { get; protected set; }
        public float GoalQuantity => goalQuantity;
        public string Description => description;

        public bool QuestGoalEnabled { private get; set; }

        [SerializeField] protected float goalQuantity;
        [SerializeField] protected string description;

        protected virtual void Awake() => RegenerateGoal();

        public void RegenerateGoal() => RemainingQuantity = goalQuantity;

        protected virtual void SubtractRemaining(float value)
        {
            if(!QuestGoalEnabled)
                return;
            
            RemainingQuantity -= value;
            OnQuestGoalProgress?.Invoke();

            if (RemainingQuantity <= 0f)
                CompleteQuestGoal();
        }

        protected virtual void CompleteQuestGoal()
        {
            QuestGoalEnabled = false;
            OnQuestGoalCompleted?.Invoke(this);
        }
    }
}