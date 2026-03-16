using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CityThatForgets
{
    /// <summary>
    /// Global game state: scenes, identities, flags, shards.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Scenes")]
        [Tooltip("Name of the hub scene (Transit Spine).")]
        public string hubSceneName = "TransitSpine";

        [Tooltip("Name of the first district scene (Ledger Ward).")]
        public string ledgerWardSceneName = "LedgerWard";

        [Header("Player")]
        public GameObject playerPrefab;

        private HashSet<string> _collectedShards = new HashSet<string>();
        private HashSet<string> _completedDialogues = new HashSet<string>();
        private HashSet<string> _recordLines = new HashSet<string>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Auto-spawn player if needed.
            if (playerPrefab == null) return;

            var existingPlayer = FindObjectOfType<Player.PlayerController2D>();
            if (existingPlayer == null)
            {
                var spawnPoint = GameObject.FindWithTag("PlayerSpawn");
                Vector3 spawnPos = spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;
                Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            }
        }

        #region Shards

        public bool HasShard(string shardId) => _collectedShards.Contains(shardId);

        public void CollectShard(string shardId)
        {
            if (_collectedShards.Add(shardId))
            {
                Debug.Log($"Collected shard: {shardId}");
            }
        }

        #endregion

        #region Dialogues

        public void DialogueFinished(string dialogueId)
        {
            _completedDialogues.Add(dialogueId);
            Debug.Log($"Dialogue finished: {dialogueId}");
        }

        public bool HasCompletedDialogue(string id) => _completedDialogues.Contains(id);

        #endregion

        #region Record Lines

        public bool IsRecordLineSet(string key) => _recordLines.Contains(key);

        public void SetRecordLine(string key, bool value)
        {
            if (value)
            {
                if (_recordLines.Add(key))
                {
                    Debug.Log($"Record line set: {key}=TRUE");
                }
            }
            else
            {
                if (_recordLines.Remove(key))
                {
                    Debug.Log($"Record line set: {key}=FALSE");
                }
            }
        }

        #endregion

        #region Scene Control

        public void GoToHub()
        {
            if (!string.IsNullOrEmpty(hubSceneName))
            {
                SceneManager.LoadScene(hubSceneName);
            }
        }

        public void GoToLedgerWard()
        {
            if (!string.IsNullOrEmpty(ledgerWardSceneName))
            {
                SceneManager.LoadScene(ledgerWardSceneName);
            }
        }

        public void PlayerDied()
        {
            var current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }

        #endregion
    }
}

