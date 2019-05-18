using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SA.Utilities
{
        [CustomEditor(typeof(LevelEditor))]
    public class EditorEvents : MonoBehaviour
    {
            LevelEditor lvl; 

            void onSceneGUI()
            {
                if (lvl == null)
                    lvl = LevelEditor.singleton;
                if (lvl == null)
                    return; 

                Event e = Event.current; 


                switch(e.type)
                {

                case EventType.MouseDown:
                      HandleMouse(e);
                      break; 
                case EventType.KeyDown:
                    HandleKeys(e);
                      break; 
                }
    
        
            }

        void HandleMouse(Event e)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit; 

            if(Physics.Raycast(ray,out hit, Mathf.Infinity))
            {
                //edit the node; 
            }
        }

        void HandleKeys(Event e)
        {
            if(e.keyCode == lvl.hotKeys.editMode)
            {
                lvl.editMode = !lvl.editMode;
            }
            if (e.keyCode == lvl.hotKeys.initLevel)
            {
                lvl.InitializeLevel();            }
            if (e.keyCode == lvl.hotKeys.saveLevel)
            {
                //lvl.editMode = !lvl.editMode;
            }
        }
    }


}
