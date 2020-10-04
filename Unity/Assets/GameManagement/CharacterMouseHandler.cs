using System;
using Characters;
using ProjectKronosUtils;
using UnityEngine;


// SINGLETON
namespace GameManagement
{
    public class CharacterMouseHandler : MonoBehaviour
    {
        public static CharacterMouseHandler current;
        public const int CHARACTERS_LAYER = 8;

        private void Awake()
        {
            current = this;

        }

        private void Update()
        {
            MouseUtils.HandleClickObject
            (
                CHARACTERS_LAYER,
                OnCharacterClicked,
                delegate (Collider2D hit) { } // do nothing if character is not clicked
            );
        }

        private void OnCharacterClicked(Collider2D hit)
        {
            var character = hit.gameObject.GetComponent<CharacterManager>();
            if (character != null)
            {
                CharacterClicked(character);
            }
        }

        // EVENTS
        public event Action<CharacterManager> OnCharacterClickedEvent;
        public void CharacterClicked(CharacterManager characterManager)
        {
            OnCharacterClickedEvent?.Invoke(characterManager);
        }
    }
}
