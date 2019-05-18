using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;


namespace SA.TB { 
    public static class Serialization 
    {
        public static void SaveLevel(string saveName)
        {

            if (string.IsNullOrEmpty(saveName))
                saveName = "default_level_name";
            SaveLevelFile saveFile = new SaveLevelFile();

            GridBase grid = GridBase.singleton;
            saveFile.sizeX = grid.sizeX;
            saveFile.sizeY = grid.sizeY;
            saveFile.sizeZ = grid.sizeZ;
            saveFile.scaleXZ = grid.scaleXZ;
            saveFile.scaleY = grid.scaleY;
            saveFile.savedNodes = NodeToSaveable(grid);


            string saveLocation = SaveLocation();
            saveLocation += saveName;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(saveLocation, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, saveFile);
            stream.Close();

            Debug.Log(saveName + " saved!");
        }

        public static SaveLevelFile LoadLevel(string loadName)
        {
            SaveLevelFile saveFile = null;

            string targetName = SaveLocation();
            targetName += loadName;

            if(!File.Exists(targetName))
            {
                Debug.Log("Can't find level " + loadName);
            }
            else
            {
                IFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(targetName, FileMode.Open);
                SaveLevelFile save = (SaveLevelFile)formatter.Deserialize(stream);
                saveFile = save;
                stream.Close();

            }

            return saveFile; 
        }
       

        public static List<SavableNode> NodeToSaveable(GridBase grid)
        {

            List<SavableNode> retVal = new List<SavableNode>();

            for (int x = 0; x < grid.sizeX; x++)
            {
                for (int y = 0; y < grid.sizeY; y++)
                {
                    for (int z = 0; z < grid.sizeZ; z++)
                    {
                        SavableNode sn = new SavableNode();
                        Node n = grid.grid[x, y, z];
                        sn.x = n.x;
                        sn.y = n.y;
                        sn.z = n.z;
                        sn.isWalkable = n.isWalkable;
                        retVal.Add(sn);
                    } 
                }
            }

            return retVal; 
        }

        static string SaveLocation()
        {
            string saveLocation = Application.streamingAssetsPath + "/Levels/";
            if(!Directory.Exists(saveLocation))
            {
                Directory.CreateDirectory(saveLocation);
            }

            return saveLocation; 
        }
    }

    [Serializable]
    public class SaveLevelFile
    {
        public int sizeX;
        public int sizeY;
        public int sizeZ;

        public float scaleXZ;
        public float scaleY; 

        public List<SavableNode> savedNodes; 
    }


    [Serializable]
    public class SavableNode
    {
        public int x, y, z;
        public bool isWalkable; 
    }

}
