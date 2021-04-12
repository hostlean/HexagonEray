using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGrid : MonoBehaviour
{
    
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float hexagonSize;

    [SerializeField] private GameObject hexagonSensor;
    [SerializeField] private GameObject sensorHolder;
    
    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
    public float HexagonSize => hexagonSize;
    
    private List<Vector2> _gridPoses = new List<Vector2>();

    private List<HexagonSensor> _sensors = new List<HexagonSensor>();

    private Dictionary<Vector2Int, HexagonSensor> _sensorGridPoses = new Dictionary<Vector2Int, HexagonSensor>();

    public List<Vector2> GridPoses => _gridPoses;

    private bool _canCheck = true;

    #region Singleton

    private static HexagonGrid _instance;

    public static HexagonGrid Instance
    {
        get
        {
            if(_instance == null)
                Debug.LogError("Hexagon Grid is Null");
            return _instance;
        }
    }

    #endregion
    
    private void Awake()
    {
        _instance = this;
        DrawHexagonGrid();
    }

    private void Update()
    {
        if (_canCheck)
        {
            if (CheckIfGridHasEmpty() == false)
            {
                _canCheck = false;
                PointGrid.Instance.CheckAndDestroyIfHasMatch();
            }
        }
    }

    private void DrawHexagonGrid()
    {
        Vector2 pos;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                float xPos = x * hexagonSize * .75f;
                float yPos = y * hexagonSize * Mathf.Sqrt(3) / 2;

                if (x % 2 != 0)
                    yPos -= Mathf.Sqrt(3) / 4 * hexagonSize;
                    

                pos = new Vector2(xPos, yPos);
                _gridPoses.Add(pos);
                CreateSensor(pos, x, y);
            }
        }
    }

    private void CreateSensor(Vector2 pos, int x, int y)
    {
        GameObject go = Instantiate(hexagonSensor, pos, Quaternion.identity, sensorHolder.transform);
        HexagonSensor hs = go.GetComponent<HexagonSensor>();
        
        _sensors.Add(hs);

        hs.GridX = x;
        hs.GridY = y;
        var gridPos = new Vector2Int(x, y);
        _sensorGridPoses.Add(gridPos, hs);
    }

    public void MoveHexagonsDown()
    {
        if (CheckIfGridHasEmpty())
        {
            Player.Instance.CanPickPoints = false;
            Player.Instance.DeactivatePoint();
            int counter = 0;
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var hs = _sensorGridPoses[new Vector2Int(x, y)];
                    if (hs.PointCheck() == false)
                    {
                        counter--;
                    }

                    if (hs.PointCheck())
                    {
                        Hexagon hexagon = hs.hexagon.GetComponent<Hexagon>();

                        hexagon.TargetPos = GridToWorldPos(x, y + counter);
                        hexagon.AtTarget = false;
                    }
                }

                for (int n = gridHeight + counter; n < gridHeight; n++)
                {
                    CreateNewHexagonWithGridPos(x, n, counter);
                    
                }

                counter = 0;

            }
            
            _canCheck = true;

            Player.Instance.CanPickPoints = true;
            PointGrid.Instance.CheckAndDestroyIfHasMatch();
            Player.Instance.ActivateSamePoint();

        }
    }

    public void CreateNewHexagonWithGridPos(int x, int y, int counter)
    {
        GameObject go = HexagonManager.Instance.CreateHexagonFromPool();
        Hexagon hexagon = go.GetComponent<Hexagon>();
        go.SetActive(true);
        go.transform.position = GridToWorldPos(x, y - counter + 1);
        hexagon.TargetPos = GridToWorldPos(x, y);
    }

    public Vector2 GridToWorldPos(int x, int y)
    {
        float xPos = x * hexagonSize * .75f;
        float yPos = y * hexagonSize * Mathf.Sqrt(3) / 2;

        if (x % 2 != 0)
            yPos -= .433f * hexagonSize;
                    

        Vector2 pos = new Vector2(xPos, yPos);
        return pos;
    }

    public bool CheckIfGridHasEmpty()
    {
        for (int i = 0; i < _sensors.Count; i++)
        {
            if (_sensors[i].PointCheck() == false)
                return true;
        }

        return false;
    }
    

    // public Vector2Int WorldToGridPos(Vector2 pos)
    // {
    //     float xPos = pos.x / (hexagonSize * .75f);
    //     float yPos = pos.y / (hexagonSize / (Mathf.Sqrt(3) / 2));
    //
    //     if (xPos % 2 != 0)
    //         yPos += .433f * hexagonSize;
    //
    //     return new Vector2Int((int) xPos, (int) yPos);
    // }
    
    
    // private void OnDrawGizmos()
    // {
    //     for (int x = 0; x < gridWidth; x++)
    //     {
    //         for (int y = 0; y < gridHeight; y++)
    //         {
    //             float xPos = x * hexagonSize * .75f;
    //             float yPos = y * hexagonSize * Mathf.Sqrt(3) / 2;
    //
    //             if (x % 2 == 0)
    //             {
    //                 Gizmos.DrawSphere(new Vector2(xPos, yPos), 0.05f);
    //             }
    //             else
    //             {
    //                 yPos -= .433f * hexagonSize;
    //                 Gizmos.DrawSphere(new Vector2(xPos, yPos), 0.05f);
    //             }
    //         }
    //     }
    // }
    
    
}
