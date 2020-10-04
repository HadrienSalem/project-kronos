namespace World.Board.Tiles.TilesScripts
{
    public class TileUIState
    {
        public bool isHovered;
        public bool isSelected;

        public TileUIState(bool isHovered, bool isSelected)
        {
            this.isHovered = isHovered;
            this.isSelected = isSelected;
        }
    }
}
