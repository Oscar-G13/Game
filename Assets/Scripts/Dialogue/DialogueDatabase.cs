using System.Collections.Generic;
using UnityEngine;

namespace CityThatForgets.Dialogue
{
    [CreateAssetMenu(menuName = "CityThatForgets/Dialogue Database", fileName = "DialogueDatabase")]
    public class DialogueDatabase : ScriptableObject
    {
        [System.Serializable]
        public class Line
        {
            public string speaker;
            [TextArea] public string text;
        }

        [System.Serializable]
        public class Sequence
        {
            public string id;
            public List<Line> lines = new();
        }

        public List<Sequence> sequences = new();

        private Dictionary<string, Sequence> _lookup;

        public Sequence Get(string id)
        {
            if (_lookup == null)
            {
                _lookup = new Dictionary<string, Sequence>();
                foreach (var seq in sequences)
                {
                    if (string.IsNullOrEmpty(seq.id)) continue;
                    if (_lookup.ContainsKey(seq.id)) continue;
                    _lookup.Add(seq.id, seq);
                }
            }

            _lookup.TryGetValue(id, out var result);
            return result;
        }
    }
}

