using UnityEngine;

namespace _Project.CodeBase.Gameplay
{
    public interface IMazeMapGenerator
    {
        Cell[,] Generate(Vector2Int size);
    }
}