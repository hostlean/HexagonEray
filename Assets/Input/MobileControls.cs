// GENERATED AUTOMATICALLY FROM 'Assets/Input/MobileControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MobileControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MobileControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MobileControls"",
    ""maps"": [
        {
            ""name"": ""Mobile"",
            ""id"": ""e59075b7-ca23-415b-ad79-10eb296bdffe"",
            ""actions"": [
                {
                    ""name"": ""TouchInput"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cb81c750-fbd0-46d5-b88f-e3336a8741fd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchPress"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c1680d57-54c4-4b11-a906-9ea9655508e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""TouchPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cc1c137d-df8f-4167-b134-0f31d371b801"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""59d54226-0d4b-4d89-932d-917298ad423f"",
                    ""path"": ""<Touchscreen>/primaryTouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac6a740d-87b2-49fd-b015-43a374c9f79f"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9697de1b-9ab6-4190-9fe4-18f03e29e129"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mobile
        m_Mobile = asset.FindActionMap("Mobile", throwIfNotFound: true);
        m_Mobile_TouchInput = m_Mobile.FindAction("TouchInput", throwIfNotFound: true);
        m_Mobile_TouchPress = m_Mobile.FindAction("TouchPress", throwIfNotFound: true);
        m_Mobile_TouchPosition = m_Mobile.FindAction("TouchPosition", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Mobile
    private readonly InputActionMap m_Mobile;
    private IMobileActions m_MobileActionsCallbackInterface;
    private readonly InputAction m_Mobile_TouchInput;
    private readonly InputAction m_Mobile_TouchPress;
    private readonly InputAction m_Mobile_TouchPosition;
    public struct MobileActions
    {
        private @MobileControls m_Wrapper;
        public MobileActions(@MobileControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TouchInput => m_Wrapper.m_Mobile_TouchInput;
        public InputAction @TouchPress => m_Wrapper.m_Mobile_TouchPress;
        public InputAction @TouchPosition => m_Wrapper.m_Mobile_TouchPosition;
        public InputActionMap Get() { return m_Wrapper.m_Mobile; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MobileActions set) { return set.Get(); }
        public void SetCallbacks(IMobileActions instance)
        {
            if (m_Wrapper.m_MobileActionsCallbackInterface != null)
            {
                @TouchInput.started -= m_Wrapper.m_MobileActionsCallbackInterface.OnTouchInput;
                @TouchInput.performed -= m_Wrapper.m_MobileActionsCallbackInterface.OnTouchInput;
                @TouchInput.canceled -= m_Wrapper.m_MobileActionsCallbackInterface.OnTouchInput;
                @TouchPress.started -= m_Wrapper.m_MobileActionsCallbackInterface.OnTouchPress;
                @TouchPress.performed -= m_Wrapper.m_MobileActionsCallbackInterface.OnTouchPress;
                @TouchPress.canceled -= m_Wrapper.m_MobileActionsCallbackInterface.OnTouchPress;
                @TouchPosition.started -= m_Wrapper.m_MobileActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.performed -= m_Wrapper.m_MobileActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.canceled -= m_Wrapper.m_MobileActionsCallbackInterface.OnTouchPosition;
            }
            m_Wrapper.m_MobileActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TouchInput.started += instance.OnTouchInput;
                @TouchInput.performed += instance.OnTouchInput;
                @TouchInput.canceled += instance.OnTouchInput;
                @TouchPress.started += instance.OnTouchPress;
                @TouchPress.performed += instance.OnTouchPress;
                @TouchPress.canceled += instance.OnTouchPress;
                @TouchPosition.started += instance.OnTouchPosition;
                @TouchPosition.performed += instance.OnTouchPosition;
                @TouchPosition.canceled += instance.OnTouchPosition;
            }
        }
    }
    public MobileActions @Mobile => new MobileActions(this);
    public interface IMobileActions
    {
        void OnTouchInput(InputAction.CallbackContext context);
        void OnTouchPress(InputAction.CallbackContext context);
        void OnTouchPosition(InputAction.CallbackContext context);
    }
}
