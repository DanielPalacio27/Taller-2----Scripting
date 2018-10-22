using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] GameObject door;

	void Start () {

        Player.OnGameOver += GameOver;
        Player.OnEpicWin += EpicWin;
	}

    private void GameOver()
    {
        Time.timeScale = 0;
        print("Game Over");
    }

    private void EpicWin()
    {
        Animator doorAnimator = door.GetComponent<Animator>();
        doorAnimator.SetTrigger("open");
        Collider doorCollider = door.GetComponent<Collider>();
        doorCollider.enabled = false;

        print("Ganador FELICITACIONES!!!");
    }
}
