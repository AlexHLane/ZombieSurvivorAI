using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public CRYSTAL_COLOR BaseColor;

    public Crystal crystal;
    public Gate[] gates;

    // Start is called before the first frame update
    void Start()
    {
        BaseColor = crystal.Color;

    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
