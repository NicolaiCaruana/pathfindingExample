using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;


public class customAIMoveScript : MonoBehaviour
{
    Seeker seeker;

    Path pathToFollow;


    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();

        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        Debug.Log("Path complete " + p.error);
        pathToFollow = p;
        StartCoroutine(moveTowardsPath());
    }

    IEnumerator moveTowardsPath()
    {
        foreach(Vector3 position in pathToFollow.vectorPath)
        {
            while(Vector3.Distance(this.transform.position,position)>0.5f)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, position, 1f);
                Debug.Log(position);
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


