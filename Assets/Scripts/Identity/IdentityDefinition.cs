using UnityEngine;

namespace CityThatForgets.Identity
{
    [CreateAssetMenu(menuName = "CityThatForgets/Identity", fileName = "IdentityDefinition")]
    public class IdentityDefinition : ScriptableObject
    {
        [Header("Presentation")]
        public string displayName;
        [TextArea] public string description;
        public Color tintColor = Color.white;

        [Header("Tags / Clearance")]
        public string[] clearanceTags;

        [Header("Movement Modifiers")]
        public float moveSpeedMultiplier = 1f;
        public float jumpForceMultiplier = 1f;
        public bool canDoubleJump;
        public bool canWallSlide;

        [Header("Perception Filters")]
        public string[] visibleLayers; // Design-time use: which conceptual layers this identity can perceive.
    }
}

