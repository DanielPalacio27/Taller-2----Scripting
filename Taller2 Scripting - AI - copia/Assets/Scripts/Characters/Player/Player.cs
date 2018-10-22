using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameHandler();

public class Player : Actor {

    public static event GameHandler OnGameOver;
    public static event GameHandler OnEpicWin;

    private bool hasTheKey;
    public bool HasTheKey { get { return hasTheKey; } }

    [SerializeField] Transform camPivot;
    [SerializeField] float rotSpeed;

    protected override void Start()
    {
        speed = 0.12f;
        rotSpeed = 2f;
        base.Start();
    }

    public float VirusATimeInfected{get{ return virusATimeInfected; } }
    public float VirusSTimeInfected { get { return virusSTimeInfected; } }
    public float BlackDeathTimeInfected { get { return blackdeathTimeInfected; } }

    protected override IEnumerator Death()
    {       
        mAnimator.SetTrigger("isDead");
        OnGameOver();
        StopAllCoroutines();
        yield return null;  
    }

    protected override void UpdateStats(float _speed)
    {
        speed = _speed;
    }

    protected override IEnumerator Movement()
    {
        while (!IsDead)
        {
            float vDirection = Input.GetAxis("Vertical") * speed;
            float hDirection = Input.GetAxis("Horizontal") * speed * 0.35f;

            if (vDirection > 0 || hDirection > 0) mAnimator.SetFloat("velocity", 1);
            else if(vDirection == 0 || hDirection == 0) mAnimator.SetFloat("velocity", 0);
            if(vDirection < 0) mAnimator.SetFloat("velocity", -1);

            Vector3 movement = new Vector3(hDirection, 0f, vDirection);
            movement = transform.rotation * movement;

            transform.position += movement;

            Quaternion rotation = Quaternion.Euler(0f, camPivot.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);

            yield return null;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if(collision.gameObject.GetComponent<Key>() != null)
        {
            hasTheKey = true;
            UIController.Instance.key.SetActive(true);
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "exit" && hasTheKey)
        {
            if(diseaseList.Count <= 0)
            {
                OnEpicWin();
                return;
            }

            foreach(Disease d in diseaseList)
            {
                switch (d.Type)
                {
                    case DiseaseType.VirusA:
                        if(virusATimeInfected < d.OnsetTime) OnEpicWin();
                        break;
                    case DiseaseType.VirusS:
                        if (virusSTimeInfected < d.OnsetTime) OnEpicWin();
                        break;
                    case DiseaseType.BlackDeath:
                        if (blackdeathTimeInfected < d.OnsetTime) OnEpicWin();
                        break;
                }

            }
        }
    }
}




