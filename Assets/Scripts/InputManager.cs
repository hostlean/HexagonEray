
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    #region Singleton

    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            if(_instance == null)
                Debug.LogError("Input Manager is Null");
            return _instance;
        }
    }

    #endregion

    private Camera camera;
    
    private MobileControls _mobileControls;
    
    public delegate void StartTouchEvent(Vector2 pos, float time);
    
    public event StartTouchEvent OnStartTouch;

    public event StartTouchEvent OnStartSwipe;
    
    public delegate void EndTouchEvent(Vector2 pos, float time);
    
    public event EndTouchEvent OnEndTouch;

    public event EndTouchEvent OnEndSwipe;

    private void Awake()
    {
        _instance = this;
        _mobileControls = new MobileControls();
        camera = Camera.main;

    }
    
    private void OnEnable()
    {
        _mobileControls.Enable();
    }
    
    private void Start()
    {
        _mobileControls.Mobile.TouchPress.started += ctx => StartTouch(ctx);
        _mobileControls.Mobile.TouchPress.canceled += ctx => EndTouch(ctx);
        _mobileControls.Mobile.TouchPress.started += ctx => StartSwipe(ctx);
        _mobileControls.Mobile.TouchPress.canceled += ctx => EndSwipe(ctx);
    }
    
    private void StartTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
        {
            OnStartTouch(_mobileControls.Mobile.TouchPosition.ReadValue<Vector2>(), (float) context.startTime);
        }
    }
    
    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
            OnEndTouch(_mobileControls.Mobile.TouchPosition.ReadValue<Vector2>(), (float) context.time);
    
    }

    private void StartSwipe(InputAction.CallbackContext context)
    {
        if (OnStartSwipe != null)
        {
            OnStartSwipe(ScreenToWorldPoint(camera, _mobileControls.Mobile.TouchPosition.ReadValue<Vector2>()),
                (float)context.startTime);
        }
    }

    private void EndSwipe(InputAction.CallbackContext context)
    {
        if (OnEndSwipe != null)
        {
            OnEndSwipe(ScreenToWorldPoint(camera, _mobileControls.Mobile.TouchPosition.ReadValue<Vector2>()),
                (float)context.time);
        }
    }

    public Vector3 ScreenToWorldPoint(Camera camera, Vector3 pos)
    {
        pos.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(pos);
    }

    public Vector2 TouchPosiiton()
    {
        return ScreenToWorldPoint(camera, _mobileControls.Mobile.TouchPosition.ReadValue<Vector2>());
    }
    


    private void OnDisable()
    {
        _mobileControls.Disable();
    }


}
