using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public Texture background;
    public Texture title;
    // Use this for initialization
    public static MenuManager inst;
    void Start () { // 해상도 조정

        int w = Screen.width;
        int h = Screen.height;
        if((float)w/h< (float)1280/720) // 
            Screen.SetResolution(w, (int)((float)w/1280 * 720), Screen.fullScreen);
        inst = this;
        stage.init(); // 스테이지 클래스 초기화
    }
    public static MenuManager getInst()
    {
        return inst;
    }
    private void OnGUI() // 버튼 작성
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), background);
        
        int gap = Screen.width / 13;
        GUI.Label(new Rect(gap*1.5f, Screen.height / 4, gap*10, gap*3), title);
        var Style = GUI.skin.GetStyle("Button");
        Style.fontSize = (int)gap / 3;
        Style.fontStyle = FontStyle.Bold;
        if (GUI.Button(new Rect(gap, Screen.height / 4 * 3, gap*2, gap), "로직 선택", Style))
            SceneManager.LoadScene("SelectScene");

        if (GUI.Button(new Rect(gap*4, Screen.height / 4 * 3, gap * 2, gap), "로직 제작", Style))
            SceneManager.LoadScene("EditScene");

        if (GUI.Button(new Rect(gap*7, Screen.height / 4 * 3, gap * 2, gap), "로직 도감", Style))
            SceneManager.LoadScene("CollectionScene");

        if (GUI.Button(new Rect(gap * 10, Screen.height / 4 * 3, gap * 2, gap), "게임 종료", Style))
            Application.Quit();
    }
}

