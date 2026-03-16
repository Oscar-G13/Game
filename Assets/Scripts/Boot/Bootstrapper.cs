using UnityEngine;
using UnityEngine.SceneManagement;

namespace CityThatForgets.Boot
{
    /// <summary>
    /// Lightweight entry point: place in an empty Boot scene,
    /// assign a GameManager prefab, and it will load the hub.
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        public GameObject gameManagerPrefab;

        private void Start()
        {
            if (CityThatForgets.GameManager.Instance == null && gameManagerPrefab != null)
            {
                Instantiate(gameManagerPrefab);
            }

            var hub = CityThatForgets.GameManager.Instance.hubSceneName;
            if (!string.IsNullOrEmpty(hub))
            {
                SceneManager.LoadScene(hub);
            }
        }
    }
}

