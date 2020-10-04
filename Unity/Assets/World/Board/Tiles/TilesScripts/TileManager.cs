using UnityEngine;
using World.Board.BoardScripts;

namespace World.Board.Tiles.TilesScripts
{
    public class TileManager : MonoBehaviour
    {

        public BoardManager parentBoard;
        public Vector2Int positionInBoard; 

        private SpriteRenderer spriteRenderer;

        public TileUIState tileUIState = new TileUIState(
            isHovered: false,
            isSelected: false
        );

        private void Awake()
        {
            parentBoard = BoardManager.current;
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {

            if (tileUIState.isHovered)
            {
                spriteRenderer.color = new Color(0,0,0,1);
            }
            else spriteRenderer.color = new Color(1,1,1,1);

        }

    }
}
