using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonSensor : MonoBehaviour
{
    public int GridX { get; set; }
    public int GridY { get; set; }
    [field: SerializeField] public bool HasHexagon { get; set; }

    public GameObject hexagon;

    private RaycastHit2D hit;

    [SerializeField] private LayerMask hexagonLayer;
    
    
    public bool PointCheck()
    {

        hit = Physics2D.Raycast(transform.position, Vector2.zero, -1, hexagonLayer);
 
        Debug.DrawRay(transform.position, Vector3.forward * -1, Color.green);

        HasHexagon = hit.collider != null;

        if(HasHexagon)
            hexagon = hit.collider.gameObject;

        
        return HasHexagon;
    }
}
