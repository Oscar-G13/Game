using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CityThatForgets.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; }

        [Header("Data")]
        public DialogueDatabase database;

        [Header("UI")]
        public GameObject panel;
        public TMP_Text speakerLabel;
        public TMP_Text bodyLabel;
        public Button nextButton;

        private DialogueDatabase.Sequence _currentSeq;
        private int _index;
        private bool _active;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (nextButton != null)
            {
                nextButton.onClick.AddListener(Next);
            }

            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        public void StartDialogue(string id)
        {
            if (database == null)
            {
                Debug.LogWarning("DialogueController: no database assigned.");
                return;
            }

            _currentSeq = database.Get(id);
            if (_currentSeq == null || _currentSeq.lines.Count == 0)
            {
                Debug.LogWarning("DialogueController: sequence not found: " + id);
                return;
            }

            _index = 0;
            _active = true;

            if (panel != null) panel.SetActive(true);
            ShowLine();
        }

        private void ShowLine()
        {
            var line = _currentSeq.lines[_index];
            if (speakerLabel != null) speakerLabel.text = line.speaker;
            if (bodyLabel != null) bodyLabel.text = line.text;
        }

        public void Next()
        {
            if (!_active) return;

            _index++;
            if (_index >= _currentSeq.lines.Count)
            {
                End();
            }
            else
            {
                ShowLine();
            }
        }

        private void End()
        {
            _active = false;
            if (panel != null) panel.SetActive(false);
            CityThatForgets.GameManager.Instance.DialogueFinished(_currentSeq.id);
        }
    }
}

