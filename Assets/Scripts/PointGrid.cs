using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGrid : MonoBehaviour
{
    [SerializeField] private HexagonGrid hexagonGrid;
    [SerializeField] private GameObject pointObject;
    [SerializeField] private GameObject pointHolder;
    
    public int GridWidth => (hexagonGrid.GridWidth - 1) ;
    public int GridHeight => (hexagonGrid.GridHeight - 1) * 2;
    public float HexagonSize => hexagonGrid.HexagonSize;

    public List<GameObject> _points = new List<GameObject>();

    
    private float _xPos;
    private float _yPos;
    private float _oddXPos;

    public bool HasMatch { get; set; }

    #region Singleton

    private static PointGrid _instance;

    public static PointGrid Instance
    {
        get
        {
            if(_instance == null)
                Debug.LogError("Point Grid is Null");
            return _instance;
        }
    }

    #endregion
   
    
    
    private void Awake()
    {
        _instance = this;
        _xPos = HexagonSize / 2;
        _oddXPos = HexagonSize / 4;
        _yPos = HexagonSize * Mathf.Sqrt(3) / 4;
        DrawIntersectionGrid();
    }

    public void DrawIntersectionGrid()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            if (x == 0)
            {
                _xPos = HexagonSize / 2;
                _oddXPos = HexagonSize / 4;
            }
            if (x != 0 && x % 2 == 1)
            {
                _xPos += HexagonSize / 2;
                _oddXPos += HexagonSize;
            }
            if (x != 0 && x % 2 == 0)
            {
                _xPos += HexagonSize;
                _oddXPos += HexagonSize / 2;
            }
            
            for (int y = 0; y < GridHeight; y++)
            {
                float pos = y % 2 == 0 ? _xPos : _oddXPos;
                GameObject go = Instantiate(pointObject, 
                    new Vector2(pos, _yPos * y), 
                    Quaternion.identity, 
                    pointHolder.transform);
                _points.Add(go);

            }
        }
    }

    public void CheckAndDestroyIfHasMatch()
    {
        List<GameObject> hasMatchPoints = new List<GameObject>();
        
        for (int i = 0; i < _points.Count; i++)
        {
            IntersectPoint p = _points[i].GetComponent<IntersectPoint>();
            if (p.CheckForMatch())
            {
                hasMatchPoints.Add(_points[i]);
            }
        }

        if (hasMatchPoints.Count > 0)
        {
            HasMatch = true;
            for (int i = 0; i < hasMatchPoints.Count; i++)
            {
                IntersectPoint p = hasMatchPoints[i].GetComponent<IntersectPoint>();
                p.DestroyHexagons();
            }
            
            HexagonGrid.Instance.MoveHexagonsDown();

            
            hasMatchPoints.Clear();
        }
        
    }
    
    
    // private void OnDrawGizmos()
    // {
    //     _xPos = HexagonSize / 2;
    //     _yPos = HexagonSize * Mathf.Sqrt(3) / 4;
    //     _oddXPos = HexagonSize / 4;
    //     
    //     for (int x = 0; x < GridWidth; x++)
    //     {
    //         if (x == 0)
    //         {
    //             _xPos = HexagonSize / 2;
    //             _oddXPos = HexagonSize / 4;
    //         }
    //         if (x != 0 && x % 2 == 1)
    //         {
    //             _xPos += HexagonSize / 2;
    //             _oddXPos += HexagonSize;
    //         }
    //         if (x != 0 && x % 2 == 0)
    //         {
    //             _xPos += HexagonSize;
    //             _oddXPos += HexagonSize / 2;
    //         }
    //
    //
    //         for (int y = 0; y < GridHeight; y++)
    //         {
    //             float pos = y % 2 == 0 ? _xPos : _oddXPos;
    //             Gizmos.DrawSphere(new Vector2(pos, _yPos * y), 0.05f);
    //         }
    //     }
    // }

    // public void DrawHexagonPoints(Vector3 pos, float radius, Quaternion rot)
    // {
    //     var hexPoints = 6;
    //     Vector2[] points = new Vector2[hexPoints];
    //
    //     for (int i = 0; i < hexPoints; i++)
    //     {
    //         float t = i / (float)hexPoints;
    //         float rad = t * Constants.Numbers.TAU;
    //
    //         Vector2 point = new Vector2(Mathf.Cos(rad),
    //             Mathf.Sin(rad));
    //         point *= radius;
    //
    //         points[i] = pos + rot * point;
    //     }
    //   
    //     for (int i = 0; i < hexPoints; i++)
    //     {
    //         Gizmos.DrawSphere(points[i], 0.05f);
    //     }
    // }

}
