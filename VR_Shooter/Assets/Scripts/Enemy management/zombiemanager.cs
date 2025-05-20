using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombiemanager : MonoBehaviour
{
    public Camera frustum;
    public LayerMask mask;
    public Transform target;

    public BlackBoard blackboard;
    public float followSpeed = 5f;



    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
        if (target != null )
        {
            GetComponent<NavMeshAgent>().SetDestination(target.position);

        }
        else
        {
            DetectTarget();

        }

    }



   public void DetectTarget()
    {


        Collider[] colliders = Physics.OverlapSphere(transform.position, frustum.farClipPlane, mask);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustum);

        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject && GeometryUtility.TestPlanesAABB(planes, col.bounds))
            {
                RaycastHit hit;
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = (col.transform.position - transform.position).normalized;
                ray.origin = ray.GetPoint(frustum.nearClipPlane);

                if (Physics.Raycast(ray, out hit, frustum.farClipPlane, mask))
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        GetComponentInParent<BlackBoard>().Broadcast(hit.collider.gameObject.transform);
                    }
                
            }
        }

        

    }

   public void FollowTarget(Transform Target)
    {

        target = Target;

        
    }
    




}
