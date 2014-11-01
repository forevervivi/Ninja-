using UnityEngine;
using System.Collections;

public class skill_craze : MonoBehaviour {

	// Use this for initialization\
	public GameObject effcet;
	Attribute attribute;
	void Start () {
		attribute=transform.parent.gameObject.GetComponent<Attribute>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Useskill(Skill s)
	{
		attribute.normaldamagepercent+=0.2f;
		attribute.normalup+=0.3f;
		GameObject g=(GameObject)Instantiate(effcet,transform.parent.position,transform.rotation);
		g.transform.parent=transform;
		//g.GetComponent<>
		Destroy(g,s.Getsub());
		Invoke("End",s.Getsub());
	}
	
	void End()
	{
		attribute.normaldamagepercent+=-0.2f;
		attribute.normalup+=-0.3f;
	}
}
