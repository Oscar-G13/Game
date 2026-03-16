using UnityEngine;

namespace CityThatForgets.World
{
    [RequireComponent(typeof(Collider2D))]
    public class RecordLineTerminal : MonoBehaviour
    {
        [Tooltip("Unique key for this Record Line, e.g. 'ledger_bridge_open'.")]
        public string recordKey;

        [Tooltip("Objects enabled when the record is TRUE.")]
        public GameObject[] trueObjects;

        [Tooltip("Objects enabled when the record is FALSE.")]
        public GameObject[] falseObjects;

        public void Toggle()
        {
            bool current = CityThatForgets.GameManager.Instance.IsRecordLineSet(recordKey);
            CityThatForgets.GameManager.Instance.SetRecordLine(recordKey, !current);
            Refresh();
            UI.AuditLog.Instance?.Add($"Record '{recordKey}' set to {!current}");
        }

        private void Refresh()
        {
            bool current = CityThatForgets.GameManager.Instance.IsRecordLineSet(recordKey);

            foreach (var go in trueObjects)
            {
                if (go != null) go.SetActive(current);
            }

            foreach (var go in falseObjects)
            {
                if (go != null) go.SetActive(!current);
            }
        }
    }
}

