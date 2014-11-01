using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int speed = 50;

    Camera myCamera;
    Vector3 newPosition;
    Vector3 curPosition;
    Animator playerAnimator;

    //用来存储用于Animator中的String的Hash值
    //状态机
    int idelState;
    int walkState;
    int runState;

    //Animator中的参数
    int isWalk;
    int isIdel;

    void Awake()
    {
        myCamera = GameObject.FindGameObjectWithTag("MyCamera").GetComponent<Camera>();
        newPosition = this.transform.position;
        playerAnimator = this.GetComponent<Animator>();

        idelState = Animator.StringToHash("Base Layer.Idel");
        walkState = Animator.StringToHash("Base Layer.Walk");
        runState = Animator.StringToHash("Base Layer.Run");

        isWalk = Animator.StringToHash("IsWalk");
        isIdel = Animator.StringToHash("IsIdel");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Floor")
                {
                    newPosition = hit.point;
                    //  curPosition = this.transform.position;
                    //print("newPostion :" + newPosition);
                }
            }
        }

        //如果点击的位置距离当前位置超过一定值才控制人物移动
        if (Vector3.Magnitude(newPosition - this.transform.position) > 1.0f)
        {
            //print("isWalk！！！   newPosition: "+ newPosition);

            //控制人物执行Walk动画
            playerAnimator.SetBool(isWalk, true);
            playerAnimator.SetBool(isIdel, false);

            //LookAt（）来调整人物方向面对newPosition。
            this.transform.LookAt(newPosition);

            //控制人物以固定速度运动到鼠标点击的地点
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            playerAnimator.SetBool(isIdel, true);
            playerAnimator.SetBool(isWalk, false);
        }
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.tag == "Wall")
    //    {
    //        //Player和墙壁发生碰撞
    //        print("撞到墙了…………");

    //        //Player撞墙后把newPosition设为当前position，从而使Player停止移动。
    //        newPosition = this.transform.position;
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
           // Player和墙壁发生碰撞
            print("撞到墙了…………");

           // Player撞墙后把newPosition设为当前position，从而使Player停止移动。
            newPosition = this.transform.position;
        }
    }
}
