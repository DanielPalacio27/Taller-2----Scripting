using System.Collections;
using UnityEngine;

public class Grudger : AI {

    [SerializeField] float distance, distanceToDetect;
    int currentPoint;
    float timeToLeave;

    protected override void Start()
    {
        base.Start();
        distanceToDetect = 5;
        currentPoint = 1;
        timeToLeave = 5f;
        NavMesh.destination = Points[currentPoint].position;

    }

    protected override IEnumerator Movement()
    {
        distance = distanceToDetect + 1;
        mAnimator.SetFloat("velocity", 1);
        while (distance > distanceToDetect)
        {
            distance = Vector3.Distance(transform.position, Player.position);
            if (NavMesh.remainingDistance <= 0.1) currentPoint = (currentPoint < Points.Length - 1) ? currentPoint + 1 : 1;
            NavMesh.destination = Points[currentPoint].position;
            yield return null;
        }

        StartCoroutine(FollowPlayer());

    }

    public virtual IEnumerator FollowPlayer()
    {
        float t = 0;
        while (t < timeToLeave)
        {
            t += Time.deltaTime;
            NavMesh.destination = Player.position;
            yield return null;
        }
        StartCoroutine(Movement());
    }

}
