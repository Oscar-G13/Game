using UnityEngine;
using CityThatForgets.Player;

namespace CityThatForgets.Identity
{
    public class IdentityController : MonoBehaviour
    {
        [Header("Equipped Identities")]
        public IdentityDefinition faceA;
        public IdentityDefinition faceB;

        [Header("Runtime")]
        public IdentityDefinition current;

        private PlayerController2D _player;
        private InputSystem_Actions _actions;

        private bool _useFaceB;
        private bool _canDoubleJump;
        private int _extraJumpsRemaining;

        private void Awake()
        {
            _player = GetComponent<PlayerController2D>();
            _actions = new InputSystem_Actions();
            _actions.Player.Previous.performed += _ => SwitchIdentity(false);
            _actions.Player.Next.performed += _ => SwitchIdentity(true);
        }

        private void OnEnable()
        {
            _actions.Enable();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }

        private void Start()
        {
            EquipInitial();
        }

        private void EquipInitial()
        {
            if (faceA != null)
            {
                current = faceA;
            }
            else if (faceB != null)
            {
                current = faceB;
                _useFaceB = true;
            }

            ApplyToPlayer();
        }

        private void SwitchIdentity(bool toNext)
        {
            if (faceA == null && faceB == null) return;

            if (toNext)
            {
                _useFaceB = !_useFaceB;
            }
            else
            {
                _useFaceB = !_useFaceB;
            }

            var newIdentity = _useFaceB ? faceB : faceA;
            if (newIdentity == null)
            {
                newIdentity = _useFaceB ? faceA : faceB;
            }

            current = newIdentity;
            ApplyToPlayer();
        }

        private void ApplyToPlayer()
        {
            if (_player == null || current == null) return;

            _player.moveSpeed = 7f * current.moveSpeedMultiplier;
            _player.jumpForce = 14f * current.jumpForceMultiplier;

            _canDoubleJump = current.canDoubleJump;
            _extraJumpsRemaining = _canDoubleJump ? 1 : 0;

            // Tint sprite
            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = current.tintColor;
            }
        }
    }
}

