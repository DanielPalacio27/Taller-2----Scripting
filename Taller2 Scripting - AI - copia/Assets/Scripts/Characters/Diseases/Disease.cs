using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiseaseType
{
    VirusA,
    VirusS,
    BlackDeath
}

public delegate IEnumerator SymptomsHandler(Actor _actor);

public abstract class Disease : MonoBehaviour{

    public SymptomsHandler symptomsHandler;

    protected float onsetTime, timeUntilDeath;
    [SerializeField] Shader mShader;
    [SerializeField] protected DiseaseType type;

    
    public Disease()
    {
        symptomsHandler = Symptoms;
    }
    

    protected abstract IEnumerator Symptoms(Actor _actor);
    
    #region Properties
    public float OnsetTime
    {
        get
        {
            return onsetTime;
        }

        protected set
        {
            onsetTime = value;
        }
    }

    public Shader MShader
    {
        get
        {
            return mShader;
        }

        protected set
        {
            mShader = value;
        }
    }

    public float TimeUntilDeath
    {
        get
        {
            return timeUntilDeath;
        }

        protected set
        {
            timeUntilDeath = value;
        }
    }

    
    public DiseaseType Type
    {
        get
        {
            return type;
        }

        protected set
        {
            type = value;
        }
    }
    
    #endregion
}

