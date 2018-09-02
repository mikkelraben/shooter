using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Move : NetworkBehaviour {
    public float Horizontal;
    public float Vertical;
    public float speed;
    float Y;
    Vector3 Movement;
    public float Jumpforce;
    public CharacterController controller;
	// Update is called once per frame
	void Update () {

        if(hasAuthority == false){
            return;
        }
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        Movement = new Vector3(Horizontal, Y, Vertical);
        Movement = transform.rotation * Movement;
        MoveChar(Movement);

	}

    private void MoveChar (Vector3 vector)
	{
        if (controller.isGrounded)
        {
            vector.y = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            vector.y += Physics.gravity.y * Time.deltaTime *9.8f;
        }
        if(Input.GetKeyDown(KeyCode.Space) && controller.isGrounded) {
            vector.y = Jumpforce;
        } 
        controller.Move(vector * Time.deltaTime * speed);
	}
}
