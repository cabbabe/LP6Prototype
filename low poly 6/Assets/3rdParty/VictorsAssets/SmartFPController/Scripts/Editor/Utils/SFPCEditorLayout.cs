/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using System;
using UnityEngine;

namespace SmartFPController.Inspector
{
    public struct SFPCEditorLayout : IDisposable
    {
        enum ELayoutMode : byte
        {
            Horizontal,
            Vertical,
            ScrollView
        }

        readonly ELayoutMode m_LayoutMode;


        // Constructor
        private SFPCEditorLayout( ELayoutMode mode, GUIStyle style, params GUILayoutOption[] options )
        {
            m_LayoutMode = mode;

            switch( mode )
            {
                case ELayoutMode.Horizontal:
                    GUILayout.BeginHorizontal( style, options );
                    break;
                case ELayoutMode.Vertical:
                    GUILayout.BeginVertical( style, options );
                    break;

                default:
                    break;
            }
        }
        // Constructor
        private SFPCEditorLayout( ref Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options )
        {
            m_LayoutMode = ELayoutMode.ScrollView;
            scrollPosition = GUILayout.BeginScrollView( scrollPosition, style, options );
        }


        // Horizontal
        public static SFPCEditorLayout Horizontal( params GUILayoutOption[] options )
        {
            return Horizontal( GUIStyle.none, options );
        }
        // Horizontal
        public static SFPCEditorLayout Horizontal( GUIStyle style, params GUILayoutOption[] options )
        {
            return new SFPCEditorLayout( ELayoutMode.Horizontal, style, options );
        }

        // Vertical
        public static SFPCEditorLayout Vertical( params GUILayoutOption[] options )
        {
            return Vertical( GUIStyle.none, options );
        }
        // Vertical
        public static SFPCEditorLayout Vertical( GUIStyle style, params GUILayoutOption[] options )
        {
            return new SFPCEditorLayout( ELayoutMode.Vertical, style, options );
        }

        // ScrollView
        public static SFPCEditorLayout ScrollView( ref Vector2 scrollPosition, params GUILayoutOption[] options )
        {
            return ScrollView( ref scrollPosition, GUIStyle.none, options );
        }
        // ScrollView
        public static SFPCEditorLayout ScrollView( ref Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options )
        {
            return new SFPCEditorLayout( ref scrollPosition, style, options );
        }


        // Dispose
        void IDisposable.Dispose()
        {
            switch( m_LayoutMode )
            {
                case ELayoutMode.Horizontal:
                    GUILayout.EndHorizontal();
                    break;
                case ELayoutMode.Vertical:
                    GUILayout.EndVertical();
                    break;
                case ELayoutMode.ScrollView:
                    GUILayout.EndScrollView();
                    break;

                default:
                    break;
            }
        }
    };
}
