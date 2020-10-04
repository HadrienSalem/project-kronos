using System;
using System.Collections.Generic;
using Characters;
using GameManagement;
using ProjectKronosUtils;
using UnityEngine;
using World.Board.Tiles.TilesScripts;


// SINGLETON
namespace World.Board.BoardScripts
{
    public class BoardManager : MonoBehaviour
    {
        public static BoardManager current;

        public BoardInitData initData;
        public List<CharacterSpawnData> characterSpawnDataList;

        private TileManager[,] board;
        private List<GameObject> characters = new List<GameObject>();

        private BoardState state;
        private CharacterManager selectedCharacter;
        private readonly BoardManagerMouseEventHandler mouseEventHandler;
        private readonly BoardManagerCharacterEventHandler characterEventHandler;

        public BoardManager()
        {
            mouseEventHandler = new BoardManagerMouseEventHandler(this);
            characterEventHandler = new BoardManagerCharacterEventHandler(this);
        }
        
        private void Awake()
        {
            current = this;
            SetState(BoardState.INITIATING);
        }

        private void Start()
        {
            board = BoardGenerator.NewBoard(gameObject, initData);
            SpawnCharacters();

            SubscribeToAllEvents();

            SetState(BoardState.STANDBY);
        }

        private void Update()
        {
            if(state == BoardState.DESELECTING)
            {
                SetState(BoardState.STANDBY);
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromAllEvents();

            SetState(BoardState.TERMINATING);
        }

        private void SpawnCharacters()
        {
            foreach (var spawnData in characterSpawnDataList)
            {
                var position = new Vector2Int(spawnData.spawnPosition.x, spawnData.spawnPosition.y);

                var newCharacter = CharacterManager.NewCharacter(
                    character: spawnData.characterType,
                    positionOnBoard: position,
                    positionInScene: board[position.x, position.y].gameObject.transform.position
                );

                newCharacter.SetParentGameObject(GameObject.Find("Dynamic"));

                characters.Add(newCharacter);
            
            }
        }

        private bool IsCharacterOnBoard(CharacterManager character)
        {
            return characters.Exists(x => x == character.gameObject);
        }

        private void MoveSelectedCharacterToPosition(Vector2Int position)
        {
            selectedCharacter.MoveTo(position, board[position.x, position.y].gameObject.transform.position);
        }

        // EVENT HANDLING
        private void SubscribeToAllEvents()
        {
            mouseEventHandler.SubscribeToBoardMouseEvents();
            mouseEventHandler.SubscribeToCharacterMouseEvents();
            characterEventHandler.SubscribeToCharacterEvents();
        }
        private void UnsubscribeFromAllEvents()
        {
            mouseEventHandler.UnsubscribeFromBoardMouseEvents();
            mouseEventHandler.UnsubscribeFromCharacterMouseEvents();
            characterEventHandler.UnsubscribeFromCharacterEvents();
        }


        // BOARD STATE

        private void SetState(BoardState boardState)
        {
            state = boardState;
            Debug.Log("Board state set to " + state);
        }

        private enum BoardState
        {
            INITIATING,
            STANDBY,
            CHARACTER_CLICKED,
            DESELECTING,
            ACTION_OCCURING,
            TERMINATING
        }

        private class BoardManagerMouseEventHandler
    {
        private readonly BoardManager boardManager;

        public BoardManagerMouseEventHandler(BoardManager boardManager)
        {
            this.boardManager = boardManager;
        }

        internal void SubscribeToBoardMouseEvents()
        {
            BoardMouseHandler.current.OnTileHoveredEvent += OnTileHovered;
            BoardMouseHandler.current.OnTileExitedEvent += OnTileExited;
            BoardMouseHandler.current.OnTileClickedEvent += OnTileClicked;
            BoardMouseHandler.current.OnNonBoardClickedEvent += OnNonBoardClicked;
        }

        internal void UnsubscribeFromBoardMouseEvents()
        {
            BoardMouseHandler.current.OnTileHoveredEvent -= OnTileHovered;
            BoardMouseHandler.current.OnTileExitedEvent -= OnTileExited;
            BoardMouseHandler.current.OnTileClickedEvent -= OnTileClicked;
            BoardMouseHandler.current.OnNonBoardClickedEvent += OnNonBoardClicked;
        }

        private void OnTileHovered(Vector2Int position)
        {
            boardManager.board[position.x, position.y].tileUIState.isHovered = true;
        }

        private void OnTileExited(Vector2Int position)
        {
            boardManager.board[position.x, position.y].tileUIState.isHovered = false;
        }

        private void OnTileClicked(Vector2Int position)
        {
            if (boardManager.state != BoardManager.BoardState.CHARACTER_CLICKED) return;
            
            boardManager.state = BoardManager.BoardState.ACTION_OCCURING;
            boardManager.MoveSelectedCharacterToPosition(position);
        }

        private void OnNonBoardClicked()
        {
            if (boardManager.state != BoardManager.BoardState.CHARACTER_CLICKED) return;
            
            boardManager.SetState(BoardManager.BoardState.DESELECTING);
            Debug.Log(boardManager.selectedCharacter.gameObject.name + " unselected");
            boardManager.selectedCharacter = null;
        }

        internal void SubscribeToCharacterMouseEvents()
        {
            CharacterMouseHandler.current.OnCharacterClickedEvent += OnCharacterClicked;
        }

        internal void UnsubscribeFromCharacterMouseEvents()
        {
            CharacterMouseHandler.current.OnCharacterClickedEvent -= OnCharacterClicked;
        }

        private void OnCharacterClicked(CharacterManager character)
        {
            if (boardManager.state != BoardManager.BoardState.STANDBY ||
                !boardManager.IsCharacterOnBoard(character)) return;
            boardManager.SetState(BoardManager.BoardState.CHARACTER_CLICKED);
            boardManager.selectedCharacter = character;
            Debug.Log(character.gameObject.name + " selected");
        }
    }

        private class BoardManagerCharacterEventHandler
        {
            private readonly BoardManager boardManager;

            public BoardManagerCharacterEventHandler(BoardManager boardManager)
            {
                this.boardManager = boardManager;
            }

            internal void SubscribeToCharacterEvents()
            {
                CharacterEventHandler.current.OnCharacterFinishedMovingEvent += OnCharacterFinishedMoving;
            }

            internal void UnsubscribeFromCharacterEvents()
            {
                CharacterEventHandler.current.OnCharacterFinishedMovingEvent -= OnCharacterFinishedMoving;
            }

            private void OnCharacterFinishedMoving(CharacterManager character)
            {
                if (character != boardManager.selectedCharacter) return;

                boardManager.SetState(BoardManager.BoardState.STANDBY);
                boardManager.selectedCharacter = null;
            }
        }
    }
}
