using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

	public int openingDirection;
	// 1 --> need bottom door
	// 2 --> need top door
	// 3 --> need left door
	// 4 --> need right door

[SerializeField]
	private RoomTemplates templates;
	private int rand;
	public bool spawned = false;

	public float waitTime = 1f;

	void Start(){
		waitTime = 1f;
		Destroy(gameObject, waitTime);
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		print(templates);
		Invoke("Spawn", 0.1f);
	}


	void Spawn(){
		if(spawned == false){
			if(openingDirection == 1){
				// Need to spawn a room with a BOTTOM door.
				rand = Random.Range(0, templates.bottomRooms.Length);
				Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
				//Debug.Log(templates.bottomRooms[rand].name+transform.position+templates.bottomRooms[rand].transform.rotation);
			} else if(openingDirection == 2){
				// Need to spawn a room with a TOP door.
				rand = Random.Range(0, templates.topRooms.Length);
				Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
				///Debug.Log(templates.topRooms[rand].name+transform.position+templates.topRooms[rand].transform.rotation);
			} else if(openingDirection == 3){
				// Need to spawn a room with a LEFT door.
				rand = Random.Range(0, templates.leftRooms.Length);
				Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
				//Debug.Log(templates.leftRooms[rand].name+transform.position+templates.leftRooms[rand].transform.rotation);
			} else if(openingDirection == 4){
				// Need to spawn a room with a RIGHT door.
				rand = Random.Range(0, templates.rightRooms.Length);
				Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
				//Debug.Log(templates.rightRooms[rand].name+transform.position+templates.rightRooms[rand].transform.rotation);
			}
			spawned = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("ApawnPoint")){
			if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false){
				//print(gameObject.transform.parent.parent.name +" Position"+ transform.parent.parent.position);
				//print(other.gameObject.transform.parent.parent.name +" Position"+ other.gameObject.transform.parent.parent.position);
				//print(templates);
				if(templates == null){
					templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
				}
				//print(other.transform.parent.parent.name);
				//print(transform.parent.parent.name);
				Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
				Destroy(gameObject);
			} 
			spawned = true;
		}
	}
}
