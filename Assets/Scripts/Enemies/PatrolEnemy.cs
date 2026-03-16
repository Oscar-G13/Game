using UnityEngine;
using CityThatForgets.Player;

namespace CityThatForgets.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PatrolEnemy : MonoBehaviour
    {
        public Transform leftPoint;
        public Transform rightPoint;
        public float moveSpeed = 2.5f;
        public int contactDamage = 1;

        private Rigidbody2D _rb;
        private bool _movingRight = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            float dir = _movingRight ? 1f : -1f;
            _rb.velocity = new Vector2(dir * moveSpeed, _rb.velocity.y);

            if (_movingRight && transform.position.x > rightPoint.position.x)
            {
                Flip();
            }
            else if (!_movingRight && transform.position.x < leftPoint.position.x)
            {
                Flip();
            }
        }

        private void Flip()
        {
            _movingRight = !_movingRight;
            var s = transform.localScale;
            s.x *= -1f;
            transform.localScale = s;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Player")) return;

            var player = collision.collider.GetComponent<PlayerController2D>();
            if (player != null)
            {
                player.TakeDamage(contactDamage, transform.position);
            }
        }
    }
}

