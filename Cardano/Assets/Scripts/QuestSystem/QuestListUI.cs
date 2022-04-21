using System.Collections.Generic;
using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] private Canvas activeQuestCanvas;
        [SerializeField] private Transform content;
        [SerializeField] private Button modelButton;
        [SerializeField] private SelectedQuestUI selectedQuestUI;

        private readonly Dictionary<Quest, Button> activeQuests = new Dictionary<Quest, Button>();

        private void Awake()
        {
            SetUIWindowState(false);
            
            QuestInputManager.OnQuestAbandoned += RemoveQuest;
            PlayerQuests.OnNewQuestAdded += AddQuest;
            Quest.OnQuestCompleted += RemoveQuest;
            
            modelButton.gameObject.SetActive(false);
            if (selectedQuestUI == null)
                selectedQuestUI = FindObjectOfType<SelectedQuestUI>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                SetUIWindowState(!activeQuestCanvas.gameObject.activeSelf);
        }

        private void OnDestroy()
        {
            QuestInputManager.OnQuestAbandoned -= RemoveQuest;
            PlayerQuests.OnNewQuestAdded -= AddQuest;
            Quest.OnQuestCompleted -= RemoveQuest;
        }

        public void AddQuest(Quest addedQuest)
        {
            Button addedQuestButton = Instantiate(modelButton, content);
            
            TextMeshProUGUI addedQuestText = addedQuestButton.GetComponentInChildren<TextMeshProUGUI>();
            addedQuestText.SetText(addedQuest.QuestName);
            
            addedQuestButton.gameObject.SetActive(true);
            addedQuestButton.onClick.AddListener(() => ShowSelectedQuestWindow(addedQuest));
            
            activeQuests.Add(addedQuest, addedQuestButton);
        }

        public void RemoveQuest(Quest removedQuest)
        {
            if(!activeQuests.ContainsKey(removedQuest))
                return;
            
            Destroy(activeQuests[removedQuest].gameObject);
            activeQuests.Remove(removedQuest);
            selectedQuestUI.SetWindowVisibility(false);
        }

        private void SetUIWindowState(bool value)
        {
            if (!value)
                selectedQuestUI.SetWindowVisibility(false);
            
            activeQuestCanvas.gameObject.SetActive(value);
        }
        
        private void ShowSelectedQuestWindow(Quest selectedQuest)
        {
            selectedQuestUI.Bind(selectedQuest);
            selectedQuestUI.SetWindowVisibility(true);
        }
    }
}
