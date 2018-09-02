using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float health = 50f;
    public void TakeDamage(float Damage){
        health -= Damage;
    }
	void Update()
	{
        if(health <= 0f){
            Destroy(gameObject);
        }
	}
}
