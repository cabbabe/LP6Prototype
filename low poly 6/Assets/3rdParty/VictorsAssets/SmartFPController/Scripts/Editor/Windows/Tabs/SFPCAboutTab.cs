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
    public static class SFPCAboutTab
    {
        const string PABLISHER_URL = "http://u3d.as/5Fb";
        const string SUPPORT_URL = "http://bit.ly/vk-SupportNew";

        const string VERSION = "1.4";
        const string LOGO_NAME = "SFPCLogoBig";

        const string
             MANUAL_URL = "https://goo.gl/7MND7V"
            , FORUM_URL = "http://forum.unity.com/threads/333931"
            , CHANGELOG_URL = "http://smart-assets.org/index/0-12"
            , ASSET_URL = "http://u3d.as/h9j";


        static Texture2D m_Logo;
        private static Texture2D logo
        {
            get
            {
                if( m_Logo == null )
                {
                    m_Logo = SFPCWindow.GetImage( SFPCWindow.imagesPath + LOGO_NAME );
                }

                return m_Logo;
            }
        }


        // OnWindowGUI
        public static void OnWindowGUI()
        {
            var style = SFPCEditorStyle.Get;

            // LINK's
            using( SFPCEditorLayout.Vertical( "box", GUILayout.ExpandWidth( true ), GUILayout.ExpandHeight( true ) ) )
            {
                GUILayout.Space( 5f );

                using( SFPCEditorLayout.Vertical( style.area ) )
                {
                    GUILayout.Label( "Documentation", style.headLabel );

                    GUILayout.BeginVertical( style.area );
                    SFPCEditorHelper.DrawLink( "Online Manual", MANUAL_URL );
                    GUILayout.EndVertical();
                }

                using( SFPCEditorLayout.Vertical( style.area ) )
                {
                    GUILayout.Label( "Support, News, More Assets", style.headLabel );

                    GUILayout.BeginVertical( style.area );
                    SFPCEditorHelper.DrawLink( "Support", SUPPORT_URL );
                    GUILayout.Space( 10f );
                    SFPCEditorHelper.DrawLink( "Forum", FORUM_URL );
                    GUILayout.Space( 25f );
                    SFPCEditorHelper.DrawLink( "More Assets", PABLISHER_URL );
                    GUILayout.Space( 15f );
                    /*SFPCEditorHelper.DrawLink( "Get \"Save Game Kit\"", "http://u3d.as/Z6E" );
                    GUILayout.Space( 10f );*/
                    SFPCEditorHelper.DrawLink( "Get \"Touch Controls Kit\"", "http://u3d.as/5NP" );
                    GUILayout.EndVertical();
                }

                using( SFPCEditorLayout.Vertical( style.area ) )
                {
                    GUILayout.Label( "Release Notes", style.headLabel );

                    GUILayout.BeginVertical( style.area );
                    SFPCEditorHelper.DrawLink( "Full Changelog", CHANGELOG_URL );
                    GUILayout.EndVertical();
                }
            }

            // LOGO
            using( SFPCEditorLayout.Vertical( "box", GUILayout.Width( 280f ), GUILayout.ExpandHeight( true ) ) )
            {
                GUILayout.Space( 5f );

                GUILayout.Label( "<size=18>Smart FP Controller</size>", style.centeredLabel );

                GUILayout.Space( 5f );
                GUILayout.Label( "<size=16> Developed by Victor Klepikov\n" +
                                 "Version <b>" + VERSION + "</b> </size>", style.centeredLabel );

                EditorGUILayout.Space();
                SFPCEditorHelper.Separator();

                if( logo != null )
                {
                    GUILayout.FlexibleSpace();

                    using( SFPCEditorLayout.Horizontal() )
                    {
                        GUILayout.FlexibleSpace();

                        Rect logoRect = EditorGUILayout.GetControlRect( GUILayout.Width( logo.width ), GUILayout.Height( logo.height ) );

                        if( GUI.Button( logoRect, new GUIContent( logo, "Open AssetStore Page" ), EditorStyles.label ) )
                        {
                            Application.OpenURL( ASSET_URL );
                        }

                        EditorGUIUtility.AddCursorRect( logoRect, MouseCursor.Link );

                        GUILayout.FlexibleSpace();
                    }

                    GUILayout.FlexibleSpace();
                }
                else
                {
                    GUILayout.Label( "<size=15>Logo not found</size> \n" + LOGO_NAME, style.centeredLabel );
                }
            }
        }
    };
}
