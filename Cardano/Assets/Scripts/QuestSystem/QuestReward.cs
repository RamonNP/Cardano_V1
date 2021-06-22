using UnityEngine;

namespace QuestSystem
{
    public abstract class QuestReward : MonoBehaviour
    {
        public abstract string Name { get ; protected set; }
        public float Quantity => quantity;
        
        [SerializeField] private float quantity;
        
        public abstract void GetReward();
    }
}