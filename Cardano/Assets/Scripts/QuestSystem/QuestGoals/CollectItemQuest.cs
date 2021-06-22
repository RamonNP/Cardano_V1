using System;
using UnityEngine;

namespace QuestSystem.QuestGoals
{
    public class CollectItemQuest : QuestGoal
    {
        [SerializeField] private Item anyItemWithType;
        
        private Type itemType;

        protected override void Awake()
        {
            base.Awake();
            itemType = anyItemWithType.GetType();
            Item.OnItemCollected += CheckForProgress;
        }

        private void CheckForProgress(Item item)
        {
            if (item.GetType() != itemType)
                return;
            
            SubtractRemaining(1f);
        }
    }
}