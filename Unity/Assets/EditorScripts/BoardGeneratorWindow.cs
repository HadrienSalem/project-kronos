using UnityEditor;
using UnityEngine;
using World.Board.BoardScripts;
using static ProjectKronosUtils.EditorUtils;

namespace EditorScripts
{
    public class BoardGeneratorWindow : EditorWindow
    {

        static BoardInitData boardData;

        // REMOVED FOR NOW BECAUSE IT WAS NOT USEFUL
        //[MenuItem("GameObject/Project Kronos Utils/Board generator")]
        public static void OpenWindow()
        {
            BoardGeneratorWindow window = (BoardGeneratorWindow) GetWindow(typeof(BoardGeneratorWindow));
            window.minSize = new Vector2(200, 100);
            window.Show();
        }

        private void Awake()
        {
            boardData = ScriptableObject.CreateInstance<BoardInitData>();
        }

        private void OnGUI()
        {
            DrawSettings();
        }

        private void DrawSettings()
        {
            boardData.tilePrefab = PrefabField("Tile prefab", boardData.tilePrefab);
            boardData.width = IntField("Width", boardData.width);
            boardData.height = IntField("Height", boardData.height);
        
            MakeButton("Create board", 40, GenerateBoard);
        }

        private void GenerateBoard()
        {
            //BoardGenerator.NewBoard(boardData);
        }
    }
}
