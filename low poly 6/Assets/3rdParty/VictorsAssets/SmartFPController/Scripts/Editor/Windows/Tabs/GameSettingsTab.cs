﻿/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using UnityEngine;
using UnityEditor;

namespace SmartFPController.Inspector
{
    public static class GameSettingsTab
    {
        private static string MAIN_DATABASE_PATH { get { return SFPCWindow.mainDirectory + "/GameSettings.asset"; } }
        private static string TMP_DATABASE_PATH { get { return SFPCWindow.mainDirectory + "/tmp/GameSettingsTMP.asset"; } }

        //
        private static SerializedObject serializedObject = null;
        private static SerializedProperty
            invertLookXProp, invertLookYProp,
            lookSensitivityProp,
            masterVolumeProp, sfxVolumeProp, musicVolumeProp, voiceVolumeProp,
            masterMixerProp, sfxOutputProp, musicOutputProp, voiceOutputProp;


        // Load CurrentAssetFile
        private static GameSettings LoadAssetFile( string path )
        {
            GameSettings currentFile = AssetDatabase.LoadAssetAtPath( path, typeof( GameSettings ) ) as GameSettings;

            if( currentFile == null )
            {
                currentFile = ScriptableObject.CreateInstance<GameSettings>();
                AssetDatabase.CreateAsset( currentFile, path );
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            return currentFile;
        }

        // Save CopyAssetFile
        private static void SaveCopyAssetFile( string copyFrom, string copyTo )
        {
            if( copyFrom == MAIN_DATABASE_PATH )
                LoadAssetFile( copyFrom );

            AssetDatabase.DeleteAsset( copyTo );
            AssetDatabase.CopyAsset( copyFrom, copyTo );
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        // Setup Tab
        internal static void SetupTab()
        {
            if( serializedObject == null )
                SaveCopyAssetFile( MAIN_DATABASE_PATH, TMP_DATABASE_PATH );

            serializedObject = new SerializedObject( LoadAssetFile( TMP_DATABASE_PATH ) );
            
            invertLookXProp = serializedObject.FindProperty( "invertLookX" );
            invertLookYProp = serializedObject.FindProperty( "invertLookY" );
            lookSensitivityProp = serializedObject.FindProperty( "lookSensitivity" );
            masterVolumeProp = serializedObject.FindProperty( "masterVolume" );
            sfxVolumeProp = serializedObject.FindProperty( "sfxVolume" );
            musicVolumeProp = serializedObject.FindProperty( "musicVolume" );
            voiceVolumeProp = serializedObject.FindProperty( "voiceVolume" );
            masterMixerProp = serializedObject.FindProperty( "masterMixer" );
            sfxOutputProp = serializedObject.FindProperty( "sfxOutput" );
            musicOutputProp = serializedObject.FindProperty( "musicOutput" );
            voiceOutputProp = serializedObject.FindProperty( "voiceOutput" );
        }

        // Reload Settings
        internal static void ReloadSettings()
        {
            SaveCopyAssetFile( MAIN_DATABASE_PATH, TMP_DATABASE_PATH );
            FullReset();
            SetupTab();
        }

        // Save Settings
        internal static void SaveSettings()
        {
            SaveCopyAssetFile( TMP_DATABASE_PATH, MAIN_DATABASE_PATH );
        }


        // OnWindowGUI
        internal static void OnWindowGUI()
        {
            // BEGIN
            serializedObject.Update();
            // BEGIN

            ShowSide();

            // END
            serializedObject.ApplyModifiedProperties();
            // END
        }

        // Show Side
        private static void ShowSide()
        {
            using( SFPCEditorLayout.Vertical() )
            {
                using( new SFPCEditorChangeCheck( SFPCWindow.SetDirtyData ) )
                {
                    DrawPanel( "Gameplay", DrawGameplayPanel );
                    DrawPanel( "Audio", DrawSoundPanel );
                }
            }
        }


        // DrawPanel
        static void DrawPanel( string label, System.Action OnDraw )
        {
            using( SFPCEditorLayout.Vertical( "box", GUILayout.ExpandHeight( true ) ) )
            {
                GUILayout.Space( 5f );
                GUILayout.Label( label, SFPCEditorStyle.Get.centeredHeadLabel );
                GUILayout.Space( 10f );
                OnDraw.Invoke();
            }
        }


        // Show GameplayTab
        private static void DrawGameplayPanel()
        {
            DrawCenteredPropertyField( invertLookXProp );
            DrawCenteredPropertyField( invertLookYProp );
            DrawCenteredPropertyField( lookSensitivityProp );
        }
        // Show SoundTab
        private static void DrawSoundPanel()
        {
            DrawCenteredPropertyField( masterVolumeProp );
            DrawCenteredPropertyField( masterMixerProp );

            GUILayout.Space( 5f );

            DrawCenteredPropertyField( musicVolumeProp );
            DrawCenteredPropertyField( musicOutputProp );

            GUILayout.Space( 5f );

            DrawCenteredPropertyField( sfxVolumeProp );
            DrawCenteredPropertyField( sfxOutputProp );

            GUILayout.Space( 5f );

            DrawCenteredPropertyField( voiceVolumeProp );
            DrawCenteredPropertyField( voiceOutputProp );
        }


        // Draw Centered PropertyField
        static void DrawCenteredPropertyField( SerializedProperty property )
        {
            const float centerSpace = 100f;

            using( SFPCEditorLayout.Horizontal() )
            {
                GUILayout.Space( centerSpace );

                if( property.propertyType == SerializedPropertyType.Enum )
                {
                    SFPCEditorHelper.DrawEnumAsToolbar( property );
                }
                else
                {
                    EditorGUILayout.PropertyField( property );
                }

                GUILayout.Space( centerSpace );
            }
        }


        // FullReset
        internal static void FullReset()
        {
            serializedObject = null;

            invertLookXProp = invertLookYProp = lookSensitivityProp = null;
            masterVolumeProp = sfxVolumeProp = musicVolumeProp = voiceVolumeProp = null;
            masterMixerProp = sfxOutputProp = musicOutputProp = voiceOutputProp = null;
        }
    };
}