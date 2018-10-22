using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Denizen {

    [SerializeField] List<Transform> vaccines  = new List<Transform>();
    Collider[] mColliders;
    Vector3 vaccinePosition;
    float offset = 2.5f;
    float scanTime = 20f;
    Rigidbody mRigid;

    protected override void Start ()
    {
        mColliders = GetComponents<Collider>();
        mColliders[1].enabled = false;
        mRigid = GetComponent<Rigidbody>();
        mRigid.WakeUp();

        base.Start();        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    protected override IEnumerator Infected(Disease dis)
    {
        StartCoroutine(base.Infected(dis));
        yield return new WaitForSeconds(20f);

        while (diseaseList.Count > 0)
        {     
            StartCoroutine(SearchCure());
            yield return new WaitForSeconds(20f);
        }
    
    }

    private IEnumerator SearchCure()
    {
        pauseMovement = true;
        mColliders[1].enabled = true;
        yield return new WaitForSeconds(0.01f);
        mColliders[1].enabled = false;

        if (vaccines.Count <= 0)
        {
            pauseMovement = false;
            yield break;
        }

        float vaccineDistance = Vector3.Distance(transform.position, vaccines[0].position);

        foreach (Transform v in vaccines)
        {
            Vaccine vaccine = v.GetComponent<Vaccine>();
            foreach (Disease d in diseaseList)
            {
                if(vaccine.DiseaseToCure == d.Type)
                {
                    vaccinePosition = v.position;
                    NavMesh.destination = vaccinePosition;
                    break;
                }
            }
        }
      
        //for (int i = 0; i < vaccines.Count; i++)
        //{
        //    float aux = Vector3.Distance(transform.position, vaccines[i].position);
        //    if (vaccineDistance >= aux)
        //        vaccinePosition = vaccines[i].position;
        //}      
        //NavMesh.destination = vaccinePosition;

        while (NavMesh.remainingDistance >= offset)
        {
            yield return null;
        }

        vaccines.Clear();
        pauseMovement = false;

    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.tag == "vaccine")
        {
            if (vaccines.Contains(other.transform))
            {
                return;
            }
            vaccines.Add(other.transform);
        }
    }
}
