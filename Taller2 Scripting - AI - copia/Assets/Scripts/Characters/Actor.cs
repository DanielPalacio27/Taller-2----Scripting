using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Actor : MonoBehaviour {

    public Action<Disease, float> healingHandler;
    public Action<float> statsHandler;

    private Renderer mRenderer;
    Shader mShader;

    protected Animator mAnimator;
    [SerializeField] protected float speed;
    private float initialSpeed;

    protected List<Disease> diseaseList;
    Disease[] initialDiseases;
    public List<Disease> DiseaseList { get { return diseaseList; } }

    public bool IsHealing { get; set; }
    public bool IsDead { get; protected set; }

    [SerializeField] float virusAProb, virusSProb, blackDeathProb;
    [SerializeField] protected float virusAResistance, virusSResistance, blackDeathResistance;
    [SerializeField] protected float virusATimeInfected, virusSTimeInfected, blackdeathTimeInfected;
    
    protected abstract IEnumerator Movement();
    protected abstract IEnumerator Death();
    protected abstract void UpdateStats(float _speed);

    protected Coroutine movementCoroutine = null;

    #region Properties

    public float Speed
    {
        get
        {
            return speed;
        }

        protected set
        {
            speed = value;
        }
    }

    public float InitialSpeed
    {
        get
        {
            return initialSpeed;
        }

        protected set
        {
            initialSpeed = value;
        }
    }
  
    #endregion

    protected virtual void Start()
    {
        diseaseList = new List<Disease>();
        initialDiseases = GetComponents<Disease>();
         
        VirusA virusA = new VirusA();
        virusAResistance = virusA.OnsetTime;

        VirusS virusS = new VirusS();
        virusSResistance = virusS.OnsetTime;

        BlackDeath blackDeath = new BlackDeath();
        blackDeathResistance = blackDeath.OnsetTime;
        
        virusAProb = 100f;
        virusSProb = 100f;
        blackDeathProb = 100f;

        if(initialDiseases.Length > 0)
        {
            foreach (Disease d in initialDiseases)
            {
                switch (d.Type)
                {
                    case DiseaseType.VirusA:
                        StartCoroutine(Infected(d));
                        break;
                    case DiseaseType.VirusS:
                        StartCoroutine(Infected(d));
                        break;
                    case DiseaseType.BlackDeath:
                        StartCoroutine(Infected(d));
                        break;
                }

                diseaseList.Add(d);
            }
        }

        mAnimator = GetComponent<Animator>();
        healingHandler = VaccineEffects;
        statsHandler = UpdateStats;

        initialSpeed = speed;
        mRenderer = GetComponentInChildren<Renderer>();
        mShader = mRenderer.material.shader;
        movementCoroutine = StartCoroutine(Movement());
    }

    private void VaccineEffects(Disease disease, float reduceProb)
    {
        switch (disease.Type)
        {
            case DiseaseType.VirusA:
                virusAProb -= reduceProb;
                virusAProb = (virusAProb <= 40) ? 40 : virusAProb;
                virusAResistance = virusAResistance - virusAResistance * 0.1f;
                virusAResistance = (virusAResistance <= disease.OnsetTime * 0.5f) ? disease.OnsetTime * 0.5f : virusAResistance;
                break;
            case DiseaseType.VirusS:
                virusSProb -= reduceProb;
                virusSProb = (virusAProb <= 40) ? 40 : virusSProb;
                virusSResistance = virusSResistance - virusSResistance * 0.1f;
                virusSResistance = (virusSResistance <= disease.OnsetTime * 0.5f) ? disease.OnsetTime * 0.5f : virusSResistance;
                break;
            case DiseaseType.BlackDeath:
                blackDeathProb -= reduceProb;
                blackDeathProb = (virusAProb <= 40) ? 40 : blackDeathProb;
                blackDeathResistance = blackDeathResistance - blackDeathResistance * 0.1f;
                blackDeathResistance = (blackDeathResistance <= disease.OnsetTime * 0.5f) ? disease.OnsetTime * 0.5f : blackDeathResistance;
                break;
        }
    }



    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<Actor>() != null)
        {
            if (collision.gameObject.GetComponent<Disease>() != null)
            {
                Disease dis = collision.gameObject.GetComponent<Disease>();
                
                foreach (Disease d in diseaseList)
                {
                    if (d.Type == dis.Type) return;
                }

                if (Probability(dis.Type))
                {
                    switch (dis.Type)
                    {
                        case DiseaseType.VirusA:                           
                            gameObject.AddComponent(typeof(VirusA));
                            diseaseList.Add(GetComponent<VirusA>());
                            StartCoroutine(Infected(GetComponent<VirusA>()));
                            break;
                        case DiseaseType.VirusS:
                            gameObject.AddComponent(typeof(VirusS));
                            diseaseList.Add(GetComponent<VirusS>());
                            StartCoroutine(Infected(GetComponent<VirusS>()));
                            break;
                        case DiseaseType.BlackDeath:
                            gameObject.AddComponent(typeof(BlackDeath));
                            diseaseList.Add(GetComponent<BlackDeath>());
                            StartCoroutine(Infected(GetComponent<BlackDeath>()));
                            break;
                    }
                }
            }
        }
    }

    protected virtual IEnumerator Infected(Disease dis)
    {
        float auxOnSet = 0;
        float t = 0;
        float timeInfected = 0;

        Func<float, float> timeInfectedHandler = (x) => x = 0;

        switch (dis.Type)
        {
            case DiseaseType.VirusA:
                auxOnSet = virusAResistance;
                timeInfectedHandler = (x) => virusATimeInfected = x;
                break;
            case DiseaseType.VirusS:
                auxOnSet = virusSResistance;
                timeInfectedHandler = (x) => virusSTimeInfected = x;
                break;
            case DiseaseType.BlackDeath:
                auxOnSet = blackDeathResistance;
                timeInfectedHandler = (x) => blackdeathTimeInfected = x;
                break;
        } 
        
        while (t <= auxOnSet && !IsHealing)
        {
            t += Time.deltaTime;
            timeInfected += Time.deltaTime;
            timeInfectedHandler(timeInfected);
            yield return null;
        }

        if (IsHealing)
        {
            IsHealing = !IsHealing;
            StartCoroutine(Infected(dis));
            timeInfectedHandler(0);
            yield break;
        }

        dis.StartCoroutine(dis.symptomsHandler(this));
        mRenderer.material.shader = dis.MShader;
        t = 0;

        while (t <= dis.TimeUntilDeath && !IsHealing)
        {
            t += Time.deltaTime;
            timeInfected += Time.deltaTime;
            timeInfectedHandler(timeInfected);
            yield return null;
        }

        if (IsHealing)
        {
            mRenderer.material.shader = mShader;
            IsHealing = !IsHealing;
            StartCoroutine(Infected(dis));
            timeInfectedHandler(0);
            yield break;
        }
        /*
        else if(IsHealing && diseaseList.Count > 0)
        {
            mRenderer.material.shader = mShader;
            IsHealing = !IsHealing;
            StartCoroutine(Infected(dis));
            timeInfectedHandler(0);
            yield break;
        }
        */
        if (t >= dis.TimeUntilDeath) StartCoroutine(Death());

    }

    private bool Probability(DiseaseType disease)
    {
        float random = UnityEngine.Random.Range(0f, 100f);
        switch (disease)
        {
            case DiseaseType.VirusA:
                if (random <= virusAProb)
                    return true;
                else
                    return false;
            case DiseaseType.VirusS:
                if (random <= virusSProb)
                    return true;
                else
                    return false;
            case DiseaseType.BlackDeath:
                if (random <= blackDeathProb)
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }

}
