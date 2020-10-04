using UnityEngine;

namespace World.Board.BoardScripts
{
    [CreateAssetMenu(fileName = "NewCharacterSpawnData", menuName = "World/Character spawn data")]
    public class CharacterSpawnData : ScriptableObject
    {
        public GameObject characterType;
        public Vector2Int spawnPosition;
    }
}
