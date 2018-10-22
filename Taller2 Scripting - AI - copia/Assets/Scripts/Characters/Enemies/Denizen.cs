using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Denizen : AI {

    int currentPoint = 1;
    protected bool pauseMovement;

    protected override void Start()
    {       
        base.Start();
        currentPoint = 1;
        NavMesh.destination = Points[currentPoint].position;
    }

    
    protected override IEnumerator Movement()
    {
        while (!IsDead)
        {            
            if (NavMesh.remainingDistance <= 0.1) currentPoint = (currentPoint < Points.Length - 1) ? currentPoint + 1 : 1;

            mAnimator.SetFloat("velocity", 1f);
            NavMesh.destination = Points[currentPoint].position;
            yield return new WaitWhile(() => pauseMovement == true);
        }

    }    
}
