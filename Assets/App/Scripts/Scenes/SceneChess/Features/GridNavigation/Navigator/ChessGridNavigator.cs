using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using App.Scripts.Scenes.SceneChess.Features.ChessField.ChethPath;
using UnityEngine;
namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            ChessUnit figure = grid.Get(from);
            if (grid.Get(to) != null) return null;
            char[][] gridMap = CreateCharGrid(from, to, grid);
            List < Vector2Int > chessPath = new List<Vector2Int>();
            switch (unit)
            {
                case ChessUnitType.Pon:
                    if(from.x!=to.x)return null;
                    chessPath = ShortWave(new int[,] { { 0, 1 },{ 0, -1 } }, gridMap, from) ;
                    break;
                case ChessUnitType.King:
                    chessPath = ShortWave(new int[,] { { 1, 0 }, { -1, 0 }, { 0, -1 }, { 0, 1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } }, gridMap, from);
                    break;
                case ChessUnitType.Queen:
                    chessPath = LongWave(new int[,] { { 1, 0 }, { -1, 0 }, { 0, -1 }, { 0, 1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } }, gridMap, from);
                    break;
                case ChessUnitType.Rook:
                    chessPath = LongWave(new int[,] { { 1, 0 }, { -1, 0 }, { 0, -1 }, { 0, 1 } }, gridMap, from);
                    break;
                case ChessUnitType.Knight:
                    chessPath = ShortWave(new int[,] { { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { -1, 2 }, { 1, -2 }, { -1, -2 } }, gridMap, from);
                    break;
                case ChessUnitType.Bishop:
                    if ((from.x + from.y) % 2 != (to.x + to.y) % 2) return null;
                    chessPath = LongWave(new int[,] { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } }, gridMap, from);
                    break;
            }

            if (chessPath.Count == 0) return null;
            else 
            { 
                chessPath.RemoveAt(0);
                return chessPath;
            } 
            
        }
        public List<Vector2Int> ShortWave(int[,] wavedir, char[][] gridMap, Vector2Int from)
        {
            int N = gridMap.Length, M = gridMap[0].Length;
            Queue<ChessPath> ways = new Queue<ChessPath>() { };
            ways.Enqueue(new ChessPath(from));
            Queue<ChessPath> waysTemp = new Queue<ChessPath>() { };
            Vector2Int lastpos;
            while (true)
            {
                foreach (var way in ways)
                {
                    lastpos = way.GetLast();
                    for(int i = 0; i< wavedir.Length/2; i++) 
                    {
                        int x =lastpos.x + wavedir[i, 0], 
                            y= lastpos.y + wavedir[i, 1];
                        if (x >= 0 && x < N && y >= 0 && y < M)
                        {
                            if (gridMap[x][y]== 'e')
                            {
                                way.AdPoint(new Vector2Int(x,y));
                                return way.Path;
                            }
                            else if(gridMap[x][y] == '#')
                            {
                                waysTemp.Enqueue(new ChessPath(way, new Vector2Int(x, y)));
                                gridMap[x][y] = 'X';
                            }
                        }
                    }
                }
                
                if (waysTemp.Count == 0) return null;
                ways.Clear();
                ways =new  Queue<ChessPath>(waysTemp);
                waysTemp.Clear();
            }
        }
        public List<Vector2Int> LongWave(int[,] wavedir, char[][] gridMap, Vector2Int from)
        {
            int N = gridMap.Length, M = gridMap[0].Length;
            Queue<ChessPath> ways = new Queue<ChessPath>() { };
            GenerNewDir(wavedir, new ChessPath(from, new int[] { 0, 0 }), ref ways, N, M, gridMap);
            Queue<ChessPath> waysTemp = new Queue<ChessPath>() { };
            Vector2Int lastpos;
            while (true)
            {
                foreach (var way in ways)
                {
                    lastpos = way.GetLast();
                    int x = lastpos.x, y = lastpos.y;
                    while (x >= 0 && x < N && y >= 0 && y < M && gridMap[x][y] !='x') 
                    { 
                        if (gridMap[x][y] == 'e')
                        {
                            return way.Path;
                        }
                        else
                        {
                            gridMap[x][y] = '-';
                            GenerNewDir(wavedir, way,ref waysTemp, N, M, gridMap);
                            x += way.MoveDirection[0];
                            y += way.MoveDirection[1];
                            way.Path[way.Path.Count-1] =  new Vector2Int(x, y);
                        }
                    }
                }
                for (int i = 0; i < gridMap.Length; i++)
                {
                    for (int j = 0; j < gridMap[i].Length; j++)
                    {
                        if (gridMap[i][j] == '-') gridMap[i][j] = 'x';
                    }
                }
                if (waysTemp.Count == 0) return null;
                ways.Clear();
                ways = new Queue<ChessPath>(waysTemp);
                waysTemp.Clear();
            }
        }
        public void GenerNewDir(int[,] wavedir, ChessPath way, ref Queue<ChessPath> waysTemp, int N, int M, char[][] gridMap)
        {
            Vector2Int lastpos = way.GetLast();
            int[] dir = way.MoveDirection;
            for (int i = 0; i < wavedir.Length / 2; i++)
            {
                int x = lastpos.x + wavedir[i, 0],
                    y = lastpos.y + wavedir[i, 1];
                if (x >= 0 && x < N && y >= 0 && y < M && ((dir[0] == wavedir[i, 0] && dir[1] == wavedir[i, 1])||(dir[0] ==-1* wavedir[i, 0] && dir[1] ==-1* wavedir[i, 1]))==false)
                {
                    if (gridMap[x][y] == '#' || gridMap[x][y] == 'e')
                    {
                        waysTemp.Enqueue(new ChessPath(way, new Vector2Int(x, y), new int[] { wavedir[i, 0], wavedir[i, 1] }));
                    }
                }
            }
        }
        public char[][] CreateCharGrid(Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            char[][] gridMap = new char[grid.Size.y][];
            for (var i = 0; i < grid.Size.y; i++) gridMap[i] = new char[grid.Size.x];

            for (int i = 0; i < grid.Size.y; i++)
            {
                for (int j = 0; j < grid.Size.x; j++)
                {

                    if (grid.Get(i, j) == null) gridMap[j][i] = '#';
                    else gridMap[j][i] = 'x';
                }
            }
           gridMap[from.x][from.y] = 's';
           gridMap[to.x][to.y] = 'e';

           return gridMap;
        }
    }
}