using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Message : MonoBehaviour
{

    public GameObject panel;
    public Vector3 offset;

    List<GameObject> panels;

    void Start()
    {
        panels = new List<GameObject>();
    }
    public void Add(string s)
    {
        GameObject g = (GameObject)Instantiate(panel, transform.position, panel.transform.rotation);
        g.transform.parent = transform;
        g.GetComponentInChildren<UILabel>().text = s;
        StartCoroutine("Delay", g);

        foreach (GameObject p in panels)
        {
            if (p == null)
            {
                panels.Remove(p);
            }
            else
            {
                p.transform.Translate(offset);
            }

        }
        panels.Add(g);
    }

    IEnumerator Delay(GameObject g)
    {
        yield return new WaitForSeconds(4f);
        panels.Remove(g);
        Destroy(g);
    }
}
