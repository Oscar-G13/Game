using UnityEngine;
using CityThatForgets.World;

namespace CityThatForgets.Interaction
{
    [RequireComponent(typeof(RecordLineTerminal))]
    public class RecordLineInteractable : MonoBehaviour, IInteractable
    {
        public string prompt = "Rewrite record";

        private RecordLineTerminal _terminal;

        private void Awake()
        {
            _terminal = GetComponent<RecordLineTerminal>();
        }

        public void Interact()
        {
            _terminal.Toggle();
        }

        public string GetPrompt() => prompt;
    }
}

