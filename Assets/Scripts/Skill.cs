using UnityEngine;
using System.Collections;

public enum SkillType
{
    Range,
    PositionRange,
    Single,
    Heal
}

public class Skill
{
    public bool isReady;
    public string name;
    public string description;
    public string skillgbname;
    public string animationString;
    public string iconName;
    public float[] range;
    public SkillType skillType;
    public int[] magicNeeded;
    public GameObject skillGameobject;
    public GameObject skillButton;
    public GameObject aimGameobject;
    public GameObject gameobjectfrom;
    public float delay;
    public int[] cd;
    public int[] damage;
    public int level { get; set; }
    public int[] substain;
    public bool canUse;
    public Vector3 aimPosition;

    public Skill(string name, string description, string skillgbname, string animationString, int[] magicNeeded,
        float[] range, SkillType skillType, float delay, int[] cd, string iconName, int level, int[] damage, int[] sub, bool canUse)
    {
        this.isReady = true;
        this.name = name;
        this.description = description;
        this.skillgbname = skillgbname;
        this.animationString = animationString;
        this.magicNeeded = magicNeeded;
        this.range = range;
        this.skillType = skillType;
        this.delay = delay;
        this.cd = cd;
        this.iconName = iconName;
        this.level = level;
        this.damage = damage;
        this.substain = sub;
        this.canUse = canUse;
    }

    public int GetCd()
    {
        if (this.level < 1)
            return 0;
        return this.cd[this.level - 1];
    }

    public int Getsub()
    {
        if (this.level < 1)
            return 0;
        return this.substain[this.level - 1];
    }

    public int GetMagic()
    {
        if (this.level < 1)
            return 0;
        return this.magicNeeded[this.level - 1];
    }

    public float GetRange()
    {
        if (this.level < 1)
            return 0;
        return this.range[this.level - 1];
    }

    public int GetDamage()
    {
        if (this.level < 1)
            return 0;
        return this.damage[this.level - 1];
    }

    public string GetString()
    {
        string s = "";
        if (this.GetDamage() != 0)
        {
            s += "Damage:";
            for (int i = 0; i < damage.Length; i++)
            {
                s += damage[i].ToString();
                if (i == damage.Length - 1)
                {
                    s += "\n";
                    break;
                }
                s += "/";
            }
        }
        s += "Magic:";
        for (int i = 0; i < magicNeeded.Length; i++)
        {
            s += magicNeeded[i].ToString();
            if (i == magicNeeded.Length - 1)
            {
                s += "\n";
                break;
            }
            s += "/";
        }
        if (this.GetCd() != 0)
        {
            s += "CD:";
            for (int i = 0; i < cd.Length; i++)
            {
                s += cd[i].ToString();
                if (i == cd.Length - 1)
                {
                    s += "\n";
                    break;
                }
                s += "/";
            }
        }
        if (this.Getsub() != 0)
        {
            s += "Substain:";
            for (int i = 0; i < substain.Length; i++)
            {
                s += substain[i].ToString();
                if (i == substain.Length - 1)
                {
                    s += "\n";
                    break;
                }
                s += "/";
            }
        }

        return s;
    }

    public bool LevelUp()
    {
        if (level + 1 <= 4)
        {
            level += 1;
            return true;
        }
        else
        {
            return false;
        }
    }
}
