using UnityEngine;
using CityThatForgets.Dialogue;

namespace CityThatForgets.Interaction
{
    public class NPCDialogueInteractable : MonoBehaviour, IInteractable
    {
        [Tooltip("Dialogue sequence id in the DialogueDatabase.")]
        public string dialogueId;

        [Tooltip("Button prompt text.")]
        public string prompt = "Talk";

        public void Interact()
        {
            DialogueController.Instance.StartDialogue(dialogueId);
        }

        public string GetPrompt() => prompt;
    }
}

