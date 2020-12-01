using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;


public class customAIMoveScript : MonoBehaviour
{
    Seeker seeker;

    Path pathToFollow;


    public Transform target;

    GameObject graphParent;

    GameObject targetNode;

    bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.name);

        seeker = GetComponent<Seeker>();

        targetNode = GameObject.FindGameObjectWithTag("targetnode");

        graphParent = GameObject.Find("PointGraphObject");

        graphParent.GetComponent<AstarPath>().Scan();

        pathToFollow = seeker.StartPath(transform.position, target.position, pathCompleted);

        StartCoroutine(moveTarget());

        StartCoroutine(updateGraph());

        StartCoroutine(moveTowardsPathAI(this.transform));
    }

    //the code that is going to move my target.
    IEnumerator moveTarget()
    {
        List<Vector3> positions = new List<Vector3>();
        //target's current position
        positions.Add(target.position);

        positions.Add(new Vector3(target.position.x, -target.position.y));
    
        StartCoroutine(moveTowardsPath(target.transform, positions));

        yield return null;

    }

    void pathCompleted(Path p)
    {
        pathToFollow = p;
    }

    IEnumerator updateGraph()
    {
        while(true)
        { 
            
            targetNode.transform.position = target.position;
            graphParent.GetComponent<AstarPath>().Scan();
            
            
            yield return null;
            
        }

    }

    IEnumerator moveTowardsPath(Transform t, List<Vector3> points)
    {
        foreach (Vector3 position in points)
        {
            while (Vector3.Distance(t.position, position) > 0.5f)
            {
                t.position = Vector3.MoveTowards(t.position, position, 1f);
                Debug.Log(position);/**/
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return null;
    }


    IEnumerator moveTowardsPathAI(Transform t)
    {
        
        while (true)
        {
            
            List<Vector3> posns = pathToFollow.vectorPath;
            for (int counter = 0; counter < posns.Count;counter++)
            {
                while(Vector3.Distance(t.position,posns[counter])>0.5f)
                {
                    t.position = Vector3.MoveTowards(t.position, posns[counter], 1f);
                         
                    pathToFollow = seeker.StartPath(transform.position, target.position, pathCompleted);
                    yield return pathToFollow.IsDone();
                    posns = pathToFollow.vectorPath;
                    
                    yield return new WaitForSeconds(0.2f);
                }

            }
            yield return null;
        }
    }

void Update()
{
       
}
}


