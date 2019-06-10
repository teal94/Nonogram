using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour {
    public static int stageNum = 0;

    string displaySize(int len, int level) // 레벨에 따른 별 그리기
    {
        string str = null;

        for (int i = 0; i < 5; i++)
            if (i < level)
                str += "★";
            else
                str += "☆";

        return "\n(" + len + "X" + len + ")\n"+str;
    }
    private void OnGUI() // 버튼 및 레이블 그리기
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), MenuManager.inst.background);
        int gap = Screen.width / 13;
        float []cnt = new float[4];
        var Style = GUI.skin.GetStyle("Button");
        Style.fontSize = (int)gap / 4;
        Style.fontStyle = FontStyle.Bold;
        for (int i = 0; i < stage.stageList.Count; i++)
        {
            if (stage.stageList[i].length == 10) // 로직의 길이가 10인경우
            {
                if (GUI.Button(new Rect(gap, Screen.height / 4 + gap * cnt[0], gap * 2, gap), stage.stageList[i].name 
                    + displaySize(stage.stageList[i].length, stage.stageList[i].level), Style))
                {
                    stageNum = 1 + i;
                    SceneManager.LoadScene("StageScene");

                }
                cnt[0] += 1.1f;
            }
            else if (stage.stageList[i].length == 15) // 로직의 길이가 15인경우
            {
                if (GUI.Button(new Rect(gap * 4, Screen.height / 4 + gap * cnt[1], gap * 2, gap), stage.stageList[i].name
                    + displaySize(stage.stageList[i].length, stage.stageList[i].level), Style))
                {
                    stageNum = 1 + i;
                    SceneManager.LoadScene("StageScene");
                }
                cnt[1] += 1.1f;
            }
            else if (stage.stageList[i].length == 20) // 로직의 길이가 20인경우
            {
                if (GUI.Button(new Rect(gap * 7, Screen.height / 4 + gap * cnt[2], gap * 2, gap), stage.stageList[i].name 
                    + displaySize(stage.stageList[i].length, stage.stageList[i].level), Style))
                {
                    stageNum = 1 + i;
                    SceneManager.LoadScene("StageScene");
                }
                cnt[2] += 1.1f;
            }
            else if (stage.stageList[i].length == 25) // 로직의 길이가 25인경우
            {
                if (GUI.Button(new Rect(gap * 10, Screen.height / 4 + gap * cnt[3], gap * 2, gap), stage.stageList[i].name 
                    + displaySize(stage.stageList[i].length, stage.stageList[i].level), Style))
                {
                    stageNum = 1 + i;
                    SceneManager.LoadScene("StageScene");
                }
                cnt[3] += 1.1f;
            }
        }
        // 뒤로가기 버튼
        if (GUI.Button(new Rect(Screen.width - gap * 3, gap / 2, gap * 2, gap), "뒤로가기", Style))
            SceneManager.LoadScene("MenuScene");

        GUI.color = Color.black;
        var Style2 = GUI.skin.GetStyle("Label");
        Style2.fontSize = (int)gap;
        Style2.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(gap*3, 0.2f * gap, gap * 5, gap * 2), "로직 선택", Style2);
    }
}



