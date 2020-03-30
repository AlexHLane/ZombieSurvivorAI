using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OCCUPIED
{
    NOT_OCCUPIED = 0,
    SINGLE,
    DOUBLE
}
public class DefenseNode : MonoBehaviour
{
    OCCUPIED occupyStatus;

    // Start is called before the first frame update
    void Start()
    {
        occupyStatus = OCCUPIED.NOT_OCCUPIED;
    }

    public void SetOccupiedStatus(OCCUPIED oc)
    {
        occupyStatus = oc;
    }

    public OCCUPIED GetOccupiedStatus()
    {
        return occupyStatus;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
