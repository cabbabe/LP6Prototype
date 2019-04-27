/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


// For TCK integration Uncomment this 'define' 
// OR Add to 'PlayerSettings->OtherSettings->Configuration->ScriptingDefineSymbols'.
//#define TOUCH_CONTROLS_KIT


#if TOUCH_CONTROLS_KIT
using TouchControlsKit;
using TCKAxisType = TouchControlsKit.EAxisType;
using TCKActionEvent = TouchControlsKit.EActionEvent;
#endif

using UnityEngine;
using UnityEngine.EventSystems;

namespace SmartFPController
{
    public class SmartInputManager : MonoBehaviour
    {
        [System.Serializable]
        public sealed class Axes
        {
            public string
                moveX = "Move Horizontal", moveY = "Move Vertical", lookX = "Look Horizontal", lookY = "Look Vertical";

#if TOUCH_CONTROLS_KIT
            public string moveJoystick = "Move Joystick", lookTouchpad = "Look Touchpad";
#endif
        }

        [System.Serializable]
        public sealed class Actions
        {
            public string
                run = "Run", jump = "Jump", crouch = "Crouch",
                pause = "Pause", blockCursor = "Block Cursor", unblockCursor = "Unblock Cursor";
        }


        [SerializeField]
        private EUpdateType updateType = EUpdateType.Update;

#if TOUCH_CONTROLS_KIT
        public enum EInputType { Standalone = 0, TouchControlsKit = 1 }
        [SerializeField]
        private EInputType inputType = EInputType.Standalone;

        [SerializeField]
        private TCKInput touchUIElements = null;
#endif

        [SerializeField]
        private MenuElements menuPrefab = null;

        [SerializeField]
        private Axes axes = new Axes();

        [SerializeField]
        private Actions actions = new Actions();


        public bool gameIsPaused { get; private set; }
        bool cursorIsBlocked = true;


        FirstPersonController m_Controller;
        MenuElements m_Menu;


        // Spawn UIElements 
        private void SpawnUIElements()
        {
            m_Menu = SpawnSingleUIElement( menuPrefab );
            m_Menu.AwakeMENU( this );

#if TOUCH_CONTROLS_KIT
            SpawnSingleUIElement( touchUIElements );
#endif

            if( FindObjectOfType<EventSystem>() == null )
            {
                new GameObject( "EventSystem", typeof( EventSystem ), typeof( StandaloneInputModule ) );
            }
        }

        // Spawn SingleUIElement
        private static T SpawnSingleUIElement<T>( T prefab ) where T : MonoBehaviour
        {
            T[] lostPrafabs = FindObjectsOfType<T>();
            int lostSize = lostPrafabs.Length;

            for( int i = 1; i < lostSize; i++ )
            {
                Destroy( lostPrafabs[ i ].gameObject );
            }

            T curretElement = ( lostSize > 0 ) ? lostPrafabs[ 0 ] : null;
            if( curretElement == null )
            {
                if( prefab != null )
                {
                    curretElement = Instantiate( prefab );
                }                    
                else
                {
                    Debug.LogError( "Error: UI Prefab is not setup." );
                }                   
            }

            return curretElement;
        }


        // Awake
        void Awake()
        {
            m_Controller = GetComponent<FirstPersonController>();

            SpawnUIElements();
            
            InputSettings.BindAction( actions.jump, EActionEvent.Down, m_Controller.Jump );
            InputSettings.BindAction( actions.crouch, EActionEvent.Down, m_Controller.Crouch );
            InputSettings.BindAction( actions.blockCursor, EActionEvent.Down, BlockCursor );
            InputSettings.BindAction( actions.unblockCursor, EActionEvent.Down, UnblockCursor );
        }

        // Start
        void Start()
        {
            GameSettings.UpdateMixerVolumes();
            m_Menu.SetActive( false );
        }

        
        // Update
        void Update()
        {
            if( updateType == EUpdateType.Update )
                InputsUpdate();
        }
        // Late Update
        void LateUpdate()
        {
            if( updateType == EUpdateType.LateUpdate )
                InputsUpdate();
        }
        // Fixed Update
        void FixedUpdate()
        {
            if( updateType == EUpdateType.FixedUpdate )
                InputsUpdate();
        }


        // Inputs Update
        private void InputsUpdate()
        {            

#if TOUCH_CONTROLS_KIT
            if( inputType == EInputType.TouchControlsKit )
                TouchKitInput();
            else
#endif
                StandaloneInput();
        }

        // Standalone Input
        private void StandaloneInput()
        {
            if( InputSettings.GetAction( actions.pause, EActionEvent.Down ) )
                Pause();

            if( gameIsPaused )
                return;

            InputSettings.RunActions();
            InputSettings.RunActionAxis();
            InputSettings.RunAxis();


            // Cursor lock
            if( cursorIsBlocked && Time.timeSinceLevelLoad > .1f )
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            runAction = InputSettings.GetAction( actions.run, EActionEvent.Press );

            moveHorizontal = InputSettings.GetAxis( axes.moveX );
            moveVertical = InputSettings.GetAxis( axes.moveY );
            lookHorizontal = InputSettings.GetAxis( axes.lookX ) * GameSettings.GetLookSensitivityByInvert_X;
            lookVertical = InputSettings.GetAxis( axes.lookY ) * GameSettings.GetLookSensitivityByInvert_Y;
        }

#if TOUCH_CONTROLS_KIT
        // TouchKit Input
        private void TouchKitInput()
        {
            if( TCKInput.CheckController( actions.pause ) && TCKInput.GetAction( actions.pause, TCKActionEvent.Down ) )
                Pause();

            if( gameIsPaused )
                return;

            runAction = ( TCKInput.CheckController( actions.run ) && TCKInput.GetAction( actions.run, TCKActionEvent.Press ) );

            if( TCKInput.CheckController( actions.jump ) && TCKInput.GetAction( actions.jump, TCKActionEvent.Down ) )
                m_Controller.Jump();
            if( TCKInput.CheckController( actions.crouch ) && TCKInput.GetAction( actions.crouch, TCKActionEvent.Down ) )
                m_Controller.Crouch();

            if( TCKInput.CheckController( axes.moveJoystick ) )
            {
                moveHorizontal = Mathf.Clamp( TCKInput.GetAxis( axes.moveJoystick, TCKAxisType.Horizontal ), -1f, 1f );
                moveVertical = runAction ? 1f : Mathf.Clamp( TCKInput.GetAxis( axes.moveJoystick, TCKAxisType.Vertical ), -1f, 1f );
            }

            if( TCKInput.CheckController( axes.lookTouchpad ) )
            {
                lookHorizontal = TCKInput.GetAxis( axes.lookTouchpad, TCKAxisType.Horizontal ) * GameSettings.GetLookSensitivityByInvert_X;
                lookVertical = TCKInput.GetAxis( axes.lookTouchpad, TCKAxisType.Vertical ) * GameSettings.GetLookSensitivityByInvert_Y;
            }
        }
#endif


        // Bind Action
        public static void BindAction( string m_Name, EActionEvent m_Event, ActionHandler m_Handler ) {
            InputSettings.BindAction( m_Name, m_Event, m_Handler );
        }
        // Unbind Action
        public static void UnbindAction( string m_Name, EActionEvent m_Event, ActionHandler m_Handler ) {
            InputSettings.UnbindAction( m_Name, m_Event, m_Handler );
        }

        // Bind ActionAxis
        public static void BindActionByAxis( string actionName, EAxisState axisState, ActionHandler m_Handler ) {
            InputSettings.BindActionAxis( actionName, axisState, m_Handler );
        }
        // Unbind ActionAxis
        public static void UnbindActionByAxis( string actionName, EAxisState axisState, ActionHandler m_Handler ) {
            InputSettings.UnbindActionAxis( actionName, axisState, m_Handler );
        }

        // Bind Axis
        public static void BindAxis( string m_Name, AxisHandler m_Handler ) {
            InputSettings.BindAxis( m_Name, m_Handler );
        }
        // Unbind Axis
        public static void UnbindAxis( string m_Name, AxisHandler m_Handler ) {
            InputSettings.UnbindAxis( m_Name, m_Handler );
        }


        // Get Action
        public static bool GetAction( string m_Name, EActionEvent m_Event ) {
            return InputSettings.GetAction( m_Name, m_Event );
        }

        // Get ActionAxis
        public static bool GetActionByAxis( string actionName, EAxisState axisState ) {
            return InputSettings.GetActionAxis( actionName, axisState );
        }

        // Get Axis
        public static float GetAxis( string m_Name ) {
            return InputSettings.GetAxis( m_Name );
        }


        // Block Cursor
        public void BlockCursor()
        {
            cursorIsBlocked = true;
        }
        // Unblock Cursor
        public void UnblockCursor()
        {
            cursorIsBlocked = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Pause
        public void Pause()
        {
            gameIsPaused = !gameIsPaused;
            Time.timeScale = gameIsPaused ? 0f : 1f;
            m_Menu.SetActive( gameIsPaused );

#if TOUCH_CONTROLS_KIT
            TCKInput.SetActive( !gameIsPaused );
#endif
        }

        // PlayerDie
        public void PlayerDie()
        {
            m_Controller.PlayerDie();
            m_Menu.SetActive( true );

#if TOUCH_CONTROLS_KIT
            TCKInput.SetActive( false );
#endif
        }


        // move Horizontal 
        internal float moveHorizontal { get; private set; }
        // move Vertical 
        internal float moveVertical { get; private set; }

        // look Horizontal 
        internal float lookHorizontal { get; private set; }
        // look Vertical 
        internal float lookVertical { get; private set; }

        // run Action 
        internal bool runAction { get; private set; }
    };
}