
using UnityEngine;

namespace QuestSystem.QuestRewards
{
    public class XpReward : QuestReward
    {
        //[SerializeField] private PlayerStats playerStats;
        
        public override string Name { get; protected set; } = "XP";

        public override void GetReward()
        {
            //if(playerStats != null)
                //playerStats.AddXp(Quantity);
                Debug.Log("IMPLEMENTAR GANHO DE XP COM QUEST");
        }
    }
}