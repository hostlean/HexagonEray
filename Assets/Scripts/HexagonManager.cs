using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : MonoBehaviour
{
    [SerializeField] private HexagonGrid hexagonGrid;


    private List<GameObject> _hexagons = new List<GameObject>();

    [SerializeField] private GameObject hexagon;
    [SerializeField] private GameObject hexagonBomb;
    [SerializeField] private int bombScoreLimit;
    [SerializeField] private GameObject hexagonHolder;
    [SerializeField] private float spawnYPos;

    public List<GameObject> bombs = new List<GameObject>();
    
    [Header("Hexagon Colors")]
    [SerializeField] private Color red, orange, yellow, blue, purple, green;

    private Color[] _colors;
    public Color[] Colors => _colors;

    private int _scroeCounter = 1;
    
    #region Singleton

    private static HexagonManager _instance;

    public static HexagonManager Instance
    {
        get
        {
            if(_instance == null)
                Debug.LogError("Hexagon Manager is Null");
            return _instance;
        }
    }

    #endregion
   

    private void Awake()
    {
        _instance = this;
        
        _colors = new[] {red, orange, yellow, blue, purple, green};
        
        foreach (Transform c in hexagonHolder.transform)
        {
            _hexagons.Add(c.gameObject);
        }

        StartCoroutine(ActivateHexagon(Vector2.zero));

    }
    
    IEnumerator ActivateHexagon(Vector2 pos)
    {
        var points = hexagonGrid.GridPoses;
        for (int i = 0; i < points.Count; i++)
        {
            GameObject go = CreateHexagonFromPool();
            //int randomColorIndex = Random.Range(0, _colors.Length);
            go.SetActive(true);
            go.transform.position = new Vector2(points[i].x, points[i].y + spawnYPos);
            go.GetComponent<Hexagon>().TargetPos = points[i];
            go.transform.localScale = new Vector3(hexagonGrid.HexagonSize, hexagonGrid.HexagonSize, 1);
            //_hexagons[i].GetComponent<SpriteRenderer>().color = _colors[randomColorIndex];
            yield return new WaitForSeconds(0.05f);
        }
    }

    public GameObject CreateHexagonFromPool()
    {
        for (int i = 0; i < _hexagons.Count; i++)
        {
            if (GameManager.Instance.Score >= _scroeCounter * bombScoreLimit)
            {
                GameObject bomb = Instantiate(hexagonBomb, Vector3.zero, Quaternion.identity, hexagonHolder.transform);
                bomb.GetComponent<HexagonBomb>().UpdateText();
                _scroeCounter++;
                bombs.Add(bomb);
                bomb.transform.localScale = new Vector3(hexagonGrid.HexagonSize, hexagonGrid.HexagonSize, 1);
                return bomb;
            }
            
            if (_hexagons[i].activeSelf == false)
            {
                return _hexagons[i];
            }

            GameObject go = Instantiate(hexagon, Vector3.zero, Quaternion.identity, hexagonHolder.transform);
            go.SetActive(false);
            go.transform.localScale = new Vector3(hexagonGrid.HexagonSize, hexagonGrid.HexagonSize, 1);
            return go;

        }

        return null;
    }
}
