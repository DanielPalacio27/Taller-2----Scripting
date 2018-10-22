using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusA : Disease {
	
    [SerializeField] protected float downgradePercentage;
    protected float nerfSpeed;


    public VirusA()
    {
        onsetTime = 15f;
        timeUntilDeath = 100f;
        type = DiseaseType.VirusA;
        downgradePercentage = 0.1f;
    }

    private void Start()
    {
        MShader = Resources.Load<Shader>("Shaders/VirusA");
    }


    //Reduce la velocidad de movimiento del jugador dado un porcentaje
    protected override IEnumerator Symptoms(Actor _actor)
    {
        nerfSpeed = _actor.Speed - (_actor.Speed * downgradePercentage);
        _actor.statsHandler(nerfSpeed);
        yield return null;
    }

}
