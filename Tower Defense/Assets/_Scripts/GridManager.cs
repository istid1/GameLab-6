using UnityEngine;
namespace _Scripts
{
    public class GridManager : MonoBehaviour
    {
        private const int GridSizeX = 2;
        private const int GridSizeY = 2;
        private TowerID[,] _grid;
        private int _towersCount;

        void Start()
        {
            _grid = new TowerID[GridSizeX, GridSizeY];
            _towersCount = 0;
        }

        public void PlaceTower(TowerID tower, Vector3Int cellPosition)
        {
            if (IsValidPosition(cellPosition) && _grid[cellPosition.x, cellPosition.y] == null)
            {
                _grid[cellPosition.x, cellPosition.y] = tower;
                // You can then position your tower object on the scene based on the cell position
                _towersCount++;
            }
            else
            {
                Debug.LogError("Cell is already occupied or position is invalid");
            }
        }

        private bool IsValidPosition(Vector3Int cellPosition)
        {
            return cellPosition.x >= 0 && cellPosition.y >= 0 && cellPosition.x < GridSizeX && cellPosition.y < GridSizeY;
        }

        public bool IsGridFull()
        {
            return _towersCount == GridSizeX * GridSizeY;
        }

        // Draw grid gizmo
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            for (int x = 0; x <= GridSizeX; x++)
            {
                for (int y = 0; y <= GridSizeY; y++)
                {
                    Gizmos.DrawWireCube(new Vector3(x, y, 0), Vector3.one);
                }
            }
        }
    } 
}
    
    

