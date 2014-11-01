using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour {

	UISlider bloodslider;
	UISlider magicslider;
	UISlider experienceslider;
	UILabel bloodlabel;
	UILabel magiclabel;
	Attribute player;
	UILabel moneylabel;
	MoneyControl playermoney;
	
	IEnumerator Start () {
		experienceslider=transform.FindChild("Experience").gameObject.GetComponent<UISlider>();
		bloodslider=transform.FindChild("playpanel_blood").gameObject.GetComponentInChildren<UISlider>();
		magicslider=transform.FindChild("playpanel_magic").gameObject.GetComponentInChildren<UISlider>();
		bloodlabel=transform.FindChild("playpanel_blood").FindChild("blood").gameObject.GetComponent<UILabel>();
		magiclabel=transform.FindChild("playpanel_magic").FindChild("magic").gameObject.GetComponent<UILabel>();
		moneylabel=transform.FindChild("moneyLabel").gameObject.GetComponent<UILabel>();
		yield return 3;
		player=MainUI.player.GetComponent<Attribute>();
		playermoney=MainUI.player.GetComponent<MoneyControl>();
		Reset();
		ResetMoney();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Reset()
	{
		experienceslider.value=(float)player.experience_now/(float)player.experience_max[player.Level-1];
        bloodslider.value = (float)player.nowBlood / (float)player.maxBlood;
		//Debug.Log(player.nowblood);
		//Debug.Log(player.maxblood);
		bloodlabel.text=player.nowBlood.ToString()+"/"+player.maxBlood.ToString();
        magicslider.value = (float)player.nowMagic / (float)player.maxMagic;
		magiclabel.text=player.nowMagic.ToString()+"/"+player.maxMagic.ToString();
	}
	
    //设置金钱显示
	public void ResetMoney()
	{
		moneylabel.text="Gold:"+playermoney.money.ToString();
	}
	
}
