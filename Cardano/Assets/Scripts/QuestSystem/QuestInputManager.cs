using System;

namespace QuestSystem
{
    public static class QuestInputManager
    {
        public static event Action<Quest> OnQuestAccepted = delegate { };
        public static event Action<Quest> OnQuestAbandoned = delegate { };

        public static void AcceptQuest(Quest quest)
        {
            OnQuestAccepted(quest);
        }

        public static void AbandonQuest(Quest quest)
        {
            OnQuestAbandoned(quest);
        }
    }
}