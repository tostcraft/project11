using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 180)]
    public float viewAngle;
    public GameObject goal;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public Vector3 startingPosition;

    public float activeTime;
    private float timer = 0;
    private bool active = false;
    private bool startCounting = false;
    private Vector3 dirToTargetTesting;
    private bool lookingAtTarget = false;
    

    bool findVisibleTargets()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targets.Length; i++)
        {
            Transform targetTransform = targets[i].transform;
            Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;
            dirToTargetTesting = dirToTarget;
            
            if (Vector3.Angle(transform.forward, dirToTarget) <= viewAngle)
            {
                
                float distToTarget = Vector3.Distance(targetTransform.position, transform.position);
                Debug.Log(dirToTarget + ", " + distToTarget);
                if (distToTarget <= viewRadius)
                {

                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {

                        return true;
                    }
 //                   else if(Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
//                    {
//                        startCounting = true;
//                    }

                }
/*                else if(distToTarget > viewRadius)
                {
                    startCounting = true;
                }
*/
            }
/*            else if(Vector3.Angle(Vector3.forward, dirToTarget) > viewAngle)
            {
                startCounting = true;
            }
*/            
        }
        return false;
        
        
    }

    public Vector3 directionFromAngle(float angleInDegrees, bool AngleIsGlobal)
    {
        if(!AngleIsGlobal)
        {
            angleInDegrees += gameObject.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawRay(transform.position, directionFromAngle(viewAngle, false) * viewRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, directionFromAngle(-viewAngle, false) * viewRadius);
        Gizmos.DrawRay(transform.position, dirToTargetTesting * Vector3.Distance(goal.transform.position, transform.position));
        if(active)
        {
            Gizmos.DrawRay(transform.position, goal.transform.position);
        }
        
    }

    void Update()
    {
        NavMeshAgent navmesh = GetComponent<NavMeshAgent>();
        if (findVisibleTargets() || timer < activeTime && startCounting) 
        {
            
            if (!lookingAtTarget)
            {
                transform.LookAt(goal.transform.position);
                lookingAtTarget = true;
            }
            if (Vector3.Angle(transform.forward, goal.transform.position)>=90)
            {
                navmesh.speed = 0;
            }
            else
            {
                navmesh.speed = 3.5F;
            }
            navmesh.destination = goal.transform.position;

        }
        else
        {
            if (lookingAtTarget)
            {
                startCounting = true;
            }
            timer += Time.deltaTime;
            if (timer >= activeTime)
            {
                lookingAtTarget = false;
                navmesh.destination = startingPosition;
                timer = 0;
                startCounting = false;
            }
        }

    }
}
