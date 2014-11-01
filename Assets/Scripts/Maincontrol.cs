using UnityEngine;
using System.Collections;


public enum Commond
{
    Attack, Attack_position, Move, Move_object, Skill
}

public class MainControl : MonoBehaviour
{

    // Use this for initialization
    public GameObject player;
    public static GameObject maincamera;
    Attribute playerattribute;
    GameObject aimnpc;
    GameObject skillaim;
    bool talktest = false;
    public bool cantcontrol { get; set; }
    float talkrange = 30f;
    SkillControl skillcontrol;
    Control playercontrol;
    int nowskill;
    int controlstate = 0; // 1: skill 2:
    public LayerMask layer;
    Vector3 skillposition;

    public GameObject skillup_;
    GameObject skillup;


    void Start()
    {
        playerattribute = player.GetComponent<Attribute>();
        skillcontrol = player.GetComponent<SkillControl>();
        playercontrol = player.GetComponent<Control>();
        cantcontrol = false;
        maincamera = this.gameObject;
        playerattribute.Onlevelup += LevelUp;
    }

    void MouseRaycast()
    {
        if (playerattribute.isDead)
            return;
        if (playercontrol.Bestun)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("1");
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, layer))
            {
                //Debug.Log("2");
                //Docommand(hit.point);
                if (hit.transform.gameObject.tag == "enermy")
                {
                    player.SendMessage("Docommand", hit.transform.gameObject);
                    skillcontrol.Stopskill();
                    //Debug.Log("C");
                    //Doskill(1);
                }
                else if (hit.transform.gameObject.tag == "NPC")
                {
                    aimnpc = hit.transform.gameObject;
                    player.SendMessage("Docommand", aimnpc.transform.position);
                    talktest = true;
                    skillcontrol.Stopskill();
                    //Debug.Log("N");
                }
                else
                {
                    //Debug.Log("p");
                    player.SendMessage("Docommand", hit.point);
                    playercontrol.movepoint = hit.point;
                    skillcontrol.Stopskill();
                }
                //Debug.Log(hit.point);
                //navmeshagent.SetDestination(hit.point);
            }
        }
    }

    void Talktest()
    {
        if (talktest)
        {
            if (Vector3.Distance(player.transform.position, aimnpc.transform.position) <= talkrange)
            {
                player.SendMessage("Starttalk", aimnpc);
                aimnpc.SendMessage("Starttalk", player);
                talktest = false;
                cantcontrol = true;
                Debug.Log("C");
            }
        }
    }

    void Skilltest(int i)
    {
        Vector3 position;
        if (i == 1)
        {
            position = skillaim.transform.position;
        }
        else
        {
            position = skillposition;
        }

        if (Vector3.Distance(position, player.transform.position) < skillcontrol.skills[nowskill].GetRange())
        {
            //skillcontrol.navmeshagent.Stop();
            skillcontrol.Preforskill();
            bool b = playercontrol.Turnround(position);
            if (!b)
                return;
            //Debug.Log("X");
            if (i == 1)
            {
                skillcontrol.skills[nowskill].aimGameobject = skillaim;
                skillcontrol.skills[nowskill].aimPosition = Vector3.zero;
            }
            else
            {
                skillcontrol.skills[nowskill].aimPosition = skillposition;
                skillcontrol.skills[nowskill].aimGameobject = null;
            }
            skillcontrol.Doskill(nowskill);
            controlstate = 0;
            CancelInvoke("Setmoveposition");
            playercontrol.StopInvoked("Walk");
            //Debug.Log("AA");
            //skillcontrol.navmeshagent.Stop();
            //return;
        }

    }


    public void Skill(int i)
    {
        //Debug.Log(i);
        //Debug.Log(skillcontrol.skills.Count);
        if (i >= skillcontrol.skills.Count)
            return;
        //Debug.Log(i);
        if (!skillcontrol.skills[i].isReady)
            return;
        //Debug.Log(i);
        if (playerattribute.nowMagic < skillcontrol.skills[i].GetMagic())
        {
            MainUI.message.Add("No Enough Magic");
            return;
        }
        if (!skillcontrol.skills[i].canUse)
        {
            return;
        }
        //Debug.Log(i);
        //Debug.Log(skillcontrol.skills[i].type);
        if (skillcontrol.skills[i].skillType == SkillType.Range)
        {
            skillcontrol.Doskill(i);
            controlstate = 0;
        }
        else if (skillcontrol.skills[i].skillType == SkillType.Single)
        {
            controlstate = 1;
            nowskill = i;
            //Debug.Log("XX");
            //cantcontrol=true;
        }
        else if (skillcontrol.skills[i].skillType == SkillType.PositionRange)
        {
            controlstate = 3;
            nowskill = i;
        }
    }

    public void Useitem(int i)
    {
        //if(playercontrol.Bestun)
        //	return;
        player.SendMessage("Useitem", i);
    }

    void Setmoveposition()
    {
        player.SendMessage("Docommand", skillaim.transform.position);
        //Debug.Log("X");
    }

    void LevelUp()
    {
        if (!skillup)
        {
            skillup = (GameObject)Instantiate(skillup_, transform.position, transform.rotation);
            skillup.transform.parent = transform;
            skillup.transform.localPosition = new Vector3(-13, -44, 187);
        }

        skillup.GetComponent<SkillUp_Control>().Set(playerattribute.skillUpAvilable);
    }


}
