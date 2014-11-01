using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Damage : MonoBehaviour {

	public GameObject damageUI;

	public void Add(string damage,GameObject g,Color color)
	{
		Vector3 position=g.transform.position;
		GameObject ui=(GameObject)Instantiate(damageUI,position,damageUI.transform.rotation);
		ui.GetComponent<bloodtext>().Set(g,damage,color);
		//ui.GetComponent<bloodtext>().
		
	}
}
