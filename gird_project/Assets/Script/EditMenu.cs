using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditMenu : MonoBehaviour {
    public static int length;

    string displaySize(int len, int level) // 레벨만큼 별 칠해진 스트링 반환
    {
        string str = null;

        for (int i = 0; i < 5; i++)
            if (i < level)
                str += "★";
            else
                str += "☆";


        return " 삭제\n(" + len + "X" + len + ")\n" + str;
    }
    private void OnGUI() // 버튼 및 레이블 출력
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), MenuManager.inst.background);
        int gap = Screen.width / 13;
        float[] cnt = new float[4];
        int[] stageCount = new int[4];
        var Style = GUI.skin.GetStyle("Button");
        Style.fontSize = (int)gap / 4;
        Style.fontStyle = FontStyle.Bold;


        for (int i = 0; i < stage.stageList.Count; i++)
        {
            for(int k=0;k<4;k++)
            {
                if (stage.stageList[i].length == 10+5*k)
                {
                    if (GUI.Button(new Rect(gap * (0.5f+2.5f *k), Screen.height / 4 + gap * cnt[k], gap * 2, gap),
                        stage.stageList[i].name + displaySize(stage.stageList[i].length, stage.stageList[i].level), Style))
                    {
                        stage.stageList.RemoveAt(i);
                        stage.saveStage();
                    }
                    cnt[k] += 1.1f;
                }
            }
        }

        if (GUI.Button(new Rect(Screen.width - gap * 3, gap / 2, gap * 2, gap), "뒤로가기", Style)) // 메뉴로 가는 버튼
            SceneManager.LoadScene("MenuScene");

        GUI.color = Color.black;
        var Style2 = GUI.skin.GetStyle("Label");
        Style2.fontSize = (int)gap / 3;
        Style2.fontStyle = FontStyle.Bold;

        GUI.Label(new Rect(gap * 10.5f, Screen.height / 4 , gap * 2, gap), "로직 생성", Style2); //

        for (int i = 0; i <stage.stageList.Count;i++ )
        {
            stageCount[stage.stageList[i].stage.GetLength(0)/5-2]++;
        }
        GUI.color = Color.white;
        for (int i=1;i<=4;i++)
            if(stageCount[i-1] != 5)
            if (GUI.Button(new Rect(gap * 10.5f, Screen.height / 4 + gap * 1.1f * i, gap * 2, gap), 
                (5+i*5)+" X "+ (5 + i * 5) + "\n로직 생성", Style)) // 로직 생성 버튼
            {
                length = 5+ 5*i;
                SceneManager.LoadScene("CreateScene");
            }
        GUI.color = Color.black;
        var Style3 = GUI.skin.GetStyle("Label");
        Style3.fontSize = (int)gap;
        Style3.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(gap * 2, 0.2f * gap, gap * 8, gap * 2), "로직 제작 & 삭제", Style3);


    }
}

