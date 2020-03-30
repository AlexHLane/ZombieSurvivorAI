using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PathNode : MonoBehaviour
{

    public PathNode[] neighbors;
    PathNode parent;
    float FVal;
    

    //PathNode[] open;
    //PathNode[] closed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFVal(float f)
    {
        FVal = f;
    }

    public float GetFVal()
    {
        return FVal;
    }

    public void SetParent(PathNode n)
    {
        parent = n;
    }

    public PathNode GetParent()
    {
        return parent;
    }

    public PathNode[] GetNeighbors()
    {
        return neighbors;
    }
}
