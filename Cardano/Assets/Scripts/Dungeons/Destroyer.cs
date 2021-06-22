using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
	private void Start() {
		Destroy(this.gameObject,3f);
	}

	void OnTriggerEnter2D(Collider2D other){
		print(other.tag );
		if(other.tag != "Teleport")
		Destroy(other.gameObject);
		
	}
}
