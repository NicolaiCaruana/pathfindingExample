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



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.name);

        seeker = GetComponent<Seeker>();


        //node target
        targetNode = GameObject.FindGameObjectWithTag("targetnode");

        graphParent = GameObject.Find("PointGraphObject");

        graphParent.GetComponent<AstarPath>().Scan();

        pathToFollow = seeker.StartPath(transform.position, target.position);

        StartCoroutine(moveTarget());

        StartCoroutine(updateGraph());

        StartCoroutine(moveTowardsEnemy(this.transform));
    }

    //the code that is going to move my target.
    IEnumerator moveTarget()
    {
        List<Vector3> positions = new List<Vector3>();
        //target's current position
        positions.Add(target.position);

        positions.Add(new Vector3(target.position.x, -target.position.y));
    
        StartCoroutine(moveTarget(target.transform, positions,true));

       

        yield return null;

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

    IEnumerator moveTarget(Transform t, List<Vector3> points,bool loop)
    {
        if (loop)
        {
            while (true)
            {
                List<Vector3> forwardpoints = points;
                
                foreach (Vector3 position in forwardpoints)
                {
                    while (Vector3.Distance(t.position, position) > 0.5f)
                    {
                        t.position = Vector3.MoveTowards(t.position, position, 1f);
                        Debug.Log(position);/**/
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                forwardpoints.Reverse();
                yield return null;
                
            }
        } else
        {
            foreach (Vector3 position in points)
            {
                while (Vector3.Distance(t.position, position) > 0.5f)
                {
                    t.position = Vector3.MoveTowards(t.position, position, 1f);
                    /**/
                    yield return new WaitForSeconds(0.2f);
                }
            }
            yield return null;
        }

       
    }


    IEnumerator moveTowardsEnemy(Transform t)
    {
        
        while (true)
        {
            
            List<Vector3> posns = pathToFollow.vectorPath;
            Debug.Log("Positions Count: " + posns.Count);
            for (int counter = 0; counter < posns.Count;counter++)
            {
               // Debug.Log("Distance: " + Vector3.Distance(t.position, posns[counter]));
                while(Vector3.Distance(t.position,posns[counter]) >= 0.5f)
                {
                    t.position = Vector3.MoveTowards(t.position, posns[counter], 1f);
                         
                    pathToFollow = seeker.StartPath(t.position, target.position);
                    yield return seeker.IsDone();
                    posns = pathToFollow.vectorPath;
                    Debug.Log("@"+t.position +" "+target.position + " "+posns[counter]);
                    yield return new WaitForSeconds(0.2f);
                }
                //keep looking for a path because if we have arrived the enemy will anyway move away
                pathToFollow = seeker.StartPath(t.position, target.position);
                yield return seeker.IsDone();
                posns = pathToFollow.vectorPath;


            }
            yield return null;
        }
    }


}


