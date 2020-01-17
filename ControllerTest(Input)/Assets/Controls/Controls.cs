// GENERATED AUTOMATICALLY FROM 'Assets/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""4a36b995-f317-4343-b0e9-c6b5662a5b8d"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""fe15f5e2-b8f7-400c-aefa-10d752834271"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""d56e93ad-bf7d-466f-9b9e-369e7da423ed"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""60879209-7773-4519-9695-4d63e031ceab"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""aa6dde29-a098-49ea-aa3d-f0785aba3529"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65a7e167-7376-4e9d-92a1-2f2608f937f3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""KebyBoard and Mouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c834aa43-447d-4292-bdcb-c5e0bf72ca7e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""4673f663-99d6-423b-895a-322cc3f47512"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3c119567-5e14-4d42-85b3-4606f5a53f07"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KebyBoard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0e7ab78c-0bf5-4ba9-a86b-7abd4bcda4d0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KebyBoard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""17ea73b0-ed1c-47b0-8497-15595a4eeea9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KebyBoard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7e801b2a-35de-4c5f-b186-771f8ba52196"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KebyBoard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b3b64b93-d5f2-4925-8b6c-54c8590bd81e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KebyBoard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6d4a1fdf-2477-4064-bdd5-c2649071e227"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KebyBoard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2320c0ab-0bd1-47ab-b646-c207068d62ec"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KebyBoard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""dbcb1fa4-62bb-4375-aa31-a38ade72976a"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KebyBoard and Mouse"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7732ecd-ea98-45b8-99d9-e4faef781e27"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KebyBoard and Mouse"",
            ""bindingGroup"": ""KebyBoard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_Shoot = m_GamePlay.FindAction("Shoot", throwIfNotFound: true);
        m_GamePlay_Move = m_GamePlay.FindAction("Move", throwIfNotFound: true);
        m_GamePlay_Start = m_GamePlay.FindAction("Start", throwIfNotFound: true);
        // Player2
        m_Player2 = asset.FindActionMap("Player2", throwIfNotFound: true);
        m_Player2_Shoot = m_Player2.FindAction("Shoot", throwIfNotFound: true);
        m_Player2_Move = m_Player2.FindAction("Move", throwIfNotFound: true);
        m_Player2_Start = m_Player2.FindAction("Start", throwIfNotFound: true);
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

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private IGamePlayActions m_GamePlayActionsCallbackInterface;
    private readonly InputAction m_GamePlay_Shoot;
    private readonly InputAction m_GamePlay_Move;
    private readonly InputAction m_GamePlay_Start;
    public struct GamePlayActions
    {
        private @Controls m_Wrapper;
        public GamePlayActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_GamePlay_Shoot;
        public InputAction @Move => m_Wrapper.m_GamePlay_Move;
        public InputAction @Start => m_Wrapper.m_GamePlay_Start;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void SetCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnShoot;
                @Move.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                @Start.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnStart;
            }
            m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
            }
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);

    // Player2
    private readonly InputActionMap m_Player2;
    private IPlayer2Actions m_Player2ActionsCallbackInterface;
    private readonly InputAction m_Player2_Shoot;
    private readonly InputAction m_Player2_Move;
    private readonly InputAction m_Player2_Start;
    public struct Player2Actions
    {
        private @Controls m_Wrapper;
        public Player2Actions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_Player2_Shoot;
        public InputAction @Move => m_Wrapper.m_Player2_Move;
        public InputAction @Start => m_Wrapper.m_Player2_Start;
        public InputActionMap Get() { return m_Wrapper.m_Player2; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player2Actions set) { return set.Get(); }
        public void SetCallbacks(IPlayer2Actions instance)
        {
            if (m_Wrapper.m_Player2ActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_Player2ActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_Player2ActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_Player2ActionsCallbackInterface.OnShoot;
                @Move.started -= m_Wrapper.m_Player2ActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_Player2ActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_Player2ActionsCallbackInterface.OnMove;
                @Start.started -= m_Wrapper.m_Player2ActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_Player2ActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_Player2ActionsCallbackInterface.OnStart;
            }
            m_Wrapper.m_Player2ActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
            }
        }
    }
    public Player2Actions @Player2 => new Player2Actions(this);
    private int m_KebyBoardandMouseSchemeIndex = -1;
    public InputControlScheme KebyBoardandMouseScheme
    {
        get
        {
            if (m_KebyBoardandMouseSchemeIndex == -1) m_KebyBoardandMouseSchemeIndex = asset.FindControlSchemeIndex("KebyBoard and Mouse");
            return asset.controlSchemes[m_KebyBoardandMouseSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IGamePlayActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
    }
    public interface IPlayer2Actions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
    }
}
