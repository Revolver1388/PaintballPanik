using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 


namespace SA.TB{
    public class GameManager : MonoBehaviour
    {

        public List<UnitController> units = new List<UnitController>();  
        public UnitController curUnit;
        public bool movingPlayer;
        bool hasPath;


        Node curNode;
        Node prevNode;

        List<PathInfo> redInfo; 
        List<PathInfo> pathInfo;

        public Material blue;
        public Material red;

        LineRenderer pathRed; 
        LineRenderer pathBlue;
        GridBase grid; 

        public void Init()
        {
            grid = GridBase.singleton;

           
            GameObject go = new GameObject();
            go.name = "line vis blue";
            pathBlue = go.AddComponent<LineRenderer>();
            pathBlue.startWidth = 0.2f;
            pathBlue.endWidth = 0.2f;
            pathBlue.material = blue; 

            GameObject go2 = new GameObject();
            go.name = "line vis red";
            pathRed = go2.AddComponent<LineRenderer>();
            pathRed.startWidth = 0.2f;
            pathRed.endWidth = 0.2f;
            pathRed.material = red;


            for (int i = 0; i < units.Count; i++)
            {
                units[i].Init();
            }
        }

        private void Update()
        {
            if (GridBase.singleton.isInit == false)
                return;


            FindNode();


          
                //checks the node to see if it is occupied, if so it selects the unit, otherwise it makes it the target node
                if(Input.GetMouseButton(0))
                {
                    UnitController uc = UnitHasNode(curNode);

                    if(curUnit != null)
                    {
                        if(curUnit.isMoving)
                        {
                            return;
                        }
                    }

                    if(uc == null && curUnit != null )
                    {

                        if (hasPath && pathInfo != null)
                        {
                            curUnit.AddPath(pathInfo);
                        }

                    }
                   
                    else
                    {
                        curUnit = uc; 
                    }
                }


            if (curUnit.isMoving)
            {
                return;
            }

            if(curUnit == null)
            {
                return;
            }




            #region Pathfinder

            if (prevNode != curNode)
            {
                PathfindMaster.GetInstance().RequestPathfind(curUnit.node, curNode, PathfinderCallback);
            }

            prevNode = curNode;

            if (hasPath && pathInfo != null)
            {
                if (pathInfo.Count > 0)
                {
                    pathBlue.positionCount = pathInfo.Count;

                    for (int i = 0; i < pathInfo.Count; i++)
                    {

                        pathBlue.SetPosition(i, pathInfo[i].targetPosition);
                    }
                }

                if (redInfo != null)
                {

                    if (redInfo.Count > 1)
                    {
                        pathRed.positionCount = redInfo.Count;
                        pathRed.gameObject.SetActive(true);
                        for (int i = 0; i < redInfo.Count; i++)
                        {

                            pathRed.SetPosition(i, redInfo[i].targetPosition);
                        }
                    }
                }
                else
                {
                    pathRed.gameObject.SetActive(false);
                }

            }

             #endregion



        }


        void PathfinderCallback(List<Node> p)
        {
            int curAp = curUnit.actionPoints;
            int needAp = 0;
            List<PathInfo> tp = new List<PathInfo>();
            PathInfo p1 = new PathInfo();
            p1.ap = 0;
            p1.targetPosition = curUnit.transform.position;
            tp.Add(p1);

            List<PathInfo> red = new List<PathInfo>();

            int baseAction = 2;
            int diag = 3; 
            //int diag = Mathf.FloorToInt(baseAction/2); 

            for (int i = 0; i < p.Count; i++)
            {
                Node n = p[i];
                Vector3 wp = grid.GetWorldCoordinatedFromNode(n.x, n.y, n.z);

                //direction
                Vector3 dir = Vector3.zero;

                if (i == 0)
                    dir = GetPathDirection(curUnit.node, n);
                else
                    dir = GetPathDirection(p[i-1], p[i]);

                if(dir.x != 0 && dir.z != 0)
                {
                    baseAction = diag; 
                }

                needAp += baseAction;

                PathInfo pi = new PathInfo();
                pi.ap = baseAction;
                pi.targetPosition = wp; 

                if(needAp > curAp)
                {
                    if(red.Count == 0)
                    {
                        red.Add(tp[i]);
                    }

                    red.Add(pi);
                }

                else
                {
                    tp.Add(pi);
                }
            }

            pathInfo = tp;
            redInfo = red; 
            hasPath = true; 
        }



        void FindNode()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
               curNode = GridBase.singleton.GetNodeFromWorldPosition(hit.point);
            }
        }

        //checks the nodes to see if it is occupied by a unit. If it is, it returns the unit. 

        UnitController UnitHasNode(Node n)
        {
            for (int i = 0; i < units.Count; i++)
            {
                Node un = units[i].node;


                if(un.x == n.x && un.y == n.y && un.z == n.z)
                {
                    return units[i];
                }


            }

            return null;

        }


        Vector3 GetPathDirection(Node n1, Node n2)
        {
            Vector3 dir = Vector3.zero;
            dir.x = n2.x - n1.x;
            dir.y = n2.y - n1.y;
            dir.z = n2.z - n1.z;

            return dir;

        }


        public static GameManager singleton;

        private void Awake()
        {
            singleton = this; 
        }
    }


    [System.Serializable]
    public class PathInfo
    {
        public int ap;
        public Vector3 targetPosition; 

    }
}
