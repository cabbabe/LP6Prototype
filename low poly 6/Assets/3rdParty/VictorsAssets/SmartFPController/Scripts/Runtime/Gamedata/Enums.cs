/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


namespace SmartFPController
{
    // Using for "InputManager.cs"
    public enum EUpdateType 
    { 
        Update, 
        LateUpdate, 
        FixedUpdate,
        OFF
    };

    // Using for "InputManager.cs"
    public enum EActionEvent 
    { 
        Down, 
        Press, 
        Up
    };

    // Using for "InputManager.cs"
    public enum EAxisType
    {
        Unity,
        Custom,
        Mixed
    };

    // Using for "InputManager.cs"
    public enum EActionType
    {
        KeyCode,
        Axis,        
        Mixed
    };

    // Using for "InputManager.cs"
    public enum EAxisState
    {
        NONE,
        PositiveDown, PositivePress, PositiveUp,
        NegativeDown, NegativePress, NegativeUp
    };

    // Using for "InputManager.cs"
    public enum EAxisEventsClamp
    {
        All,
        OnlyPositive,
        OnlyNegative
    };

    // Using for "InputManager.cs"
    public enum EAxisSource
    {
        CustomAxis,
        UnityInput
    };
    
    // Setter VolumeType
    public enum EVolumeType
    {
        Master,
        Music,
        SFX,
        Voice
    };
}
