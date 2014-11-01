using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillUp_Control : MonoBehaviour {

	// Use this for initialization
	int now;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void UP(int i)
	{
		//List<> MainUI.player.GetComponent<Skillcontrol>().skills
		List<Skill> skills=MainUI.player.GetComponent<Skillcontrol>().skills;
		if(skills[i].LevelUP())
		{
			now-=1;
			Attribute a=MainUI.player.GetComponent<Attribute>();
			//MainUI.player.GetComponent<Attribute>().skillupavilable-=1;
			//MainUI.pa
			a.skillupavilable-=1;
			MainUI.player.GetComponent<Skillcontrol>().Resetskill();
			if(now<=0)
			{
				Destroy(this.gameObject);
			}
		}
		else
		{
			MainUI.message.Add("Max Level");
		}
	}
	
	public void Set(int i)
	{
		now=i;
	}
}
