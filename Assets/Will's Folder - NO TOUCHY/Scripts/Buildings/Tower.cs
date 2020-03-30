using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TOWER_TYPE
{
    BULLET_TOWER
    
}


public class Tower : MonoBehaviour
{
    public virtual void Start()
    {
        gameObject.SetActive(false);
    }

}
