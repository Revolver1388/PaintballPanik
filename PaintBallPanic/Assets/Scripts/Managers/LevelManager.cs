using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.Level;

namespace SA.TB
{
    public class LevelManager : MonoBehaviour
    {
        public List<Obstacle> obstacles = new List<Obstacle>();
    

        public void LoadObstacles()
        {
            Obstacle[] allObstacle = GameObject.FindObjectsOfType<Obstacle>();
            obstacles.AddRange(allObstacle);

            GridBase grid = GridBase.singleton;

            foreach (Obstacle ob in obstacles)
            {
                BoxCollider bx = ob.mainRenderer.gameObject.AddComponent<BoxCollider>();

                float halfX = bx.size.x * 0.5f;
                float halfY = bx.size.y * 0.5f;
                float halfZ = bx.size.z * 0.5f;

                Vector3 center = ob.mainRenderer.bounds.center;
                Vector3 from = ob.mainRenderer.bounds.min;
                from.y = 0;
                Vector3 to = ob.mainRenderer.bounds.max;
                to.y = 0;

                int stepX = Mathf.CeilToInt(Mathf.Abs(from.x - to.x) / grid.scaleXZ);
                int stepZ = Mathf.CeilToInt(Mathf.Abs(from.z - to.z) / grid.scaleXZ);


                for (int x = 0; x < stepX; x++)
                {
                    for (int z = 0; z < stepZ; z++)
                    {
                        Vector3 targetPosition = from;
                        targetPosition.x += grid.scaleXZ * x;
                        targetPosition.z += grid.scaleXZ * z;

                        //targetPosition.y = ob.mainRenderer.transform.position.y;

                        Vector3 p = ob.mainRenderer.transform.InverseTransformPoint(targetPosition) - bx.center;
                        targetPosition.y = 0; 

                        if(p.x < halfX &&  p.y < halfY && p.z < halfZ 
                           && p.x > -halfX && p.y < -halfY && p.z < -halfZ)
                        {
                            Node n = grid.GetNodeFromWorldPosition(targetPosition);
                            n.ChangeNodeStatus(false);
                        }
                    }
                }

            }
        }

        public static LevelManager singleton; 


        void Awake()
        {
            singleton = this; 
        }
    }
}