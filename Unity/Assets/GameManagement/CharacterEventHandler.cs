using System;
using Characters;
using UnityEngine;

// SINGLETON
namespace GameManagement
{
    public class CharacterEventHandler : MonoBehaviour
    {
        public static CharacterEventHandler current;

        private void Awake()
        {
            current = this;
        }

        public event Action<CharacterManager> OnCharacterFinishedMovingEvent;
        public void CharacterFinishedMoving(CharacterManager character)
        {
            OnCharacterFinishedMovingEvent?.Invoke(character);
        }
    }
}
