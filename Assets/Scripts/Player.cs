
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float minimumSwipeDistance = .2f;
    [SerializeField] private float maximumTime = .1f;
    
    [SerializeField, Range(0, 1)] private float directionThreshold = .9f;
    
    
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask dotLayer;
    [SerializeField] private LayerMask chosenHexagonLayer;

    [SerializeField] private HexagonGrid hexagonGrid;

    private Vector3 _mousePos;

    private bool _onPoint;

    private RaycastHit2D hit;

    private IntersectPoint _intersectPoint;

    public bool CanPickPoints { get; set; } = false;

    private Vector2 startPos;
    private float startTime;

    private Vector2 endPos;
    private float endTime;

    private enum Swipe
    {
        Left,
        Right,
        Up,
        Down
    }

    private Swipe swipe;
    

    #region Singleton

    private static Player _instance;

    public static Player Instance
    {
        get
        {
            if(_instance == null)
                Debug.LogError("Player is Null");
            return _instance;
        }
    }

    #endregion
    
    
    private void Awake()
    {
        _instance = this;
    }

    private void OnEnable()
    {
        InputManager.Instance.OnStartTouch += PickPoint;
        InputManager.Instance.OnStartSwipe += SwipeStart;
        InputManager.Instance.OnEndSwipe += SwipeEnd;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnEndTouch -= PickPoint;
        InputManager.Instance.OnStartSwipe -= SwipeStart;
        InputManager.Instance.OnEndSwipe -= SwipeEnd;
    }

    // void Update()
    // {
    //     // _mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    //     // _mousePos.z =  -0.5f;
    //
    //     if (CanPickPoints)
    //     {
    //         // if (PointCheck() && Input.GetMouseButtonDown(0))
    //         // {
    //         //     if(_intersectPoint != null)
    //         //         _intersectPoint.DeactivatePoint();
    //         //     _intersectPoint = hit.collider.GetComponent<IntersectPoint>();
    //         //     _intersectPoint.ActivatePoint();
    //         // }
    //
    //         // if (Input.GetKeyDown(KeyCode.Space))
    //         // {
    //         //     _intersectPoint.CanTurn = true;
    //         //     CanPickPoints = false;
    //         // }
    //         //
    //         // if (Input.GetKeyDown(KeyCode.A))
    //         // {
    //         //     PointGrid.Instance.CheckAndDestroyIfHasMatch();
    //         //     PointGrid.Instance.HasMatch = false;
    //         // }
    //         //
    //         // if (Input.GetKeyDown(KeyCode.R))
    //         // {
    //         //     SceneManager.LoadScene(SceneManager.sceneCount - 1);
    //         // }
    //     }
    // }

    private void SwipeStart(Vector2 pos, float time)
    {
        startPos = pos;
        startTime = time;
    }

    private void SwipeEnd(Vector2 pos, float time)
    {
        endPos = pos;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(startPos, endPos) >= minimumSwipeDistance &&
            (endTime - startTime) <= maximumTime)
        {
            //Debug.DrawLine(startPos, endPos, Color.red, 2f);
            Vector3 direction = endPos - startPos;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
            if (ChosenHexagonCheck(startPos))
            {
                //Debug.Log("Chosen Hexagon Hit");
                Vector3 touchToPointDir = (Vector3) startPos - _intersectPoint.PointPos;
                
                Vector2 newDir2D = new Vector2(touchToPointDir.x, touchToPointDir.y);
                
                switch (swipe)
                {
                    case Swipe.Right:
                        RotatePoint(newDir2D.y > 0);
                        break;
                    case Swipe.Left:
                        RotatePoint(newDir2D.y < 0);
                        break;
                    case Swipe.Up:
                        RotatePoint(newDir2D.x < 0);
                        break;
                    case Swipe.Down:
                        RotatePoint(newDir2D.x > 0);
                        break;
                }
            }
        }
    }

    private void RotatePoint(bool clockwise)
    {
        _intersectPoint.Clockwise = clockwise;
        _intersectPoint.CanTurn = true;
        CanPickPoints = false;
    }

    private void SwipeDirection(Vector2 dir)
    {
        if (Vector2.Dot(Vector2.up, dir) > directionThreshold)
        {
            swipe = Swipe.Up;
        }
        if (Vector2.Dot(Vector2.right, dir) > directionThreshold)
        {
            swipe = Swipe.Right;
        }
        if (Vector2.Dot(Vector2.left, dir) > directionThreshold)
        {
            swipe = Swipe.Left;
        }
        if (Vector2.Dot(Vector2.down, dir) > directionThreshold)
        {
            swipe = Swipe.Down;
        }
        
    }


    private void PickPoint(Vector2 screenPos, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPos.x, screenPos.y, cam.nearClipPlane);
        Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = -.5f;
        if (CanPickPoints)
        {
            if (PointCheck(worldCoordinates))
            {
                // if(_intersectPoint != null)
                //     _intersectPoint.DeactivatePoint();
                _intersectPoint.ActivatePoint();
            }
        }
    }

    private bool ChosenHexagonCheck(Vector3 pos)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(pos, Vector2.zero, 1, chosenHexagonLayer);

        return hit2D.collider != null;
    }
    
    private bool PointCheck(Vector3 pos)
    {

        hit = Physics2D.Raycast(pos, Vector2.zero, 1, dotLayer);
 
        Debug.DrawRay(pos, Vector3.forward * 1, Color.green);

        if (hit.collider != null)
        {
            if(_intersectPoint != null)
                _intersectPoint.DeactivatePoint();
            _intersectPoint = hit.collider.GetComponent<IntersectPoint>();
        }
        
        return hit.collider != null;
    }

    public void DeactivatePoint()
    {
        if (_intersectPoint != null)
        {
            _intersectPoint.CanCheck = false;
            _intersectPoint.DeactivatePoint();
            _intersectPoint.gameObject.SetActive(false);
        }
    }

    public void ActivateSamePoint()
    {
        if (_intersectPoint != null)
        {
            _intersectPoint.gameObject.SetActive(true);
            _intersectPoint.CanCheck = true;
        }
    }

    public void ResetPoint()
    {
        if (_intersectPoint != null)
        {
            _intersectPoint.DeactivatePoint();
            _intersectPoint.gameObject.SetActive(false);
            _intersectPoint.gameObject.SetActive(true);
            _intersectPoint.ActivatePoint();
        }
       
    }
    
    
}
