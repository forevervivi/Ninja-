using UnityEngine;
using System.Collections;
using System;


public class Control : MonoBehaviour
{

    // Use this for initialization
    Command nowcommand;
    public State nowstate { get; set; }
    Vector3 movedirection;
    Vector3 aimposition;
    Vector3 jumpdirection;
    public Vector3 movepoint { get; set; }
    //float speed=15f;
    //float attackrange=40f;
    public Camera maincamera;
    //	public WeaponTrail trail;
    NavMeshAgent navmeshagent;
    CharacterController charactercontroller;
    //	AnimationController animotion;
    GameObject aimgameobject;
    public GameObject bullet;
    GameObject stunspark;
    GameObject stunspark_;
    //public Transform bullettransform;
    bool rotatable;
    bool moveable;
    bool attackable;
    public bool Bestun { get; set; }
    public bool isrange;
    public float attackdelay = 0.3f;
    float rotatespeed;
    public float attackrange = 30f;
    public float attackintervel { get; set; }
    public float attackintervel_ = 0.5f;
    //Skill nowskill;
    SkillControl myskill;
    Attribute myattribute;
    Attribute aimattribute;

    public delegate Damage_ Attackdelegate(Damage_ d);
    public Attackdelegate Onattack;
    //float attackrange;

    public AudioSource run;

    //Vector3 localdirection;

    IEnumerator Start()
    {
        attackintervel = attackintervel_;
        nowcommand = Command.Idle;
        navmeshagent = GetComponent<NavMeshAgent>();
        rotatespeed = navmeshagent.angularSpeed;
        nowstate = State.Idle;
        attackable = true;
        myskill = GetComponent<SkillControl>();
        charactercontroller = GetComponent<CharacterController>();
        myattribute = GetComponent<Attribute>();
        Bestun = false;
        yield return 0;
        stunspark = MainUI.stunspark;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (myattribute.isDead)
        {
            if (navmeshagent.velocity.sqrMagnitude > 0)
            {
                navmeshagent.Stop();
            }
            return;
        }

        if (Bestun)
        {
            return;
        }

        if (nowstate == State.Idle)
        {
            if (nowcommand == Command.Move)
            {
                Movetest();
            }
            else if (nowcommand == Command.Attack)
            {
                Attackmove();
            }
            else if (nowcommand == Command.Default)
            {

            }
            else if (nowcommand == Command.Skill)
            {
                if (!animation.IsPlaying("attackStab"))
                {
                    Entertoidle();
                }
            }
        }
        else if (nowstate == State.Attack)
        {
            if (!publicclass.Isplaying(myattribute.attackstring, this.gameObject))
            {
                attackable = false;
                Invoke("Attackable", attackintervel);
                nowstate = State.Idle;
                //animation.CrossFade("idle");
                publicclass.Rangdomanimotion(myattribute.idlestring, this.gameObject);
                if (aimattribute.isdied)
                    Entertoidle();
                //animation["aa"].
            }
        }
        else if (nowstate == State.Skill)
        {

        }
        else if (nowstate == State.Death)
        {
            //			jumpdirection.y-=5f*Time.deltaTime;
            //			CollisionFlags flag=charactercontroller.Move(jumpdirection*Time.deltaTime);
            //			bool ground=(flag&CollisionFlags.CollidedBelow)!=0;
            //			if(ground)
            //			{
            //				nowstate=State.Idle;
            //			}
        }
        else if (nowstate == State.Talk)
        {
            if (Turnround(aimgameobject.transform.position))
            {
                nowstate = State.Default;
            }
        }


    }

    void MouseRaycast()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("1");
            Ray ray = maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("2");
                //Docommand(hit.point);
                if (hit.transform.gameObject.tag == "enermy")
                {
                    Docommand(hit.transform.gameObject);
                    //Doskill(1);
                }
                else
                {
                    Docommand(hit.point);
                }
                Debug.Log(hit.point);
                //navmeshagent.SetDestination(hit.point);
            }
        }

        //		if(Input.GetButtonDown("Jump"))
        //		{
        //			animation.CrossFade("jump");
        //			animation.CrossFadeQueued("jumpfall");
        //			nowstate=State.Jump;
        //			jumpdirection=new Vector3(0f,10f,0f);
        //		}

    }

    public void Docommand(GameObject g)
    {
        if (nowcommand == Command.Attack && aimgameobject == g)
            return;

        if (myattribute.isdied)
            return;
        if (IsInvoking("Walk"))
        {
            CancelInvoke("Walk");
        }

        aimgameobject = g;
        aimattribute = aimgameobject.GetComponent<Attribute>();
        if (aimattribute.isdied)
            Docommand(g.transform.position);
        nowcommand = Command.Attack;
        animation.CrossFade(myattribute.runstring);
        nowstate = State.Idle;
    }

    public void Docommand(Vector3 position)
    {
        if (myattribute.isdied)
            return;
        Stopdealdamge();
        navmeshagent.SetDestination(position);
        //nowcommand=Command.Move;
        animation.CrossFade(myattribute.runstring);
        //	StartCoroutine("Walk");
        Invoke("Walk", 0.2f);
        nowstate = State.Idle;
        nowcommand = Command.Default;
        //movepoint=position;
        //movedirection=publicclass.Ytozero(position-transform.position);
        //movedirection=transform.TransformDirection(movedirection);
    }

    //	void Doskill(int i)
    //	{
    //		if(myattribute.isdied)
    //			return;
    //		animation.CrossFade("attackStab");
    //		navmeshagent.Stop();
    //		nowcommand=Command.Skill;
    //	}

    void Walk()
    {
        //yield return 10;
        nowcommand = Command.Move;
        animation.CrossFade(myattribute.runstring);
        //run.Play();
        //yield return null;
        //animation.CrossFade("walk");
    }

    void Movetest()
    {
        if (navmeshagent.velocity.sqrMagnitude == 0)
        {
            Entertoidle();
            //Debug.Log("XX");
        }

    }


    public void Entertoidle()
    {
        //animation.CrossFade("idle");
        nowcommand = Command.Idle;
        nowstate = State.Idle;
        publicclass.Rangdomanimotion(myattribute.idlestring, this.gameObject);
        //run.Stop();
    }

    public void Statetoidle()
    {
        //run.Stop();
        nowstate = State.Idle;
        if (nowcommand == Command.Idle)
        {
            publicclass.Rangdomanimotion(myattribute.idlestring, this.gameObject);
        }
        else if (nowcommand == Command.Move)
        {
            navmeshagent.SetDestination(movepoint);
            animation.CrossFade(myattribute.runstring);
            Invoke("Walk", 0.2f);
            //nowstate=State.Idle;
            nowcommand = Command.Default;
            //movepoint=position;
        }
        else if (nowcommand == Command.Attack)
        {
            Docommand(aimgameobject);
        }
    }

    void Attackmove()
    {
        if (Vector3.Distance(transform.position, aimgameobject.transform.position) <= attackrange)
        {
            navmeshagent.Stop();
            bool okay = Turnround(aimgameobject.transform.position);
            if (okay)
            {
                if (!attackable)
                {
                    return;
                }
                Startattack();
            }
        }
        else
        {
            navmeshagent.SetDestination(aimgameobject.transform.position);
            //Debug.Log("C");
            if (!animation.IsPlaying(myattribute.runstring))
                animation.CrossFade(myattribute.runstring);
            if (aimattribute.isdied)
                Entertoidle();
        }
    }

    void Startattack()
    {
        //run.Stop();
        nowstate = State.Attack;
        //animation.CrossFade("attackSlash");
        Invoke("Dealdamage", attackdelay);
        publicclass.Rangdomanimotion(myattribute.attackstring, this.gameObject);
        //AnimationState aa=animation["attackSlash"];
        //animotion.PlayAnimation(aa);
        //trail.StartTrail(0.5f,0.4f);
    }

    void Starttalk(GameObject g)
    {
        //run.Stop();
        aimgameobject = g;
        nowstate = State.Talk;
        //publicclass.Rangdomanimotion(myattribute.idlestring,this.gameObject);
        navmeshagent.Stop();
        StartCoroutine("Idledelay");
    }

    IEnumerator Idledelay()
    {
        yield return 10;
        publicclass.Rangdomanimotion(myattribute.idlestring, this.gameObject);
        yield return null;
    }

    void EndTalk()
    {
        Entertoidle();
    }

    void Dealdamage()
    {
        if (myattribute.isdied)
            return;

        int damage = myattribute.Realattack();
        Damage_ d = new Damage_(damage, this.gameObject, aimgameobject);

        if (Onattack != null)
        {
            foreach (Delegate dd in Onattack.GetInvocationList())
            {
                //Debug.Log("1");
                Attackdelegate d_ = (Attackdelegate)dd;
                d = d_(d);
                //Debug.Log(b);
                //				if(b>0)
                //					return;
            }
        }

        if (isrange)
        {
            GameObject g = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            g.GetComponent<bullet>().Set(d);

        }
        else
        {
            if (aimgameobject)
            {
                //int damage=Random.Range(myattribute.normaldamage-myattribute.normaldamagedif,myattribute.normaldamage+myattribute.normaldamagedif);
                //				int damage=myattribute.Realattack();
                //				Damage_ d=new Damage_(damage,this.gameObject,aimgameobject);
                aimgameobject.SendMessage("Beattack", d);
            }
        }
    }

    void Stopdealdamge()
    {
        if (IsInvoking("Dealdamage"))
        {
            CancelInvoke("Dealdamage");
        }
    }

    public bool Turnround(Vector3 position)
    {

        Vector3 direction = new Vector3(position.x - transform.position.x, 0f, position.z - transform.position.z);
        Vector3 playerdirection = new Vector3(transform.forward.x, 0f, transform.forward.z);
        Vector3 x1 = Vector3.Cross(playerdirection, direction.normalized);
        float dotvalue = Vector3.Dot(playerdirection, direction.normalized);
        float angle = Mathf.Acos(dotvalue) * Mathf.Rad2Deg;



        if (angle > 4 || angle < -4)
        {
            //if(!animation.IsPlaying(myattribute.runstring))
            //	animation.CrossFade(myattribute.runstring);
            bool turn;
            if (x1.y < 0)
            {
                turn = false;

            }
            else
            {
                turn = true;
            }

            if (turn)
            {
                transform.Rotate(Vector3.up * rotatespeed * Time.deltaTime);

            }
            else
            {
                transform.Rotate(Vector3.down * rotatespeed * Time.deltaTime);
            }

            return false;
        }
        else
        {
            return true;
        }
    }

    void Attackable()
    {
        attackable = true;
    }

    public void Died()
    {
        //run.Stop();
        CancelInvoke();
        nowstate = State.Death;
        nowcommand = Command.Default;
    }

    public void Bestund(float time)
    {
        //run.Stop();
        if (myattribute.isdied)
            return;

        if (Bestun)
        {
            StopCoroutine("Refromstun");
            //StartCoroutine("Refromstun",time);
            //Bestun=true;
        }
        else
        {
            stunspark_ = (GameObject)Instantiate(stunspark, transform.position, stunspark.transform.rotation);
            stunspark_.transform.Translate(new Vector3(0f, 23f, 0f));
            stunspark_.transform.parent = transform;
        }
        StopInvoked("Dealdamage");
        StopInvoked("Walk");
        animation.CrossFade(myattribute.stunstring);
        StartCoroutine("Refromstun", time);
        Bestun = true;
        navmeshagent.Stop();
        //myattribute.Bestun(time);
        myskill.Bestun(time);


    }

    IEnumerator Refromstun(float time)
    {
        yield return new WaitForSeconds(time);
        Bestun = false;
        publicclass.Rangdomanimotion(myattribute.idlestring, this.gameObject);
        Destroy(stunspark_);
    }

    public void StopInvoked(string s)
    {
        if (IsInvoking(s))
            CancelInvoke(s);
    }
}
