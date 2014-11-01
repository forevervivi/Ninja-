using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillControl : MonoBehaviour
{

    public string skillXML;
    public bool start { get; set; }

    public List<Skill> skills { get; set; }
    Skill nowskill;
    Attribute myattribute;
    bool ableskill = true;

    IEnumerator Start()
    {
        start = false;
        //ctrol=GetComponent<control>();
        //navmeshagent=GetComponent<NavMeshAgent>();
        skills = new List<Skill>();
        skills = Xml.ReadSkills(skillXML);
        yield return skills;
        for (int i = 0; i < skills.Count; i++)
        {
            startskill(skills[i], i);
        }
        myattribute = GetComponent<Attribute>();

        //skills=new Skill[10];
        //skills[0]=new Skill(1f,Skilltype.heal);
        //GameObject x=(GameObject)Resources.Load("1");
        //yield return x;
        //Instantiate(x,transform.position,transform.rotation);
        //yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            //Debug.Log("1");
            if (!animation.IsPlaying(nowskill.animationstring))
            {
                //ctrol.nowstate=State.Idle;
                start = false;
                //ctrol.Statetoidle();
                //Debug.Log("i");
                //Dealskill();
                //Debug.Log("s");
            }
        }

    }

    public void Doskill(int i)
    {
        //Debug.Log(skills.Count);
        if (i >= skills.Count)
            return;
        if (!skills[i].Isready)
            return;
        if (myattribute.isdied)
            return;
        if (myattribute.nowmagic < skills[i].Getmagic())
            return;
        if (!ableskill)
            return;
        //ctrol.CancelInvoke("Walk");
        animation.CrossFade(skills[i].animationstring);
        //animation.Play(skills[i].animationstring);
        animation.CrossFadeQueued(myattribute.idlestring[0]);
        //skills[i].skillgameobject.SendMessage("Useskill");
        //navmeshagent.Stop();
        nowskill = skills[i];
        //ctrol.nowstate=State.Skill;
        start = true;
        Invoke("Dealskill", nowskill.delay);
        //StartCoroutine("Delaystart");
        //Debug.Log("X");
    }

    //	IEnumerator Delay()
    //	{
    //		yield return 100;
    //		start=true;
    //	}

    public void Stopskill()
    {
        if (!start)
            return;
        start = false;
        if (IsInvoking("Dealskill"))
            CancelInvoke("Dealskill");
        //ctrol.Statetoidle();
        if (animation[nowskill.animationstring].wrapMode == WrapMode.Loop)
        {
            //ctrol.Statetoidle();
            start = false;
        }
    }

    IEnumerator Delaystart()
    {
        yield return new WaitForSeconds(4);
        start = true;
    }

    public void Preforskill()
    {
        //navmeshagent.Stop();
        //ctrol.nowstate=State.Skill;
    }

    void Dealskill()
    {
        //		switch (nowskill.type)
        //		{
        //		case Skilltype.range:
        //			nowskill.skillgameobject.SendMessage("Useskill");
        //			break;
        //		case Skilltype.single:
        //			nowskill.skillgameobject.SendMessage("Useskill",nowskill);
        //			break;
        //		default:
        //			break;
        //		}
        if (myattribute.nowmagic < nowskill.Getmagic())
            return;
        myattribute.nowmagic -= nowskill.Getmagic();
        nowskill.skillgameobject.SendMessage("Useskill", nowskill);
        if (nowskill.skillbutton)
            nowskill.skillbutton.SendMessage("Set", nowskill.GetCD());
        StartCoroutine("EntertoCD", nowskill);
        MainUI.playerui.Reset();
        if (animation[nowskill.animationstring].wrapMode == WrapMode.Loop)
        {
            start = false;
            //ctrol.Statetoidle();
        }


    }

    IEnumerator EntertoCD(Skill s)
    {
        s.Isready = false;
        yield return new WaitForSeconds(s.GetCD());
        s.Isready = true;
    }

    void startskill(Skill s, int i)
    {
        GameObject g = (GameObject)Resources.Load(s.skillgbname);
        GameObject gg = (GameObject)Instantiate(g, transform.position, g.transform.rotation);
        s.skillgameobject = gg;
        //g.transform.position=transform.position;
        gg.transform.parent = this.gameObject.transform;
        if (tag == "Player")
        {
            s.skillbutton = MainUI.buttons[i];
            skillui ui = s.skillbutton.GetComponent<skillui>();
            ui.Setmagic(s.Getmagic());
            ui.SetIcon(s.iconame);

        }

        if (!s.Canuse)
        {
            gg.SendMessage("Set", s);
        }
        //Invoke("Dealskill",s.delay);
        //s.skillbutton=xx;
    }

    public void Resetskill()
    {
        foreach (Skill s in skills)
        {
            s.skillbutton.GetComponent<skillui>().Setmagic(s.Getmagic());
        }
    }

    public void Bestun(float time)
    {
        ableskill = false;
        if (IsInvoking("Dealskill"))
            CancelInvoke("Dealskill");
        if (IsInvoking("skillable"))
            CancelInvoke("skillable");
        Invoke("skillable", time);
    }

    void skillable()
    {
        ableskill = true;
    }
}
