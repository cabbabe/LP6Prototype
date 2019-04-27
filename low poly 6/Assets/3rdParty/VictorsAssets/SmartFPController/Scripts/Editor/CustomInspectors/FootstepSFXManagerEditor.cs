/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace SmartFPController.Inspector
{
    [CustomEditor( typeof( FootstepSFXManager ) )]
    public class FootstepSFXManagerEditor : Editor
    {
        private SerializedProperty surfacesArray;

        private ReorderableList fsGenericList;
        private ReorderableList[] footstepSoundsList = new ReorderableList[ 0 ];


        bool surfacesSFo;
        int selection;
        static readonly string[] stateNames = { "Generic", "Special" };


        // OnEnable
        void OnEnable()
        {
            SerializedProperty footstepSoundsArray = serializedObject.FindProperty( "generic" ).FindPropertyRelative( "footstepSounds" );
            fsGenericList = new ReorderableList( serializedObject, footstepSoundsArray, true, true, true, true );

            surfacesArray = serializedObject.FindProperty( "surfaces" );
        }


        // OnInspectorGUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ShowParameters();
            serializedObject.ApplyModifiedProperties();
        }

        // Show Parameters
        private void ShowParameters()
        {
            GUILayout.BeginVertical( "Box", GUILayout.ExpandWidth( true ) );
            GUILayout.Space( 5f );

            surfacesSFo = System.Convert.ToBoolean( GUILayout.Toolbar( System.Convert.ToInt32( surfacesSFo ), stateNames, GUILayout.Height( 20f ) ) );

            GUILayout.Space( 5f );

            if( surfacesSFo )
            {
                // Surface index & Minimum decrement to show pain screen
                string[] surfacesNames = SurfaceDetector.allNames;
                var arrCmd = SFPCEditorHelper.DrawArrayControls( surfacesArray, surfacesNames.Length, ref selection );

                int surfacesSize = surfacesArray.arraySize;
                if( surfacesSize > 0 )
                {
                    SerializedProperty surfacesElement = surfacesArray.GetArrayElementAtIndex( selection );
                    SerializedProperty footstepSoundsArray = surfacesElement.FindPropertyRelative( "footstepSounds" );

                    if( surfacesSize != footstepSoundsList.Length )
                        footstepSoundsList = new ReorderableList[ surfacesSize ];

                    if( footstepSoundsList[ selection ] == null )
                        footstepSoundsList[ selection ] = new ReorderableList( serializedObject, footstepSoundsArray );

                    GUILayout.Space( 5f );
                    SFPCEditorHelper.DrawStringPopup( surfacesElement.FindPropertyRelative( "name" ), surfacesNames, "Surface Type" );
                    GUILayout.Space( 10f );

                    SFPCEditorHelper.ShowSFXPropertyAndPlayButton( serializedObject, surfacesElement.FindPropertyRelative( "jumpingSFX" ) );
                    SFPCEditorHelper.ShowSFXPropertyAndPlayButton( serializedObject, surfacesElement.FindPropertyRelative( "landingSFX" ) );
                    SFPCEditorHelper.ShowSFXListAndPlayButton( serializedObject, footstepSoundsList[ selection ], "Footstep Sounds" );
                }

                // Actions
                if( arrCmd.type == SFPCEditorHelper.ArrayCmd.EType.Add )
                {
                    arrCmd.newElement.FindPropertyRelative( "name" ).stringValue = surfacesNames[ selection ];

                    arrCmd.newElement.FindPropertyRelative( "jumpingSFX" ).objectReferenceValue = null;
                    arrCmd.newElement.FindPropertyRelative( "landingSFX" ).objectReferenceValue = null;
                    arrCmd.newElement.FindPropertyRelative( "footstepSounds" ).ClearArray();
                }
            }
            else
            {
                SerializedProperty surfacesElement = serializedObject.FindProperty( "generic" );
                SFPCEditorHelper.ShowSFXPropertyAndPlayButton( serializedObject, surfacesElement.FindPropertyRelative( "jumpingSFX" ) );
                SFPCEditorHelper.ShowSFXPropertyAndPlayButton( serializedObject, surfacesElement.FindPropertyRelative( "landingSFX" ) );
                SFPCEditorHelper.ShowSFXListAndPlayButton( serializedObject, fsGenericList, "Footstep Sounds" );
            }

            GUILayout.Space( 5f );
            GUILayout.EndVertical();
        }
    };
}