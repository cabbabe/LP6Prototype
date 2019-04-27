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
    public class SFPCEditorStyle
    {
        static SFPCEditorStyle m_Instance;
        public static SFPCEditorStyle Get { get { if( m_Instance == null ) m_Instance = new SFPCEditorStyle(); return m_Instance; } }


        public readonly GUIContent iconToolbarMinus;

        public readonly GUIStyle
            buttonLeft, buttonMid, buttonRight
            , rlFooterButton
            , richLabel
            , centeredLabel
            , headLabel, centeredHeadLabel
            , largeBoldLabel
            , area
            , link
            , line
            , richBoldFoldout, largeFoldout;


        // Constructor
        public SFPCEditorStyle()
        {
            iconToolbarMinus = EditorGUIUtility.IconContent( "Toolbar Minus", "|Remove this from list" );

            var guiSkin = GUI.skin;

            string btnStyleName = guiSkin.button.name;
            buttonLeft = guiSkin.FindStyle( btnStyleName + "left" );
            buttonMid = guiSkin.FindStyle( btnStyleName + "mid" );
            buttonRight = guiSkin.FindStyle( btnStyleName + "right" );

            rlFooterButton = "RL FooterButton";

            richLabel = new GUIStyle( EditorStyles.label );
            richLabel.richText = true;

            centeredLabel = new GUIStyle( richLabel );
            centeredLabel.alignment = TextAnchor.MiddleCenter;

            area = new GUIStyle();
            area.padding = new RectOffset( 10, 10, 10, 10 );

            headLabel = new GUIStyle( richLabel );
            headLabel.fontSize = 21;
            headLabel.normal.textColor = Color.grey;

            centeredHeadLabel = new GUIStyle( headLabel );
            centeredHeadLabel.alignment = TextAnchor.MiddleCenter;

            Color32 greenStyle = new Color32( 16, 144, 144, 255 );
            link = new GUIStyle();
            link.richText = true;
            link.fontSize = 16;
            link.fontStyle = FontStyle.Bold;
            link.normal.textColor = greenStyle;

            line = new GUIStyle( guiSkin.box );
            line.border.top = line.margin.top = line.padding.top = 1;
            line.border.bottom = line.margin.bottom = line.padding.bottom = 1;


            largeBoldLabel = new GUIStyle( EditorStyles.largeLabel );
            largeBoldLabel.fontStyle = FontStyle.Bold;
            largeBoldLabel.alignment = TextAnchor.MiddleCenter;
            largeBoldLabel.fontSize = 14;
            largeBoldLabel.normal.textColor = greenStyle;

            richBoldFoldout = new GUIStyle( EditorStyles.foldout );
            richBoldFoldout.richText = true;
            richBoldFoldout.fontStyle = FontStyle.Bold;

            largeFoldout = new GUIStyle( EditorStyles.foldout );
            largeFoldout.fontStyle = FontStyle.Bold;
            largeFoldout.fontSize = 13;
            largeFoldout.stretchWidth = false;
            largeFoldout.onActive.textColor = greenStyle;
            largeFoldout.onFocused.textColor = greenStyle;
            largeFoldout.onHover.textColor = greenStyle;
            largeFoldout.onNormal.textColor = greenStyle;
            largeFoldout.active.textColor = greenStyle;
            largeFoldout.focused.textColor = greenStyle;
            largeFoldout.hover.textColor = greenStyle;
            largeFoldout.normal.textColor = greenStyle;
        }
    };
}