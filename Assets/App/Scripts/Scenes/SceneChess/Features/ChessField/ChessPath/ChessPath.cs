using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.ChessField.ChethPath
{
    public class ChessPath
    {
        public List<Vector2Int> Path { get; set; }
        public int[] MoveDirection { get; set; }
        public ChessPath(Vector2Int start, int[] dir = null)
        {
            Path = new List<Vector2Int>();
            AdPoint(start);

            MoveDirection = dir;
        }
        public ChessPath(ChessPath chessPath, Vector2Int move,int[] dir=null)
        {
            Path =new List<Vector2Int>(chessPath.Path);
            AdPoint(move);
            MoveDirection = dir;
        }
        public void AdPoint(Vector2Int point)
        {
            Path.Add(point);
        }
        public Vector2Int GetLast()
        {
            return Path[Path.Count - 1];
        }
        
    }
}

