using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusS : VirusA {

    protected float waitTime = 10f;
    protected float freezeTime = 2f;
    protected int freezeProb;
    protected bool isFrozen;

    public VirusS()
    {
        onsetTime = 25f;
        freezeProb = 5;
        timeUntilDeath = 70f;
        downgradePercentage = 0.2f;
        type = DiseaseType.VirusS;
    }

    private void Start()
    {
        MShader = Resources.Load<Shader>("Shaders/VirusS");
    }


    protected override IEnumerator Symptoms(Actor _actor)
    {
        if (Type == DiseaseType.VirusS)
        {
            yield return StartCoroutine(base.Symptoms(_actor));
        }

        yield return new WaitForSeconds(waitTime);
        
        while (!_actor.IsHealing)
        {
            float random = Random.Range(1f, 100f);
            if (random <= freezeProb)
            {
                //print("Congelado");
                _actor.statsHandler(0);
                isFrozen = true;
                yield return new WaitForSeconds(freezeTime);               
                _actor.statsHandler(nerfSpeed);
                isFrozen = false;
            }

            yield return new WaitForSeconds(waitTime);
        }

    }
}
