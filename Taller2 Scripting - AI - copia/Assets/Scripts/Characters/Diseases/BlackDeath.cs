using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDeath : VirusS {

    [SerializeField] float speedDowngrade;

    public BlackDeath()
    {
        freezeProb = 90;
        freezeTime = 3f;
        downgradePercentage = 0.4f;
        onsetTime = 35f;
        timeUntilDeath = 60f;
        type = DiseaseType.BlackDeath;
    }

    private void Start()
    {
        MShader = Resources.Load<Shader>("Shaders/BlackDeath");
    }

    protected override IEnumerator Symptoms(Actor _actor)
    {
        StartCoroutine(base.Symptoms(_actor));

        float maxDowngrade = _actor.Speed - _actor.Speed * downgradePercentage; //Su perdida de velocidad no puede exceder el 40% de la velocidad inicial
        speedDowngrade = _actor.Speed - _actor.Speed * 0.05f;

        yield return new WaitForSeconds(4f);

        while (speedDowngrade >= maxDowngrade)
        {
            if (!isFrozen)
            {    
                speedDowngrade = _actor.Speed - _actor.Speed * 0.05f; //El actor infectado pierde 5% de velocidad de movimiento
                nerfSpeed = speedDowngrade;
                _actor.statsHandler(nerfSpeed);
                yield return new WaitForSeconds(4f);
            }

            yield return null;           
        }

    }
}
