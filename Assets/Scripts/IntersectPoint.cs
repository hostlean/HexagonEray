using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectPoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject whiteDot;

    [SerializeField] private List<GameObject> hexagons = new List<GameObject>();

    [SerializeField] private float turnSpeed;

    public bool CanCheck { get; set; } = true;

    private Quaternion _originalRot;
    
    public bool IsActive { get; set; }
    
    public bool CanTurn { get; set; }
    
    public Vector3 PointPos { get; private set; }
    
    public bool HasMatch { get; set; }

    public Action OnTurning;

    private float _rotateAngle = 120;
    private Quaternion _originalAngle;
    private bool _canStopTurning;
    
    public bool Clockwise { get; set; }

    private void Awake()
    {
        PointPos = transform.position;
        spriteRenderer.enabled = false;
        whiteDot.SetActive(false);
        IsActive = false;
    }

    private void Update()
    {
        if (CanTurn)
        {
            Turn120();
        }

    }

    public void ActivatePoint()
    {
        _originalAngle = transform.rotation;
        spriteRenderer.enabled = true;
        whiteDot.SetActive(true);
        hexagons.ForEach(c =>
        {
            c.layer = Constants.Layers.chosenHexagon;
            Hexagon hexagon = c.GetComponent<Hexagon>();
            hexagon.ApplyParent(gameObject);
            hexagon.ActivateOutline();
            
        });
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        IsActive = true;
    }

    public void DeactivatePoint()
    {
        CanTurn = false;
        spriteRenderer.enabled = false;
        whiteDot.SetActive(false);
        hexagons.ForEach(c =>
        {
            c.layer = Constants.Layers.hexagon;
            Hexagon hexagon = c.GetComponent<Hexagon>();
            hexagon.ReturnHolderParent();
            hexagon.DeactivateOutline();
        });
        IsActive = false;
        
        // if (_rotateAngle == 120)
        // {
        //     transform.rotation = Quaternion.AngleAxis(_rotateAngle, Vector3.forward);
        // }
        // else if(_rotateAngle == 240)
        // {
        //     transform.rotation = Quaternion.AngleAxis(-_rotateAngle / 2, Vector3.forward);
        //     _rotateAngle = 120;
        // }
        
        transform.rotation = Quaternion.Euler(0 ,0, 0);

        _rotateAngle = 120;
        
        _canStopTurning = false;

    }

    public void Turn120()
    {
        var rotation = transform.rotation;
        
        var targetAngle = Quaternion.Euler(0, 0,  _rotateAngle);
        var inverseAngle = Quaternion.Euler(0, 0, _rotateAngle - 360);
        var zeroAngle = Quaternion.Euler(0, 0, 0);
        
        var rotate = Quaternion.LerpUnclamped(rotation, targetAngle, Time.deltaTime * turnSpeed);
        var inverseRot = Quaternion.Inverse(rotate);

        if (_canStopTurning)
        {
            if (rotation == _originalAngle)
            {
                Player.Instance.CanPickPoints = true;
                Player.Instance.ResetPoint();
            }
        }
        
        if (Clockwise)
        {
            inverseAngle = Quaternion.Euler(0, 0, 360 - _rotateAngle);
            targetAngle = Quaternion.Inverse(targetAngle);
            rotate = Quaternion.LerpUnclamped(rotation, targetAngle, Time.deltaTime * turnSpeed);
        }

        else
        {
            targetAngle = Quaternion.Euler(0, 0,  _rotateAngle);
            inverseAngle = Quaternion.Euler(0, 0, _rotateAngle - 360);
            rotate = Quaternion.LerpUnclamped(rotation, targetAngle, Time.deltaTime * turnSpeed);
        }
        
        if (rotation == targetAngle)
        {
            PointGrid.Instance.CheckAndDestroyIfHasMatch();
            if (PointGrid.Instance.HasMatch)
            {
                MinusToBombs();
                Player.Instance.CanPickPoints = true;
                Player.Instance.ResetPoint();
                PointGrid.Instance.HasMatch = false;
            }
            _rotateAngle += 120;
        }
        else if (rotation == inverseAngle)
        {
            PointGrid.Instance.CheckAndDestroyIfHasMatch();
            if (PointGrid.Instance.HasMatch)
            {
                MinusToBombs();
                Player.Instance.CanPickPoints = true;
                Player.Instance.ResetPoint();
                PointGrid.Instance.HasMatch = false;
            }
            _rotateAngle = -120;
            _canStopTurning = true;
        }

        
        transform.rotation = rotate;
    }

    public bool CheckForMatch()
    {
        if (hexagons.Count == 3)
        {
            for (int i = 0; i < hexagons.Count; i++)
            {
                var type1 = hexagons[i].GetComponent<Hexagon>().hType;
                for (int p = 0; p < hexagons.Count; p++)
                {
                    var type2 = hexagons[p].GetComponent<Hexagon>().hType;
                
                    if (type1 != type2)
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    public void DestroyHexagons()
    {
        for (int i = hexagons.Count - 1; i > -1; i--)
        {
            Hexagon hexagon = hexagons[i].GetComponent<Hexagon>();
            hexagon.ReturnHolderParent();
            hexagon.DeactivateOutline();
            hexagons[i].SetActive(false);
            //hexagons.Remove(hexagons[i]);
        }
    }

    public void ResetPoint()
    {
        DeactivatePoint();
        ActivatePoint();
    }

    public void MinusToBombs()
    {
        var bomblist = HexagonManager.Instance.bombs;
        if (bomblist.Count > 0)
        {
            foreach (var bomb in bomblist)
            {
                bomb.GetComponent<HexagonBomb>().UpdateText();
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CanCheck)
        {
            if (other.CompareTag(Constants.Tags.hexagon))
            {
                hexagons.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag(Constants.Tags.hexagon))
        {
            hexagons.Remove(other.gameObject);
        }
        
    }
}
