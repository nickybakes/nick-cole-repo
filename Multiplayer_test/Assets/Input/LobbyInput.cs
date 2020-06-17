// GENERATED AUTOMATICALLY FROM 'Assets/Input/LobbyInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @LobbyInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @LobbyInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""LobbyInput"",
    ""maps"": [
        {
            ""name"": ""Client"",
            ""id"": ""f8c37598-27bf-472f-b366-db5de0ab00a9"",
            ""actions"": [
                {
                    ""name"": ""Add Keyboard Player"",
                    ""type"": ""Button"",
                    ""id"": ""b1a2e98b-1d5b-4cbf-978c-d0a1fa07a463"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Add Gamepad Player"",
                    ""type"": ""Button"",
                    ""id"": ""74c5c675-9b65-41e3-a640-3add72e7110f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""df4afb81-8e2e-404d-bd67-8a3163596881"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Add Keyboard Player"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4909635-c90e-4a4a-b705-1ef3799999e4"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Add Gamepad Player"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Client
        m_Client = asset.FindActionMap("Client", throwIfNotFound: true);
        m_Client_AddKeyboardPlayer = m_Client.FindAction("Add Keyboard Player", throwIfNotFound: true);
        m_Client_AddGamepadPlayer = m_Client.FindAction("Add Gamepad Player", throwIfNotFound: true);
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

    // Client
    private readonly InputActionMap m_Client;
    private IClientActions m_ClientActionsCallbackInterface;
    private readonly InputAction m_Client_AddKeyboardPlayer;
    private readonly InputAction m_Client_AddGamepadPlayer;
    public struct ClientActions
    {
        private @LobbyInput m_Wrapper;
        public ClientActions(@LobbyInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @AddKeyboardPlayer => m_Wrapper.m_Client_AddKeyboardPlayer;
        public InputAction @AddGamepadPlayer => m_Wrapper.m_Client_AddGamepadPlayer;
        public InputActionMap Get() { return m_Wrapper.m_Client; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ClientActions set) { return set.Get(); }
        public void SetCallbacks(IClientActions instance)
        {
            if (m_Wrapper.m_ClientActionsCallbackInterface != null)
            {
                @AddKeyboardPlayer.started -= m_Wrapper.m_ClientActionsCallbackInterface.OnAddKeyboardPlayer;
                @AddKeyboardPlayer.performed -= m_Wrapper.m_ClientActionsCallbackInterface.OnAddKeyboardPlayer;
                @AddKeyboardPlayer.canceled -= m_Wrapper.m_ClientActionsCallbackInterface.OnAddKeyboardPlayer;
                @AddGamepadPlayer.started -= m_Wrapper.m_ClientActionsCallbackInterface.OnAddGamepadPlayer;
                @AddGamepadPlayer.performed -= m_Wrapper.m_ClientActionsCallbackInterface.OnAddGamepadPlayer;
                @AddGamepadPlayer.canceled -= m_Wrapper.m_ClientActionsCallbackInterface.OnAddGamepadPlayer;
            }
            m_Wrapper.m_ClientActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AddKeyboardPlayer.started += instance.OnAddKeyboardPlayer;
                @AddKeyboardPlayer.performed += instance.OnAddKeyboardPlayer;
                @AddKeyboardPlayer.canceled += instance.OnAddKeyboardPlayer;
                @AddGamepadPlayer.started += instance.OnAddGamepadPlayer;
                @AddGamepadPlayer.performed += instance.OnAddGamepadPlayer;
                @AddGamepadPlayer.canceled += instance.OnAddGamepadPlayer;
            }
        }
    }
    public ClientActions @Client => new ClientActions(this);
    public interface IClientActions
    {
        void OnAddKeyboardPlayer(InputAction.CallbackContext context);
        void OnAddGamepadPlayer(InputAction.CallbackContext context);
    }
}
