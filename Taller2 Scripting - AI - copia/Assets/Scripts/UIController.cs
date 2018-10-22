using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private static UIController instance = null;
    public static UIController Instance { get { return instance; } }

    public Player player;
    public Text virusA, virusS, blackDeath;
    public GameObject key;

	void Start () {

		if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}
	
	void Update () {

        virusA.text = "Tiempo de contagio (Virus A) : " + player.VirusATimeInfected;
        virusS.text = "Tiempo de contagio (Virus S) : " + player.VirusSTimeInfected;
        blackDeath.text = "Tiempo de contagio (BlackDeath) : " + player.BlackDeathTimeInfected;
    }
}
