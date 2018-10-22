using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class AI : Actor {

    #region Properties

    public NavMeshAgent NavMesh
    {
        get
        {
            return navMesh;
        }

        set
        {
            navMesh = value;
        }
    }

    protected Transform Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public Transform[] Points
    {
        get
        {
            return points;
        }

        set
        {
            points = value;
        }
    }

    #endregion

    [SerializeField] Transform pointsContainer;
    Transform[] points;
    private Transform player;
    NavMeshAgent navMesh;

    protected override void Start()
    {
        points = pointsContainer.GetComponentsInChildren<Transform>();
        navMesh = GetComponent<NavMeshAgent>();
        speed = navMesh.speed;
        Player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        base.Start();
    }

    protected override void UpdateStats(float _speed)
    {
        speed = _speed;
        navMesh.speed = _speed;
    }

    protected override IEnumerator Movement()
    {
        return null;
    }

    protected override IEnumerator Death()
    {
        mAnimator.SetTrigger("isDead");
        StopCoroutine(movementCoroutine);
        navMesh.speed = 0;
        yield return new WaitForSeconds(3f);      
        Destroy(gameObject);
    }
}


