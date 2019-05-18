using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.TB;


 namespace SA.Utilities{

    [ExecuteInEditMode]
    public class LevelEditor : MonoBehaviour
    {
        public bool initLevl; 
        public bool editMode;
        public bool saveLevel; 
        public HotKeys hotKeys;

        public string levelName; 
        public static LevelEditor singleton;

        private void Update()
        {
            if (singleton == null)
                singleton = this; 

            if(initLevl == false)
            {
                editMode = false;
            }


            if(editMode)
            {
                //do stuff
            }
        }


        public void InitializeLevel()
        {
            GridBase.singleton.levelName = levelName;
            GridBase.singleton.InitPhase();
        }
    }

    [System.Serializable]
    public class HotKeys
    {
        public KeyCode editMode = KeyCode.Alpha1;
        public KeyCode initLevel = KeyCode.Alpha2;
        public KeyCode saveLevel = KeyCode.Alpha5;
    }
}

