﻿/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/

 
using UnityEditor;

namespace SmartFPController.Inspector
{
    [CustomEditor( typeof( CustomSurface ) )]
    public class CustomSurfaceEditor : Editor
    {
        SerializedProperty surfaceNameProp;


        // OnEnable
        void OnEnable()
        {
            surfaceNameProp = serializedObject.FindProperty( "m_SurfaceName" );
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
            EditorGUILayout.Space();
            SFPCEditorHelper.DrawStringPopup( surfaceNameProp, SurfaceDetector.allNames, "Surface" );
        }
    };
}
