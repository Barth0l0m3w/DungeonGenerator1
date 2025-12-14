using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DungeonGenerator : MonoBehaviour
{
    private class Cell
    {
        public bool Visited;
        public bool[] Status = new bool[4];
    }
    
    [Header("Dungeon Variables")]
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] private Vector2 _size;
    [SerializeField] private int _startPos;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private int _maxRooms;
    
    [Header("Room Variants")]
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] private GameObject[] _roomPrefabs;
    
    [Header("Necessary Links")]
    [HorizontalLine(color: EColor.Red)]
    [Tooltip("in what gameObject to spawn the dungeon rooms")]
    [SerializeField] private GameObject _dungeon;
    [SerializeField] private NavMeshBuild _navBuild;
    
    private List<Cell> _board;

    [Button]
    private void CreateNewDungeon()
    {
        ClearOldDungeon();
        Generator();
        _navBuild.BuildNavigation();
    }
    
    [Button]
    private void ClearOldDungeon()
    {
        for (int i = _dungeon.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(_dungeon.transform.GetChild(0).gameObject);
        }
        
        _navBuild.ClearOldNav();
    }
    
    void GenerateDungeon()
    {
        for (int i = 0; i < _size.x; i++)
        {
            for (int j = 0; j < _size.y; j++)
            {
                Cell currentCell = _board[Mathf.FloorToInt(i + j * _size.x)];
                if (currentCell.Visited)//only instantiate when the area has been visited so the whole board doesn't fill up with rooms
                {
                    int randomRoom = Random.Range(0, _roomPrefabs.Length);
                    var newRoom =
                        Instantiate(_roomPrefabs[randomRoom], new Vector3(i * _offset.x, 0, -j * _offset.y), Quaternion.identity, _dungeon.transform)
                            .GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.Status);
                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }
    
    void Generator()
    {
        _board = new List<Cell>();
        for (int i = 0; i < _size.x; i++)
        {
            for (int j = 0; j < _size.y; j++)
            {
                _board.Add(new Cell());
            }
        }

        int currentCell = _startPos;
        Stack<int> path = new();
        
        int rooms = 0;
        while
            (rooms < _maxRooms) //higher number == more filled up grid
        {
            rooms++;

            _board[currentCell].Visited = true;

            if (currentCell == _board.Count - 1) //stop the search when you are at the last cell
            {
                break;
            }

            List<int> neighbours = CheckNeighbors(currentCell);

            if (neighbours.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                if (newCell > currentCell)
                {
                    //down or right
                    if (newCell - 1 == currentCell)
                    {
                        _board[currentCell].Status[2] = true;
                        currentCell = newCell;
                        _board[currentCell].Status[3] = true;
                    }
                    else
                    {
                        _board[currentCell].Status[1] = true;
                        currentCell = newCell;
                        _board[currentCell].Status[0] = true;
                    }
                }
                else
                {
                    //up or left
                    if (newCell + 1 == currentCell)
                    {
                        _board[currentCell].Status[3] = true;
                        currentCell = newCell;
                        _board[currentCell].Status[2] = true;
                    }
                    else
                    {
                        _board[currentCell].Status[0] = true;
                        currentCell = newCell;
                        _board[currentCell].Status[1] = true;
                    }
                }
            }
        }

        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        //up
        if (cell - _size.x >= 0 && !_board[Mathf.FloorToInt(cell - _size.x)].Visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - _size.x));
        }

        //down
        if (cell + _size.x < _board.Count && !_board[Mathf.FloorToInt(cell + _size.x)].Visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + _size.x));
        }

        //right
        if ((cell + 1) % _size.x != 0 && !_board[Mathf.FloorToInt(cell + 1)].Visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }

        //left
        if (cell % _size.x != 0 && !_board[Mathf.FloorToInt(cell - 1)].Visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }
}