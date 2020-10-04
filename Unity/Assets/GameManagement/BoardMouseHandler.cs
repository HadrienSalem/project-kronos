using System;
using ProjectKronosUtils;
using UnityEngine;
using World.Board.Tiles.TilesScripts;


// SINGLETON
namespace GameManagement
{
    public class BoardMouseHandler : MonoBehaviour
    {

        public static BoardMouseHandler current;
        public const int BOARD_LAYER = 9;

        private Vector2Int lastHoveredTilePos = NOT_ON_BOARD;
        private static readonly Vector2Int NOT_ON_BOARD = new Vector2Int(-1, -1);

        private void Awake()
        {
            current = this;
        }

        private void Update()
        {
            MouseUtils.HandleHoverObject
            (
                targetLayer: BOARD_LAYER,
                actionIfHit: OnBoardHovered,
                actionIfNoHit: OnBoardNotHovered
            );

            MouseUtils.HandleClickObject
            (
                targetLayer: BOARD_LAYER,
                actionIfHit: OnBoardClicked,
                actionIfNoHit: OnNonBoardClicked
            );
        }

        private void OnBoardHovered(Collider2D hit)
        {
            var tile = hit.gameObject.GetComponent<TileManager>();
            if (tile == null) return;
            
            var tilePosition = tile.positionInBoard;
            TileExited(lastHoveredTilePos);
            TileHovered(tilePosition);
            lastHoveredTilePos = tilePosition;
        }

        private void OnBoardNotHovered(Collider2D hit)
        {
            TileExited(lastHoveredTilePos);
            lastHoveredTilePos = NOT_ON_BOARD;
        }

        private void OnBoardClicked(Collider2D hit)
        {
            var tile = hit.gameObject.GetComponent<TileManager>();
            if (tile == null) return;
            
            var tilePosition = tile.positionInBoard;
            TileClicked(tilePosition);
        }

        private void OnNonBoardClicked(Collider2D hit)
        {
            NonBoardClicked();
        }

        // EVENTS
        public event Action<Vector2Int> OnTileHoveredEvent;
        public void TileHovered(Vector2Int tilePosition)
        {
            OnTileHoveredEvent?.Invoke(tilePosition);
            //Debug.Log("Tile hovered at " + tilePosition);
        }

        public event Action<Vector2Int> OnTileExitedEvent;
        public void TileExited(Vector2Int tilePosition)
        {
            if(tilePosition != NOT_ON_BOARD)
            {
                OnTileExitedEvent?.Invoke(tilePosition);
            }
            //Debug.Log("Tile exited : " + tilePosition);
        }

        public event Action<Vector2Int> OnTileClickedEvent;
        public void TileClicked(Vector2Int tilePosition)
        {
            OnTileClickedEvent?.Invoke(tilePosition);
        }

        public event Action OnNonBoardClickedEvent;
        public void NonBoardClicked()
        {
            OnNonBoardClickedEvent?.Invoke();
        }
    }
}
