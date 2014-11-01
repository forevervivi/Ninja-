using UnityEngine;
using System.Collections;
using System;

public class Attribute : MonoBehaviour
{

    // Use this for initialization
    public int normalDamage { get; set; }
    public int normalDamage_ = 30;
    public int normalDamagedif { get; set; }
    public int maxBlood { get; set; }
    public int maxBlood_ = 240;
    public int nowBlood { get; set; }
    public int maxMagic { get; set; }
    public int maxMagic_ = 200;
    public int nowMagic { get; set; }
    public int protectBlood { get; set; }
    public int experience_now { get; set; }
    public int[] experience_max;
    public int Level { get; set; }

    public float time_bloodUp { get; set; }
    public float time_magicUp { get; set; }
    public float time_bloodDown;
    public float time_magicDown;

    public int skillUpAvilable { get; set; }

    public int lp_attack;
    public int lp_speed;
    public int lp_blood;
    public int lp_magic;

    public int moneyDrop;
    public int moneyDropdif;
    public int experienceDrop;
    public float normalDamagePercent { get; set; }
    public float normalUp { get; set; }
    public Camera mainCamera;
    public string[] attackString;
    public string[] idleString;
    public string deathString;
    public string runString;
    public string stunString;
    public bool isDead { get; set; }
    public GameObject[] meshs;
    public GameObject mainControl;
    //public GameObject levelupeffect;
    GameObject bloodSpark;
    Damage damage;
    public float fadeouttime;
    float startTime;
    NavMeshAgent navAgent;

    AudioSource[] audios;
    AudioSource audio_beattack;
    //AudioSource audio_1;

    public delegate int DiedDelegate(GameObject myself);
    public DiedDelegate Ondied;

    public delegate bool BeattackDelegate(Damage_ d);
    public BeattackDelegate Onbeattack;

    public delegate void LevelupDelegate();
    public LevelupDelegate Onlevelup;

    //NavMeshAgent navagent;
    //CapsuleCollider capsulec;
    Control mycontrol;


    IEnumerator Start()
    {


        audios = GetComponents<AudioSource>();
        audio_beattack = publicclass.GetAudio(audios, "damage");
        time_bloodUp = 2f;
        time_magicUp = 1.7f;

        skillUpAvilable = 0;

        Level = 1;
        experience_now = 0;
        normalUp = 1f;
        normalDamagePercent = 1f;
        normalDamagedif = 5;
        normalDamage = normalDamage_;
        maxBlood = maxBlood_;
        nowBlood = maxBlood;
        maxMagic = maxMagic_;
        nowMagic = maxMagic;
        isDead = false;
        mycontrol = GetComponent<Control>();
        damage = mainControl.GetComponent<Damage>();
        navAgent = GetComponent<NavMeshAgent>();
        protectBlood = 0;
        yield return 2;
        bloodSpark = MainUI.bloodspark;
        if (tag != "Player")
        {
            GameObject g = (GameObject)Instantiate(MainUI.bloodpanel, transform.position, MainUI.bloodpanel.transform.rotation);
            g.GetComponent<BloodControl>().Set(this.gameObject);
        }

        StartCoroutine(bloodup());
        StartCoroutine(magicup());
        //navagent=GetComponent<NavMeshAgent>();
        //capsulec=GetComponent<>

    }

    // Update is called once per frame
    void Update()
    {
        Fadeout();

    }

    public void Beattack(Damage_ damage)
    {
        if (isDead)
            return;
        if (Onbeattack != null)
        {
            foreach (Delegate de in Onbeattack.GetInvocationList())
            {
                //Debug.Log("1");
                BeattackDelegate dd = (BeattackDelegate)de;
                bool b = dd(damage);
                //Debug.Log(b);
                if (!b)
                    return;
            }
        }

        //MainUI.Beattaacl.Play();
        audio_beattack.Play();

        int realdamge = ReadDamge(damage.normaldamage);
        damage.Add("-" + realdamge.ToString(), this.gameObject, Color.red);
        nowBlood -= realdamge;
        if (nowBlood <= 0)
        {
            nowBlood = 0;
            Death(damage.gameobjectfrom);
            StopCoroutine("bloodup");
            StopCoroutine("magicup");
        }
        GameObject t = (GameObject)Instantiate(bloodSpark, transform.position, bloodSpark.transform.rotation);
        Destroy(t, 4f);
        MainUI.playerui.Reset();
    }


    void Death(GameObject g)
    {
        animation.CrossFade(deathString);
        isDead = true;
        //foreach(GameObject mesh in meshs)
        //{
        //mesh.SendMessage("Fadeout",4f);

        //}
        //mycontrol.CancelInvoke("Walk");
        if (Ondied != null)
        {
            foreach (Delegate d in Ondied.GetInvocationList())
            {
                //Debug.Log("1");
                DiedDelegate dd = (DiedDelegate)d;
                int b = dd(this.gameObject);
                //Debug.Log(b);
                if (b > 0)
                    return;
                //bool b=dd;
            }

            //			int b=Ondied(g,this.gameObject);
            //			if(b>0)
            //			{
            //				return;
            //			}
        }
        startTime = Time.time;
        Invoke("Died", fadeouttime);
        mycontrol.Died();
        //		if(Ondied!=null)
        //		{
        //			bool b=Ondied(g,this.gameObject);
        //			if(!b)
        //			{
        //				return;
        //			}
        //		}

        Attribute a = g.GetComponent<Attribute>();
        a.Addexperience(experienceDrop);

        Moneycontrol aim = g.GetComponent<Moneycontrol>();
        //		Attribute a=g.GetComponent<Attribute>();
        //		a.Addexperience(experiencedrop);
        if (aim)
        {
            int m = UnityEngine.Random.Range(moneyDrop - moneyDropdif, moneyDrop + moneyDropdif);
            aim.AddMoney(m);
            damage.Add("+" + m.ToString(), g, Color.yellow);
        }
    }

    void Died()
    {
        Destroy(this.gameObject);
    }

    void Fadeout()
    {
        if (isDead)
        {
            foreach (GameObject mesh in meshs)
            {
                Color mycolor = mesh.renderer.material.color;
                mycolor.a -= Time.deltaTime / fadeouttime;
                mesh.renderer.material.color = mycolor;
            }
        }
    }

    int ReadDamge(int d)
    {
        int dd = d - protectBlood;
        dd = Mathf.CeilToInt(dd * normalDamagePercent);
        if (dd > 0)
        {
            return dd;
        }
        else
        {
            return 0;
        }
    }

    public int Realattack()
    {
        int d = UnityEngine.Random.Range(normalDamage - normalDamagedif, normalDamage + normalDamagedif);

        return Mathf.CeilToInt(d * normalUp);
    }

    public void Addexperience(int e)
    {
        experience_now += e;
        if (experience_max.Length < 4)
            return;

        if (experience_now >= experience_max[Level - 1])
        {
            experience_now = experience_now - experience_max[Level - 1];
            Levelup();

        }
    }

    public void Levelup()
    {
        //		if(Onlevelup!=null)
        //		{
        //			Onlevelup();
        //		}
        time_bloodUp -= time_bloodDown;
        time_magicUp -= time_magicDown;

        skillUpAvilable += 1;
        Level += 1;
        nowBlood += lp_blood;
        maxBlood += lp_blood;
        nowMagic += lp_magic;
        maxMagic += lp_magic;
        normalDamage += lp_attack;
        //navagent.speed+=lp_speed;


        GameObject g = (GameObject)Instantiate(MainUI.levelupeffect, transform.position, transform.rotation);
        g.transform.parent = transform;
        Destroy(g, 3f);
        MainUI.playerui.Reset();

        if (Onlevelup != null)
        {
            Onlevelup();
        }
    }

    IEnumerator bloodup()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(time_bloodUp);
            if (nowBlood < maxBlood)
            {
                nowBlood += 1;
                MainUI.playerui.Reset();
            }
        }
    }

    IEnumerator magicup()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(time_magicUp);
            if (nowMagic < maxMagic)
            {
                nowMagic += 1;
                MainUI.playerui.Reset();
            }
        }
    }

}
