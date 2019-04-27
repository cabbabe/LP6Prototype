/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using System;
using UnityEditor;

namespace SmartFPController.Inspector
{
    public sealed class SFPCEditorChangeCheck : IDisposable
    {
        public Action OnChangeCheck = () => { };


        // Constructor
        public SFPCEditorChangeCheck()
        {
            EditorGUI.BeginChangeCheck();
        }

        // Constructor
        public SFPCEditorChangeCheck( Action OnChange )
        {
            OnChangeCheck = OnChange;
            EditorGUI.BeginChangeCheck();
        }

        
        // Dispose
        public void Dispose()
        {
            if( EditorGUI.EndChangeCheck() )
            {
                OnChangeCheck.Invoke();
            }
        }
    };
}
