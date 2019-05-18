using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.TB
{

    public class UnitController : MonoBehaviour
    {

        List<PathInfo> path;
        int pathIndex;
        float moveT;
        float actualSpeed;
        float rotationT;
        bool initLerip;
        public bool isMoving; 
        Vector3 startPosition;
        Vector3 targetPosition;
        public float walkSpeed = 2;
        public int actionPoints = 20;

        float rotaionSpeed = 8;

        Animator anim; 

        public int x1, z1; 

        public Node node
        {
            get
            { 
                return GridBase.singleton.GetNodeFromWorldPosition(transform.position);
            }
        }

        Node prevNode; 

        public void Init()
        {
            Vector3 worldPos = GridBase.singleton.GetWorldCoordinatedFromNode(x1, 0, z1);
            transform.position = worldPos;
            node.ChangeNodeStatus(false);
            anim = GetComponentInChildren<Animator>(); 


        }


        private void Update()
        {
            if (isMoving)
            {
                Moving();
                //anim.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
                anim.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
                anim.applyRootMotion = false; 
            }

            else
            {
                //anim.SetFloat("Vertical", 0, 0.4f, Time.deltaTime);
                anim.SetFloat("Vertical", 0, 0.4f, Time.deltaTime);


            }

        }

        void Moving()
        {
            if(!initLerip)
            {
                if(pathIndex == path.Count)
                {
                    isMoving = false; 
                    return; 
                }

                node.ChangeNodeStatus(true);
                moveT = 0;
                rotationT = 0;
                startPosition = this.transform.position;
                targetPosition = path[pathIndex].targetPosition;
                float distance = Vector3.Distance(startPosition, targetPosition);
                actualSpeed = walkSpeed / distance;
                initLerip = true; 
            }

            moveT += Time.deltaTime * actualSpeed; 

            if(moveT > 1)
            {
                moveT = 1;
                initLerip = false;
                RemoveAp(path[pathIndex]);
                if (pathIndex < path.Count - 1)
                {
                    pathIndex++;
                }
                else
                    isMoving = false; 
            }

            Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, moveT);
            transform.position = newPos;

            rotationT += Time.deltaTime * rotaionSpeed;

            Vector3 lookDirection = targetPosition - startPosition;
            lookDirection.y = 0; 
            if(lookDirection == Vector3.zero)
            {
                lookDirection = transform.forward; 
            }
            Quaternion targetRot = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationT);

        }

        void RemoveAp(PathInfo p)
        {
            actionPoints -= p.ap;
            node.ChangeNodeStatus(false);


        }

        public void AddPath(List<PathInfo> p)
        {
            pathIndex = 1;
            path = p;
            isMoving = true; 
        }
    }
}
