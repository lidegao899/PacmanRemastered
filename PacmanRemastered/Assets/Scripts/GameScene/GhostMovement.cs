﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    private Vector3 wayPoint;
    private Queue<Vector3> wayPoints;

    private Vector3 _direction;

    public Vector3 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }


    void Update()
    {
        
    }

    public void AILogic()
    { 
    
    }

    public void RunLogic()
    { 
    
    }


}
