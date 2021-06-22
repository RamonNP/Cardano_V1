using System;
using UnityEngine;

namespace QuestSystem.QuestGoals
{
    public class KillEnemyQuest : QuestGoal
    {
        [SerializeField] private Monster anyEnemyWithType;
        
        private MonsterType enemyType;

        protected override void Awake()
        {
            base.Awake();
            Monster.OnEnemyDeath += HandleEnemyDeath;
            enemyType = anyEnemyWithType.GetMonsterType;
        }

        private void HandleEnemyDeath(Monster enemy)
        {
            if (enemy.GetMonsterType != enemyType)
                return;
            
            SubtractRemaining(1f);
        }
    }
}