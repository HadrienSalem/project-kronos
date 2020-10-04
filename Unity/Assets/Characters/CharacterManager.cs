using System;
using GameManagement;
using UnityEngine;
using World.Board.BoardScripts;

namespace Characters
{
    public class CharacterManager : MonoBehaviour
    {
        private BoardManager parentBoard;
        private Animator animator;

        private Vector2Int positionOnBoard;
        public Vector2 destination;

        private CharacterState state;

        public const float CHARACTER_SPEED = 0.08f;

        private void Awake()
        {
            parentBoard = BoardManager.current;
            destination = gameObject.transform.position;
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        public void MoveTo(Vector2Int destinationOnBoard, Vector2 destinationInScene)
        {
            positionOnBoard = destinationOnBoard;

            destination = destinationInScene;
            SetState(CharacterState.MOVING);
        }

        private void HandleMovement()
        {
            switch (state)
            {
                case CharacterState.MOVING:
                    gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, destination, CHARACTER_SPEED);
                    if ((Vector2)gameObject.transform.position == destination)
                    {
                        SetState(CharacterState.DESTINATION_REACHED);
                    }
                    break;

                case CharacterState.DESTINATION_REACHED:
                    SetState(CharacterState.IDLE);
                    CharacterEventHandler.current.CharacterFinishedMoving(this);
                    break;
            
                case CharacterState.IDLE:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static GameObject NewCharacter (
            GameObject character,
            Vector2Int positionOnBoard,
            Vector2 positionInScene
        )
        {
            var instance = Instantiate(character);
            instance.transform.position = positionInScene;

            var manager = instance.AddComponent(typeof(CharacterManager)) as CharacterManager;
            manager.positionOnBoard = positionOnBoard;

            return instance;
        }

        private void SetState(CharacterState characterState)
        {
            state = characterState;
            Debug.Log("Character "+ gameObject.name +" state set to " + state);
        }

        private enum CharacterState
        {
            IDLE,
            MOVING,
            DESTINATION_REACHED
        }
    }
}
