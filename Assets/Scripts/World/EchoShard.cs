using UnityEngine;

namespace CityThatForgets.World
{
    [RequireComponent(typeof(Collider2D))]
    public class EchoShard : MonoBehaviour
    {
        public string shardId;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            CityThatForgets.GameManager.Instance.CollectShard(shardId);
            Destroy(gameObject);
        }
    }
}

