using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CityThatForgets.Interaction
{
    public interface IInteractable
    {
        void Interact();
        string GetPrompt();
    }

    public class PlayerInteractor : MonoBehaviour
    {
        public float radius = 1.1f;
        public LayerMask interactableLayer;

        private readonly Collider2D[] _results = new Collider2D[8];
        private IInteractable _current;

        private InputSystem_Actions _actions;

        private void Awake()
        {
            _actions = new InputSystem_Actions();
            _actions.Player.Interact.performed += _ => TryInteract();
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
            Scan();
        }

        private void Scan()
        {
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, _results, interactableLayer);

            IInteractable found = null;
            float bestDist = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var col = _results[i];
                if (col == null) continue;

                var interactable = col.GetComponent<IInteractable>();
                if (interactable == null) continue;

                float d = Vector2.SqrMagnitude(col.transform.position - transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    found = interactable;
                }
            }

            _current = found;
            // A real game would update a prompt UI here with _current?.GetPrompt()
        }

        private void TryInteract()
        {
            _current?.Interact();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}

