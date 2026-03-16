using UnityEngine;
using UnityEngine.InputSystem;

namespace CityThatForgets.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController2D : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 7f;
        public float jumpForce = 14f;
        public float coyoteTime = 0.1f;
        public float jumpBufferTime = 0.1f;
        public float dashSpeed = 16f;
        public float dashDuration = 0.15f;
        public float dashCooldown = 0.35f;

        [Header("Ground Check")]
        public Transform groundCheck;
        public float groundRadius = 0.15f;
        public LayerMask groundLayer;

        [Header("Health")]
        public int maxHealth = 3;
        public float invulnTime = 1f;
        public float knockbackForce = 8f;

        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private bool _jumpQueued;
        private bool _dashQueued;

        private float _coyoteCounter;
        private float _jumpBufferCounter;
        private bool _isFacingRight = true;

        private bool _isDashing;
        private float _dashTimer;
        private float _dashCooldownTimer;

        private bool _isInvulnerable;
        private float _invulnTimer;
        private int _currentHealth;

        private InputSystem_Actions _actions;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentHealth = maxHealth;

            _actions = new InputSystem_Actions();

            _actions.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _actions.Player.Move.canceled += _ => _moveInput = Vector2.zero;
            _actions.Player.Jump.performed += _ => _jumpQueued = true;
            _actions.Player.Sprint.performed += _ => _dashQueued = true;
        }

        private void OnEnable()
        {
            _actions.Enable();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }

        private void Update()
        {
            bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

            if (isGrounded)
                _coyoteCounter = coyoteTime;
            else
                _coyoteCounter -= Time.deltaTime;

            if (_jumpQueued)
            {
                _jumpBufferCounter = jumpBufferTime;
                _jumpQueued = false;
            }
            else
            {
                _jumpBufferCounter -= Time.deltaTime;
            }

            if (_jumpBufferCounter > 0f && _coyoteCounter > 0f && !_isDashing)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _jumpBufferCounter = 0f;
                _coyoteCounter = 0f;
            }

            // Variable jump height (keyboard + gamepad)
            bool holdingJump = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
            if (!holdingJump)
            {
                var gamepad = Gamepad.current;
                if (gamepad != null)
                    holdingJump |= gamepad.buttonSouth.isPressed;
            }

            if (!holdingJump && _rb.velocity.y > 0f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.6f);
            }

            _dashCooldownTimer -= Time.deltaTime;
            if (_dashQueued && !_isDashing && _dashCooldownTimer <= 0f)
            {
                StartDash();
            }
            _dashQueued = false;

            if (_isDashing)
            {
                _dashTimer -= Time.deltaTime;
                if (_dashTimer <= 0f)
                {
                    EndDash();
                }
            }

            if (_moveInput.x > 0.1f && !_isFacingRight) Flip();
            else if (_moveInput.x < -0.1f && _isFacingRight) Flip();

            if (_isInvulnerable)
            {
                _invulnTimer -= Time.deltaTime;
                if (_invulnTimer <= 0f)
                    _isInvulnerable = false;
            }
        }

        private void FixedUpdate()
        {
            if (_isDashing) return;

            float targetX = _moveInput.x * moveSpeed;
            _rb.velocity = new Vector2(targetX, _rb.velocity.y);
        }

        private void StartDash()
        {
            _isDashing = true;
            _dashTimer = dashDuration;
            _dashCooldownTimer = dashCooldown;

            float dir = _isFacingRight ? 1f : -1f;
            _rb.velocity = new Vector2(dir * dashSpeed, 0f);
            _rb.gravityScale = 0f;
        }

        private void EndDash()
        {
            _isDashing = false;
            _rb.gravityScale = 3f;
        }

        private void Flip()
        {
            _isFacingRight = !_isFacingRight;
            var scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }

        public void TakeDamage(int amount, Vector2 sourcePosition)
        {
            if (_isInvulnerable) return;

            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                CityThatForgets.GameManager.Instance.PlayerDied();
                return;
            }

            _isInvulnerable = true;
            _invulnTimer = invulnTime;

            Vector2 dir = ((Vector2)transform.position - sourcePosition).normalized;
            _rb.velocity = Vector2.zero;
            _rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}

