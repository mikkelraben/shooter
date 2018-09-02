using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LookRotate : NetworkBehaviour {
    public float rotateX;
    public float rotateY;
    public float sensitivityX = 4f;
    public float sensitivityY = 4f;

    GameObject Child;
    Quaternion quaternion = Quaternion.identity;
	// Update is called once per frame
	void Update () {
        if(hasAuthority == false){
            return;
        }

        rotateX = Input.GetAxis("Mouse Y") * 4 + quaternion.eulerAngles.x;
        rotateY = Input.GetAxis("Mouse X") * 4 + quaternion.eulerAngles.y;
        if(rotateX > 85 && rotateX < 180){
            rotateX = 85;
        }
        if(rotateX < 270 && rotateX > 180){
            rotateX = 270;
        }
        Child = transform.GetChild(0).gameObject;

        quaternion.eulerAngles = new Vector3(0, rotateY, 0);
        transform.SetPositionAndRotation(transform.position, quaternion);
        quaternion.eulerAngles = new Vector3(rotateX, rotateY, 0);
        Child.transform.SetPositionAndRotation(Child.transform.position, quaternion);

    }
}
