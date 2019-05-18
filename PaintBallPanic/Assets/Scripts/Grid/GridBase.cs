using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.TB
{
    public class GridBase : MonoBehaviour
    {
        public bool isInit; 
        public int sizeX = 32;
        public int sizeY = 3;
        public int sizeZ = 32;
        public float scaleXZ = 1;
        public float scaleY = 2.3f;



        public Node[,,] grid;
        public List<YLevels> yLevels = new List<YLevels>();

        public string levelName;
        public bool saveLevel;
        public bool loadLevel;

        SaveLevelFile savedLevel; 

        public bool debugNode = true;
        public Material debugMaterial;
        public Material unWalkableMat;

        GameObject debugNodeObj;

        private void Update()
        {
            if(saveLevel)
            {
                Serialization.SaveLevel(levelName);
                saveLevel = false; 
            }
        }

        private void Start()
        {
            InitPhase();
        }

        public void InitPhase()
        {
            if (debugNode)
            {
                debugNodeObj = WorldNode();
            }
            //load level
            bool hasSavedLevel = (loadLevel) ? CheckForSavedLevel() : false;


            if (!loadLevel)
                hasSavedLevel = false; 

            if(hasSavedLevel)
            {
                sizeX = savedLevel.sizeX;
                sizeY = savedLevel.sizeY;
                sizeX = savedLevel.sizeZ;
                scaleXZ = savedLevel.scaleXZ;
                scaleY = savedLevel.scaleY;
            }


            Check();
            CreateGrid();
            GameManager.singleton.Init();


            if (hasSavedLevel == false)
                LevelManager.singleton.LoadObstacles();
            else
                LoadLevel();

            CameraManager.singleton.Init();
            isInit = true; 
        }

        bool CheckForSavedLevel()
        {
            SaveLevelFile s = Serialization.LoadLevel(levelName);

            if (s == null)
                return false;

            savedLevel = s;

                return true; 

        }

        void LoadLevel()
        {
            List<SavableNode> sn = savedLevel.savedNodes;

            for (int i = 0; i < sn.Count; i++)
            {
                grid[sn[i].x, sn[i].y, sn[i].z].ChangeNodeStatus(sn[i].isWalkable);
            }

            Debug.Log("Loaded Level");
        }

        void Check()
        {
            if (sizeX == 0)
            {
                Debug.Log("Size X is 0, assigning min");
                sizeX = 16;
            }

            if (sizeY == 0)
            {
                Debug.Log("Size Y is 0, assigning min");
                sizeY = 1;
            }

            if (sizeZ == 0)
            {
                Debug.Log("Size Z is 0, assigning min");
                sizeX = 16;
            }

            if (scaleXZ == 0)
            {
                Debug.Log("Scale XZ is 0, assgning min");
                scaleXZ = 1;
            }

            if (scaleY == 0)
            {
                Debug.Log("Scale Y is 0, assgning min");
                scaleY = 2;
            }
        }

        void CreateGrid()
        {
            grid = new Node[sizeX, sizeY, sizeZ];

            for (int y = 0; y < sizeY; y++)
            {
                YLevels ylvl = new YLevels();
                ylvl.nodeParent = new GameObject();
                ylvl.nodeParent.name = "Level " + y.ToString();
                ylvl.y = y;
                yLevels.Add(ylvl);

                //create collision
                createCollision(y);

                for (int x = 0; x < sizeX; x++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        Node n = new Node();
                        n.x = x;
                        n.y = y;
                        n.z = z;
                        n.ChangeNodeStatus(true);

                        if (debugNode)
                        {
                            Vector3 targetPosition = GetWorldCoordinatedFromNode(x, y, z);
                            GameObject go = Instantiate(debugNodeObj, targetPosition, Quaternion.identity) as GameObject;
                            go.transform.parent = ylvl.nodeParent.transform;
                            go.SetActive(true);
                            n.worldObject = go; 
                        }

                        grid[x, y, z] = n;
                    }
                }
            }
        }

        void createCollision(int y)
        {
            YLevels lvl = yLevels[y];
            GameObject go = new GameObject();
            BoxCollider box = go.AddComponent<BoxCollider>();
            box.size = new Vector3(sizeX * scaleXZ + (scaleXZ * 2), 0.2f, sizeZ * scaleXZ + (scaleXZ * 2));

            box.transform.position = new Vector3((sizeX * scaleXZ) * .5f - (scaleXZ * .5f), y * scaleY, (sizeZ * scaleXZ) * .5f - (scaleXZ * .5f));

            lvl.collisionsObj = go;
            lvl.collisionsObj.name = "lvl " + y + "collision";
        }

        public Node GetNodeFromWorldPosition(Vector3 wp)
        {
            int x = Mathf.RoundToInt(wp.x / scaleXZ);
            int y = Mathf.RoundToInt(wp.y / scaleY);
            int z = Mathf.RoundToInt(wp.z / scaleXZ);

            return GetNode(x, y, z);
        }

        public Node GetNode(int x, int y, int z)
        {
            x = Mathf.Clamp(x, 0, sizeX - 1);
            y = Mathf.Clamp(y, 0, sizeY - 1);
            z = Mathf.Clamp(z, 0, sizeZ - 1);

            return grid[x, y, z];
        }

        public Vector3 GetWorldCoordinatedFromNode(int x, int y, int z)
        {
            Vector3 r = Vector3.zero;
            r.x = x * scaleXZ;
            r.y = y * scaleY;
            r.z = z * scaleXZ;

            return r;
        }


        GameObject WorldNode()
        {
            GameObject go = new GameObject();
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(go.GetComponent<Collider>());
            quad.transform.parent = go.transform;
            quad.transform.localPosition = Vector3.zero;
            quad.transform.localEulerAngles = new Vector3(90, 0, 0);
            quad.transform.localScale = Vector3.one * 0.95f;
            quad.GetComponentInChildren<MeshRenderer>().material = debugMaterial;
            go.SetActive(false);
            return go;
        }


        public static GridBase singleton;

        void Awake()
        {
            singleton = this;
        }

    }



    [System.Serializable]
    public class YLevels
    {
        public int y;
        public GameObject nodeParent;
        public GameObject collisionsObj;
    }
}
