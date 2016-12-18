using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkHUD : MonoBehaviour {


	public NetworkManager manager;
	//Network

	// Use this for initialization
	void Start () {
		manager = GetComponent<NetworkManager> ();
		//Invoke ("printInfo", 5.0f);
	}
	
	void printInfo()
	{
		
		manager.StartHost ();
		//manager.StartClient ();
		Debug.Log (manager.matchHost);
	}

	public void startHost()
	{
		manager.StartHost ();
	}

	public void startClient()
	{
		manager.StartClient ();
	}
}
