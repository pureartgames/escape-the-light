using UnityEngine;

namespace _Project.CodeBase.Gameplay
{
    public struct Cell
    {
        public bool IsVisited { get; set; }
        public Vector2Int PreviousCellPosition { get; set; }
    }
}