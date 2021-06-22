using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom2 : MonoBehaviour {

	private RoomTemplates2 templates;

	void Start(){

		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates2>();
		templates.rooms.Add(this.gameObject);
	}
}
