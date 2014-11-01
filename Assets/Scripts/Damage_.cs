using UnityEngine;
using System.Collections;

public class Damage_  {

	// Use this for initialization
	public int normaldamage;
	public GameObject gameobjectfrom;
	public GameObject gameobjrctto;
	
	public Damage_(int i,GameObject gf,GameObject gt)
	{
		this.normaldamage=i;
		this.gameobjectfrom=gf;
		this.gameobjrctto=gt;
	}
}
