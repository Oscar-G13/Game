using System.Collections.Generic;
using UnityEngine;

namespace CityThatForgets.UI
{
    public class AuditLog : MonoBehaviour
    {
        public static AuditLog Instance { get; private set; }

        [TextArea] public string preview;

        private readonly List<string> _entries = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Add(string line)
        {
            _entries.Add(line);
            if (_entries.Count > 10)
            {
                _entries.RemoveAt(0);
            }
            preview = string.Join("\n", _entries);
        }
    }
}

