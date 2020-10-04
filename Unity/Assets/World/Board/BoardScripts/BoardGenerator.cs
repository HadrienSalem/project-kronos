using ProjectKronosUtils;
using UnityEngine;
using World.Board.Tiles.TilesScripts;

namespace World.Board.BoardScripts
{
    public static class BoardGenerator
    {

        public static TileManager[,] NewBoard(GameObject parent, BoardInitData data)
        {
            var width = data.width;
            var height = data.height;
            var tile = data.tilePrefab;

            var firstTileCenter = data.firstTileCenter;
            var distanceBetweenCenters = data.distanceBetweenCenters;
            var offset = data.offset;

            var board = new TileManager[height, width];

            for(var i=0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {

                    var newTile = PlaceAndCreateTile(
                        tile,
                        firstTileCenter,
                        distanceBetweenCenters,
                        offset,
                        new Vector2Int(i, j)
                    );

                    newTile.SetParentGameObject(parent);                   
                
                    board[i,j] = newTile.GetComponent<TileManager>();
                    board[i,j].positionInBoard = (new Vector2Int(i,j));
                }
            }

            return board;
        }

        private static GameObject PlaceAndCreateTile(
            GameObject tile,
            Vector2 firstTileCenter, 
            Vector2 distanceBetweenCenters, 
            Vector2 offset,
            Vector2Int positionInBoard
        )
        {
            var position = firstTileCenter;
            position.x += distanceBetweenCenters.x * positionInBoard.y + offset.x * positionInBoard.x;
            position.y -= distanceBetweenCenters.y * positionInBoard.x - offset.y * positionInBoard.y;
                
            var instantiatedTile = Object.Instantiate(tile);
            instantiatedTile.transform.position = position;

            return instantiatedTile;
        }

    }
}
