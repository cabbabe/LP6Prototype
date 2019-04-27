/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using UnityEngine;
using UnityEditor;

namespace SmartFPController.Inspector
{
    [CustomEditor( typeof( FirstPersonController ) )]
    //[CanEditMultipleObjects]
    public class FirstPersonControllerEditor : Editor
    {
        private SerializedProperty
            canWalkProp, walkSpeedProp, backwardsSpeedProp, sidewaysSpeedProp, inAirSpeedProp,
            canRunProp, runSpeedProp,
            canCrouchProp, crouchSpeedProp, crouchHeightProp,
            canJumpProp, jumpForceProp,
            canClimbProp, climbingSpeedProp,
            useHeadBobProp, posForceProp, tiltForceProp,
            gravityMultiplierProp, 
            fallingDistanceToDamageProp, fallingDamageMultiplierProp, damageFunctionProp,
            stepIntervalProp,
            lookSmoothProp, maxLookAngleYProp, cameraOffsetProp;

        
        // OnEnable
        void OnEnable()
        {
            canWalkProp = serializedObject.FindProperty( "canWalk" );
            walkSpeedProp = serializedObject.FindProperty( "walkSpeed" );
            backwardsSpeedProp = serializedObject.FindProperty( "backwardsSpeed" );
            sidewaysSpeedProp = serializedObject.FindProperty( "sidewaysSpeed" );
            inAirSpeedProp = serializedObject.FindProperty( "inAirSpeed" );

            canRunProp = serializedObject.FindProperty( "canRun" );
            runSpeedProp = serializedObject.FindProperty( "runSpeed" );

            canCrouchProp = serializedObject.FindProperty( "canCrouch" );
            crouchSpeedProp = serializedObject.FindProperty( "crouchSpeed" );
            crouchHeightProp = serializedObject.FindProperty( "crouchHeight" );

            canJumpProp = serializedObject.FindProperty( "canJump" );
            jumpForceProp = serializedObject.FindProperty( "jumpForce" );

            canClimbProp = serializedObject.FindProperty( "canClimb" );
            climbingSpeedProp = serializedObject.FindProperty( "climbingSpeed" );

            useHeadBobProp = serializedObject.FindProperty( "useHeadBob" );
            posForceProp = serializedObject.FindProperty( "posForce" );
            tiltForceProp = serializedObject.FindProperty( "tiltForce" );

            gravityMultiplierProp = serializedObject.FindProperty( "gravityMultiplier" );
            fallingDistanceToDamageProp = serializedObject.FindProperty( "fallingDistanceToDamage" );
            fallingDamageMultiplierProp = serializedObject.FindProperty( "fallingDamageMultiplier" );
            damageFunctionProp = serializedObject.FindProperty( "damageFunction" );

            stepIntervalProp = serializedObject.FindProperty( "stepInterval" );

            lookSmoothProp = serializedObject.FindProperty( "lookSmooth" );
            maxLookAngleYProp = serializedObject.FindProperty( "maxLookAngleY" );
            cameraOffsetProp = serializedObject.FindProperty( "cameraOffset" );
        }


        // OnInspectorGUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ShowParameters();
            serializedObject.ApplyModifiedProperties();
        }


        // ShowParameters
        private void ShowParameters()
        {
            const float SPACE = 15f;

            SFPCEditorHelper.DrawBoolField( canWalkProp );
            SFPCEditorHelper.DrawPropertyField( walkSpeedProp, "Normal Speed", SPACE );
            SFPCEditorHelper.DrawPropertyField( backwardsSpeedProp, SPACE );
            SFPCEditorHelper.DrawPropertyField( sidewaysSpeedProp, SPACE );
            SFPCEditorHelper.DrawPropertyField( inAirSpeedProp, "InAir Speed", SPACE );
            GUI.enabled = true;

            SFPCEditorHelper.DrawBoolField( canRunProp );
            SFPCEditorHelper.DrawPropertyField( runSpeedProp, "Move Speed", SPACE );
            GUI.enabled = true;

            SFPCEditorHelper.DrawBoolField( canCrouchProp );
            SFPCEditorHelper.DrawPropertyField( crouchSpeedProp, "Move Speed", SPACE );
            SFPCEditorHelper.DrawPropertyField( crouchHeightProp, "Capsule Height", SPACE );
            GUI.enabled = true;

            SFPCEditorHelper.DrawBoolField( canJumpProp );
            SFPCEditorHelper.DrawPropertyField( jumpForceProp, "Force", SPACE );
            GUI.enabled = true;

            SFPCEditorHelper.DrawBoolField( canClimbProp );
            SFPCEditorHelper.DrawPropertyField( climbingSpeedProp, "Move Speed", SPACE );
            GUI.enabled = true;

            GUILayout.Space( 5f );
            SFPCEditorHelper.DrawBoolField( useHeadBobProp );
            SFPCEditorHelper.DrawPropertyField( posForceProp, "Pos Force", SPACE );
            SFPCEditorHelper.DrawPropertyField( tiltForceProp, "Tilt Force", SPACE );
            GUI.enabled = true;

            GUILayout.Space( 5f );
            EditorGUILayout.PropertyField( gravityMultiplierProp );
            EditorGUILayout.PropertyField( fallingDistanceToDamageProp );
            EditorGUILayout.PropertyField( fallingDamageMultiplierProp );
            EditorGUILayout.PropertyField( damageFunctionProp );

            GUILayout.Space( 5f );
            EditorGUILayout.PropertyField( stepIntervalProp );

            GUILayout.Space( 5f );
            EditorGUILayout.PropertyField( lookSmoothProp );
            EditorGUILayout.PropertyField( maxLookAngleYProp );
            EditorGUILayout.PropertyField( cameraOffsetProp );
        }
    };
}