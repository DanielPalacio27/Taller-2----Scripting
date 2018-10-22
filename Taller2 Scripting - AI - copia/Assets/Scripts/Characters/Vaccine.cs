using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaccine : MonoBehaviour {

    [SerializeField] DiseaseType diseaseToCure;
    [SerializeField] float immunity;
    
    public DiseaseType DiseaseToCure { get { return diseaseToCure; } }

    public void OnCollisionEnter(Collision collision)
    {
        print("Esta vacuna cura la enfermedad " + diseaseToCure);
        if (collision.gameObject.GetComponent<Actor>() != null)
        {
            if (collision.gameObject.GetComponent<Disease>() != null)
            {
                Actor actor = collision.gameObject.GetComponent<Actor>();

                foreach(Disease dis in actor.DiseaseList)
                {
                    if(dis.Type == DiseaseToCure)
                    {
                        actor.healingHandler(dis, immunity);
                        actor.IsHealing = true;
                        actor.statsHandler(actor.InitialSpeed);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

}
