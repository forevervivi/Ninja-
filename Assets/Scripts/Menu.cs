using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    
    public void OnButtonStart(){
        print("点击了Start按键");
        Application.LoadLevel(1);    
    }
    public void OnButtonExit() {
        print("点击了Exit按键");
        Application.Quit();
    }
}
