using UnityEngine;
using UnityEngine.Events;

namespace Miscellaneous
{
    public class DoActionsOnTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask triggerMask;
        [SerializeField] private UnityEvent onTriggerEnter;
        [SerializeField] private UnityEvent onTriggerExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            onTriggerEnter.Invoke();
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            //if (!other.CompareLayer(triggerMask))
             //   return;
            
            onTriggerExit.Invoke();
        }
    }
}