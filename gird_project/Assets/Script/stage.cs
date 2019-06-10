using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class stageData // 스테이지 데이터
{
    public int level; // 난이도
    public int length; // 스테이지 길이
    public int[,] stage; // 스테이지 그리드
    public string name; // 스테이지 이름
    public int bestCount = 0; // 최소 카운트
    public int bestTime = 0; // 최소 클리어 타임

    public stageData(int level, int[,]stage, string name, int bt, int bc)
    {
        this.level = level;
        this.stage = (int[,])stage.Clone();
        this.length = stage.GetLength(0);
        this.name = name;
        this.bestTime = bt;
        this.bestCount = bc;
    }
}

public class stage {
    public static List<stageData> stageList; // 스테이지 담을 리스트
    public static int[,] stage1 = new int[10, 10] {
    {0,0,1,1,0,0,0,0,0,0},
    {1,1,1,1,1,0,0,0,0,0},
    {1,1,1,1,1,0,0,0,1,0},
    {0,0,1,1,1,0,0,0,0,1},
    {0,0,1,0,0,0,0,0,0,1},
    {0,1,1,1,0,0,0,1,1,0},
    {0,1,1,1,1,1,1,1,1,0},
    {0,0,1,1,1,1,1,1,1,0},
    {0,0,0,1,0,0,0,0,1,0},
    {0,0,1,1,0,0,0,1,1,0}};
   
    public static int[,] stage2 = new int[15, 15] {
    {0,0,0,0,0,0,0,0,0,1,1,1,0,0,0},
    {0,0,0,0,0,0,0,0,1,1,1,1,1,0,0},
    {0,0,0,0,0,0,0,1,1,1,1,0,1,1,1},
    {0,0,0,0,0,0,0,1,1,1,1,1,1,1,0},
    {0,0,0,0,0,0,0,0,1,1,1,1,1,0,0},
    {0,0,0,0,0,0,0,0,0,1,1,1,0,0,0},
    {0,0,0,0,0,0,0,0,1,1,1,1,1,0,0},
    {1,0,0,0,0,0,1,1,1,1,1,1,1,1,0},
    {1,1,1,0,0,1,1,1,0,0,0,1,1,1,0},
    {1,1,1,1,1,1,1,0,1,1,1,0,1,1,0},
    {0,1,1,1,1,1,0,1,1,1,1,0,1,1,0},
    {0,1,1,1,1,1,1,1,1,0,0,1,1,0,0},
    {0,0,1,1,1,1,1,1,1,1,1,1,0,0,0},
    {0,0,0,0,1,1,0,1,1,1,0,0,0,0,0},
    {0,0,0,0,0,0,1,1,1,1,1,1,0,0,0}};

    public static int[,] stage3 = new int[10, 10] {
    {0,1,1,0,0,0,0,1,1,0},
    {0,1,0,1,0,0,1,0,1,0},
    {0,1,0,0,1,1,0,0,1,0},
    {0,1,0,0,0,0,0,0,1,0},
    {1,0,0,0,0,0,0,0,0,1},
    {1,0,0,1,0,0,1,0,0,1},
    {1,0,0,1,0,0,1,0,0,1},
    {1,0,0,0,0,0,0,0,0,1},
    {0,1,0,0,0,0,0,0,1,0},
    {0,0,1,1,1,1,1,1,0,0}};

    public static int[,] stage4 = new int[20, 20] {
    {0,0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,1,0},
    {0,0,0,0,1,0,0,1,0,0,1,0,0,1,1,1,0,1,0,0},
    {0,0,0,0,1,0,0,1,0,0,1,0,1,0,1,1,1,0,0,0},
    {0,0,0,0,1,0,0,1,0,0,1,1,0,1,0,1,1,1,0,0},
    {0,0,1,1,1,0,0,1,0,1,1,0,1,0,1,0,1,1,0,0},
    {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
    {0,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,0},
    {0,1,0,0,1,1,1,1,0,0,0,0,1,1,1,1,0,0,1,0},
    {0,1,1,0,0,1,1,0,0,1,1,0,0,1,1,0,0,1,1,0},
    {0,0,1,1,0,0,0,0,1,1,1,1,0,0,0,0,1,1,0,0},
    {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
    {0,0,1,1,0,0,1,0,0,1,0,0,0,0,0,1,0,1,0,0},
    {0,0,1,0,0,0,0,1,0,0,0,1,0,1,0,0,1,1,0,0},
    {0,0,1,0,1,0,0,0,0,1,0,0,0,0,1,0,0,1,0,0},
    {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}};

    public static int[,] stage5 = new int[25, 25] {
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,1,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,1,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,1,1,1,1,1,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0},
    {0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0},
    {0,0,0,0,1,1,1,0,0,0,0,1,1,1,1,0,0,1,1,1,0,0,0,0,0},
    {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
    {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
    {0,0,0,1,0,0,1,1,1,1,1,1,0,0,1,0,0,0,0,0,0,0,0,0,0},
    {0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
    {0,1,0,1,0,0,1,1,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0},
    {1,0,0,1,0,0,1,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0},
    {1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0},
    {0,0,0,1,0,0,1,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0},
    {0,0,0,1,0,0,1,1,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0},
    {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,1,1,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
    {0,0,1,0,0,1,0,1,0,1,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,1,1,1,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};

    public static int getStage(int i) // 스테이지 길이 반환
    {
        return stageList[i - 1].stage.GetLength(0);
    }

    public static void init() // 초기화
    {
        stageList = new List<stageData>();

        loadStage();

        // 스테이지가 하나도 없을 경우 기본 스테이지 생성
        if(stageList.Count == 0)
        {
            stageList.Add(new stageData(1, stage3, "고양이",0,0));
            stageList.Add(new stageData(2, stage1, "강아지", 0, 0));
            stageList.Add(new stageData(3, stage2, "새", 0, 0));
            stageList.Add(new stageData(4, stage4, "케이크", 0, 0));
            stageList.Add(new stageData(5, stage5, "눈사람", 0, 0));
        }
    }
    public static void saveStage() // 리스트에 있는 스테이지 저장
    {
        for(int i=0;i<stageList.Count;i++)
        {
            
            PlayerPrefs.SetInt("stageLevel" + (i+1), stageList[i].level);
            PlayerPrefs.SetString("stageName" + (i + 1), stageList[i].name);
            PlayerPrefs.SetInt("stageLength" + (i+1), stageList[i].length);
            PlayerPrefs.SetInt("bestTime" + (i + 1), stageList[i].bestTime);
            PlayerPrefs.SetInt("bestCount" + (i + 1), stageList[i].bestCount);
            char []str = new char[stageList[i].length * stageList[i].length + 1];
            for (int j = 0; j < stageList[i].length; j++)
                for (int k = 0; k < stageList[i].length; k++)
                    str[j * stageList[i].length + k] = (char)(stageList[i].stage[j, k] + '0');

            string str2 = new string(str);
            PlayerPrefs.SetString("stageMatrix" + (i+1), str2);
            Debug.Log((i + 1) + "번째 저장 완료");
        }
       
        PlayerPrefs.SetInt("stageNumber", stageList.Count);
    }
    public static void loadStage() // 저장된 스테이지 불러오기
    {
        int num = PlayerPrefs.GetInt("stageNumber");

        stageList.Clear();
        for (int i = 0; i < num; i++)
        {
            string str;
            int lev;
            int len;
            string name;
            int bt, bc;
            str = PlayerPrefs.GetString("stageMatrix" + (i+1));
            lev = PlayerPrefs.GetInt("stageLevel" + (i+1));
            len = PlayerPrefs.GetInt("stageLength" + (i+1));
            name = PlayerPrefs.GetString("stageName" + (i + 1));
            bt = PlayerPrefs.GetInt("bestTime" + (i + 1));
            bc = PlayerPrefs.GetInt("bestCount" + (i + 1));
            name = PlayerPrefs.GetString("stageName" + (i + 1));

            int[,] tmp = new int[len, len];

            for (int k = 0; k < len; k++)
                for (int j = 0; j < len; j++)
                    tmp[k, j] = str[k * len + j] -'0';

            stageList.Add(new stageData(lev, (int[,])tmp.Clone(), name, bt, bc));
        }

    }

}
