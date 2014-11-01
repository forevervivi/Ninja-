 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainUI : MonoBehaviour {

	// Use this for initialization
	public static GameObject[] buttons;
	public static GameObject[] itembtn;
	public static item[] allitems;
	public static GameObject player;
	public static Message message;
	public static Damage d;
	public static GameObject bloodspark;
	public static GameObject stunspark;
	public static GameObject bloodpanel;
	public static GameObject information;
	public static GameObject levelupeffect;
	public static PlayerUI playerui;
	public static AudioSource Beattaacl;
	
	public GameObject[] buttons2;
	public GameObject[] item_buttons;
	public Message messagepanel;
	public GameObject damage_blood;
	public GameObject player_;
	public GameObject stunspark_;
	public PlayerUI playerui_;
	public GameObject bloodpanel_;
	public GameObject information_;
	public GameObject levelupeffect_;
	public AudioSource Beattaacl_;
	
	
	void Start () {
		Beattaacl=Beattaacl_;
		levelupeffect=levelupeffect_;
		information=information_;
		bloodpanel=bloodpanel_;
		stunspark=stunspark_;
		playerui=playerui_;
		player=player_;
		bloodspark=damage_blood;
		message=messagepanel;
		buttons=buttons2;
		itembtn=item_buttons;
		d=GetComponent<Damage>();
		allitems=Xml.ReadItems("XML/allitems");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
