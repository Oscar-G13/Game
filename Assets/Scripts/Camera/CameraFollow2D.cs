using UnityEngine;

namespace CityThatForgets.CameraSystem
{
    public class CameraFollow2D : MonoBehaviour
    {
        public Transform target;
        public float smoothTime = 0.15f;
        public Vector2 minBounds;
        public Vector2 maxBounds;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desired = new Vector3(target.position.x, target.position.y, transform.position.z);

            desired.x = Mathf.Clamp(desired.x, minBounds.x, maxBounds.x);
            desired.y = Mathf.Clamp(desired.y, minBounds.y, maxBounds.y);

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, smoothTime);
        }
    }
}

