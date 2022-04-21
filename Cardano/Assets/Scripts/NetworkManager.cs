using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
using System;
using Cinemachine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
	public GameObject equip;
	public UI_InventoryV2 uI_1_InventoryV2;
	public bool isConected;
	public Text status;
	public GameObject btnLogin;
    public GameObject cameraShake;
    public GameObject netPlayer;
    public GameObject playAtributsPanel;
    public GameObject changeSkinPainel;
	public static NetworkManager instance;

	public SocketIOComponent socket;

	public GameObject playerPrefab;
	public GameObject playerLocalInstance;
	public Dictionary<int, ItensEntity> itensDatabase = new Dictionary<int, ItensEntity>();


	public Dictionary<string,GameObject> networkPlayers = new Dictionary<string, GameObject> ();
	[SerializeField]
	List<ItensEntity> itensDatabaseTEST = new List<ItensEntity>();
	public TextAsset jsonFile;
	[SerializeField] public UI_CharacterEquipment uiCharacterEquipment;

	
    // Start is called before the first frame update
    void Start()
    {
		LoadItensDatabase();

		if (instance == null) 
		{

			instance = this;

			socket = GetComponent<SocketIOComponent> ();

			//socket.On ("PONG",OnReceivePong);

			//socket.On ("JOIN_SUCCESS", OnJoinSuccess);

			//socket.On ("SPAWN_PLAYER",OnSpawnPlayer);

			//socket.On ("UPDATE_POS_ROT",OnUpdatePosAndRot);

			//socket.On ("UPDATE_ANIMATOR",OnUpdateAnimator );

			//socket.On ("USER_DISCONNECTED",OnUserDisconnected);

			SendPingToServer();
			print("Mandei PINGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG");
		}
		else
		{
			Destroy (this.gameObject);
		}
    }

	void OnReceivePong(SocketIOEvent pack)
	{
		Dictionary<string,string> result = pack.data.ToDictionary ();


		Debug.Log ("mensagem do servidor: "+result["message"]);
		isConected = true;
	}

	//send "ping" to server
	public void SendPingToServer()
	{
        print("Sendo Ping");
		Dictionary<string,string> pack = new Dictionary<string,string> ();

		pack["message"] = "ping!!!";

		socket.Emit ("", new JSONObject (pack));//send to nodejs server
		socket.Emit ("", new JSONObject (pack));//send to nodejs server
		socket.Emit ("", new JSONObject (pack));//send to nodejs server
		socket.Emit ("", new JSONObject (pack));//send to nodejs server
		

	}


	public void EmitJoin()
	{
		status.text = "Loading";
		if(isConected) {

			Dictionary<string,string> data = new Dictionary<string,string> ();

			data ["name"] = CanvasManager.instance.inputLogin.text;

			socket.Emit("JOIN_ROOM",new JSONObject(data));
			print("JOIN_ROOM");
		} else {
			StartCoroutine(ConectFail());
		}

	}

	IEnumerator ConectFail()
    {
		int i =0;
        while (!isConected && i < 20) // loop infinito
        {
			status.text = "Loading "+i;
			SendPingToServer();
			yield return new WaitForSeconds(2f);
			i++;
            
        }
		if(isConected){
			Dictionary<string,string> data = new Dictionary<string,string> ();
			data ["name"] = CanvasManager.instance.inputLogin.text;
			socket.Emit("JOIN_ROOM",new JSONObject(data));
			print("JOIN_ROOM");
		} else {
			status.text = "Try Later Please. ";
		}
    }
	public void OnJoinSuccess(SocketIOEvent pack)
	{
        print("JOIN_SUCCESS");
		cameraShake.SetActive(true);
        playAtributsPanel.SetActive(true);
        changeSkinPainel.SetActive(true);
		Dictionary<string,string> result = pack.data.ToDictionary ();

		playerLocalInstance = Instantiate (playerPrefab, new Vector3 (0.03f, -0.7f, 0), Quaternion.identity);
        PlayerController new_player = playerLocalInstance.GetComponent<PlayerController>();
		new_player.VirtualCamera = cameraShake.GetComponent<CinemachineVirtualCamera>();
		cameraShake.GetComponent<CinemachineVirtualCamera>().LookAt = playerLocalInstance.transform;
		cameraShake.GetComponent<CinemachineVirtualCamera>().Follow = playerLocalInstance.transform;
        Player player = playerLocalInstance.GetComponent<Player>();
        //player.attributesPanel = playAtributsPanel;
		player.entity.id = result ["id"];

		player.entity.name = result ["name"];
		player.nameText.text = result ["name"];

		player.entity.isLocalPlayer = true;
        //new_player.c PlayerSkinController
        PlayerSkinController cso = playerLocalInstance.GetComponent<PlayerSkinController>();

        cso.changeSkinPainel = changeSkinPainel;
        print(cso);
        cso.changeSkin = changeSkinPainel.GetComponent<ChangeSkin>();


        changeSkinPainel.SetActive(false);
        playAtributsPanel.SetActive(false);

		CanvasManager.instance.OpenScreen (1);
		GameObject.Find("Login").SetActive(false);

	}

	void OnSpawnPlayer(SocketIOEvent pack)
	{
        
		Dictionary<string,string> result = pack.data.ToDictionary ();

		if(!networkPlayers.ContainsKey(result ["id"]))
		{
			GameObject new_player = Instantiate (playerPrefab,new Vector3(0,0,0),Quaternion.identity);

			Player player = new_player.GetComponent<Player>();
            //player.attributesPanel = playAtributsPanel;
            player.entity.id = result ["id"];
            player.entity.name = result ["name"];
            player.nameText.text = result ["name"];

			//new_player.gameObject.GetComponentInChildren<TextMesh>().text = result["name"];

			networkPlayers [result ["id"]] = new_player;
		}



	}

	public void EmitPosAndRot(Vector3 _newPos, Quaternion _newRot, float lookLeft)
	{
		Dictionary<string,string> data = new Dictionary<string,string> ();

		data ["position"] = _newPos.x + ":" + _newPos.y + ":" + _newPos.z;
		data ["rotation"] = _newRot.x + ":" + _newRot.y + ":" + _newRot.z + ":" + _newRot.w;
        data ["mov"] = lookLeft.ToString();
        //print("ENVIANDO lookLeft "+lookLeft);
		socket.Emit ("MOVE_AND_ROT",new JSONObject(data));


	}


	void OnUpdatePosAndRot(SocketIOEvent pack)
	{
        
		Dictionary<string,string> result = pack.data.ToDictionary ();
		// pega na lista de jogadores pelo id
		GameObject netPlayer = networkPlayers [result ["id"]];
		Vector3 _pos = UtilsClass.StringToVector3(result ["position"]);
		Vector4 _rot =  UtilsClass.StringToVector4(result["rotation"]);
        //print("CONVERTE");
        float mov = float.Parse(result ["mov"]);

        //print("CONVERTE FOMMM");
        //print(result ["lookLeft"]);
        //print(lookLeft);
		netPlayer.GetComponent<PlayerController>().UpdatePosAndRot (_pos, new Quaternion (_rot.x, _rot.y, _rot.z, _rot.w), mov);
        //print("FIMMMMMMMMMMMM");

	}

	public void EmitAnimation(string _animation, String id)
	{
		Dictionary<string,string> data = new Dictionary<string,string> ();

		data ["id"] = id;
		data ["animation"] = _animation;

		socket.Emit ("ANIMATION",new JSONObject(data));

	}

	void OnUpdateAnimator(SocketIOEvent pack)
	{
        
		Dictionary<string,string> result = pack.data.ToDictionary ();

		netPlayer = networkPlayers [result ["id"]];
		//Debug.Log ("receive animation: " + result ["id"]);
		//Debug.Log ("receive animation: " + result ["animation"]);
		//Debug.Log ("receive animation: " + result ["value"]);
        print(result ["id"]);
		netPlayer.GetComponent<PlayerController>().UpdateAnimator (result["animation"]); 


	}

	void OnUserDisconnected(SocketIOEvent pack)
	{
		Dictionary<string,string> result = pack.data.ToDictionary ();

		if (GameObject.Find (result ["id"]))
		{
			Destroy (GameObject.Find (result ["id"]));

			networkPlayers.Remove (result["id"]);
		} 

	}

	public void OpenInventory() {
		playerLocalInstance.GetComponent<PlayerController>().OpenInventory();
	}

	 
	public void LoadItensDatabase() {
		ListaItensJson itensJson = JsonUtility.FromJson<ListaItensJson>(jsonFile.text);
		foreach (ItensEntity item in itensJson.Itens)
        {
			itensDatabaseTEST.Add(item);
			itensDatabase.Add(item.idItem, item);
        }
	}


}