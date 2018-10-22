using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] Transform player, cam, pivot;
    float mouseX, mouseY;

    private void Update()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");
        mouseY = Mathf.Clamp(mouseY, -40f, 40f);

        cam.LookAt(pivot);
        pivot.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        pivot.position = new Vector3(player.position.x, player.position.y + 2, player.position.z);
    }

}
