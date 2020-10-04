using UnityEngine;

namespace World.Board.BoardScripts
{
    [CreateAssetMenu(fileName = "NewBoardData", menuName = "World/Board init data")]
    public class BoardInitData : ScriptableObject
    {

        public GameObject tilePrefab;
        public int width;
        public int height;

        public Vector2 firstTileCenter;
        public Vector2 distanceBetweenCenters;
        public Vector2 offset;
    
    }
}
