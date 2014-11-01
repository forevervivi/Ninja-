using UnityEngine;
using System.Collections;

public class MainCameraFollow : MonoBehaviour
{
    public float smooth = 1.5f;  //摄像机移动到新的合适位置的速度参数。

    Transform player;
    Vector3 relCameraPos;  //Camera相对于Player的位置。
    float relCameraPosMag; //Camera和Player的距离。
    Vector3 newPos;  //新的Camera位置

    // Use this for initialization
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        relCameraPos = camera.transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude;
        newPos = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //摄像机只跟随玩家玩家位置一定距离
        camera.transform.position = player.transform.position + relCameraPos;

    }

}
