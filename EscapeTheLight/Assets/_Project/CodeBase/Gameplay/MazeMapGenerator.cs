using System.Collections.Generic;
using UnityEngine;

namespace _Project.CodeBase.Gameplay
{
    public sealed class MazeMapGenerator : IMazeMapGenerator
    {
        private const int DirectionsCount = 4;

        public static readonly Vector2Int UndefinedCellPosition = Vector2Int.one * -1;
        
        private readonly List<Vector2Int> _adjacentCells = new List<Vector2Int>(DirectionsCount);

        private Cell[,] _map;
        
        public Cell[,] Generate(Vector2Int size)
        {
            _map = new Cell[size.x, size.y];
            
            Vector2Int currentCellPos = Vector2Int.zero;
            VisitCell(currentCellPos, UndefinedCellPosition);

            Stack<Vector2Int> history = new Stack<Vector2Int>(size.x * size.y);
            history.Push(currentCellPos);
            
            while (history.Count > 0)
            {
                Vector2Int? nextCellPos = FindNextCell(currentCellPos);

                if (nextCellPos == null)
                {
                    currentCellPos = history.Pop();
                    continue;
                }

                VisitCell(nextCellPos.Value, currentCellPos);
                
                history.Push(nextCellPos.Value);
                currentCellPos = nextCellPos.Value;
            }

            return _map;
        }

        private void VisitCell(Vector2Int currentCellPos, Vector2Int previousCellPos)
        {
            _map[currentCellPos.x, currentCellPos.y].PreviousCellPosition = previousCellPos;
            _map[currentCellPos.x, currentCellPos.y].IsVisited = true;
        }

        private Vector2Int? FindNextCell(Vector2Int currentCellPosition)
        {
            _adjacentCells.Clear();
            
            RegisterAdjacentCells(currentCellPosition);

            if (_adjacentCells.Count == 0)
                return null;

            Vector2Int chosenCell = _adjacentCells[Random.Range(0, _adjacentCells.Count)];

            return chosenCell;
        }

        private void RegisterAdjacentCells(Vector2Int currentCellPosition)
        {
            Vector2Int? southCellPos = currentCellPosition.y - 1 >= 0
                ? new Vector2Int(currentCellPosition.x, currentCellPosition.y - 1)
                : null;
            
            RegisterAdjacentCell(southCellPos);

            Vector2Int? eastCell = currentCellPosition.x + 1 < _map.GetLength(0)
                ? new Vector2Int(currentCellPosition.x + 1, currentCellPosition.y)
                : null;
            
            RegisterAdjacentCell(eastCell);

            Vector2Int? northCell = currentCellPosition.y + 1 < _map.GetLength(1)
                ? new Vector2Int(currentCellPosition.x, currentCellPosition.y + 1)
                : null;
            
            RegisterAdjacentCell(northCell);

            Vector2Int? westCell = currentCellPosition.x - 1 >= 0
                ? new Vector2Int(currentCellPosition.x - 1, currentCellPosition.y)
                : null;
            
            RegisterAdjacentCell(westCell);
        }

        private void RegisterAdjacentCell(Vector2Int? cellPos)
        {
            if (cellPos != null && !_map[cellPos.Value.x, cellPos.Value.y].IsVisited)
                _adjacentCells.Add(cellPos.Value);
        }
    }
}
