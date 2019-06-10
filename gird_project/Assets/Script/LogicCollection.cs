using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicCollection : MonoBehaviour {
    private static Material mat;
    Vector3 startDrawgrid;
    int selectNum = 0;
    float gap;
    int boldWidth = 5;
    // Use this for initialization
    void Start () { //초기화
        gap = Screen.width / 19;
        startDrawgrid = new Vector3(gap, Screen.height/3);
        stage.stageList.Sort(delegate(stageData A, stageData B)
        {
            if (A.stage.GetLength(0) > B.stage.GetLength(0))
                return 1;
            else return -1;
        });
    }
	

    void Awake() // GL에서 색상을 사용하기 위한 마테리얼 생성
    {
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        mat = new Material(shader);
        mat.hideFlags = HideFlags.HideAndDontSave;
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        mat.SetInt("_ZWrite", 0);
    }
    void OnApplicationQuit() // 마테리얼 제거
    {
        DestroyImmediate(mat);
    }
    private void OnGUI() // 버튼 그리기
    { 
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), MenuManager.inst.background);
        var Style = GUI.skin.GetStyle("Button");
        Style.fontSize = (int)gap / 3;
        Style.fontStyle = FontStyle.Bold;

        // 메뉴로 가는 버튼
        if (GUI.Button(new Rect(Screen.width - gap * 3 , gap / 2 , gap * 2, gap), "뒤로가기", Style))
            SceneManager.LoadScene("MenuScene");

        // 다음 3개의 도감을 보는 버튼
        if (stage.stageList.Count > selectNum * 3 + 3)
            if (GUI.Button(new Rect(Screen.width - gap * 5.5f, gap / 2, gap * 2, gap), "다음", Style))
                selectNum++;

        // 이전 3개의 도감을 보는 버튼
        if (selectNum != 0)
            if (GUI.Button(new Rect(Screen.width - gap * 8, gap / 2, gap * 2, gap), "이전", Style))
                selectNum--;

        var Style2 = GUI.skin.GetStyle("Label");
        Style2.fontSize = (int)gap / 2;
        Style2.fontStyle = FontStyle.Bold;
        GUI.color = Color.black;

        // 도감별 텍스트 출력
        for (int j = selectNum * 3; j < 3 + selectNum * 3; j++)
        {

            if (j == stage.stageList.Count)
                break;

            string str = null;

            for (int i = 0; i < 5; i++)
                if (i < stage.stageList[j].level)
                    str += "★";
                else
                    str += "☆";
            string timeStr = null;
            int minutes = (int)stage.stageList[j].bestTime / 60;
            int second = (int)stage.stageList[j].bestTime % 60;

            timeStr = minutes.ToString("D2") + ":";
            timeStr += second.ToString("D2");
            

            Vector2 tempStartGrid = startDrawgrid;
            tempStartGrid.x = startDrawgrid.x + gap * 6 * (j - selectNum * 3);
            int len = stage.stageList[j].stage.GetLength(0);
            GUI.Label(new Rect(tempStartGrid.x, Screen.height - tempStartGrid.y, gap * 5, gap), 
                stage.stageList[j].name +" ("+ len+"X"+len+")", Style2);
            GUI.Label(new Rect(tempStartGrid.x, Screen.height - tempStartGrid.y + gap*0.8f, gap * 5, gap), str, Style2);
            GUI.Label(new Rect(tempStartGrid.x, Screen.height - tempStartGrid.y + gap * 1.6f, gap * 5, gap),
                "클리어 타임 : " + timeStr, Style2);
            GUI.Label(new Rect(tempStartGrid.x, Screen.height - tempStartGrid.y + gap*2.4f, gap * 5, gap),
                "최소 카운트 : "+ stage.stageList[j].bestCount, Style2);
        }
            OnPostRender();
    }

    // GL을 이용하여 도감 로직 그리기
    private void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("마테리얼 없음!");
            return;
        }
        GL.PushMatrix();
        float tmpgap = gap;
        
        mat.SetPass(0);
        GL.LoadOrtho();
        for(int j= selectNum*3; j<3+ selectNum*3; j++)
        {
            if (j == stage.stageList.Count)
                break;
            Vector2 tempStartGrid = startDrawgrid;
            tempStartGrid.x = startDrawgrid.x + gap * 6 *(j- selectNum * 3);
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            // 도감 하얀 사각형 그리기
            GL.Vertex(new Vector3((tempStartGrid.x) / Screen.width, (tempStartGrid.y + gap * 5) / Screen.height));
            GL.Vertex(new Vector3((tempStartGrid.x) / Screen.width, (tempStartGrid.y) / Screen.height));
            GL.Vertex(new Vector3((tempStartGrid.x + gap * 5-3) / Screen.width, tempStartGrid.y / Screen.height));
            GL.Vertex(new Vector3((tempStartGrid.x + gap * 5-3) / Screen.width, (tempStartGrid.y + gap*5) / Screen.height));
            GL.End();
            GL.Begin(GL.QUADS); // 미니맵 그리기
            GL.Color(Color.gray);
            int length = stage.stageList[j].stage.GetLength(0);
            tmpgap = (gap * ((float) 5 / length));
            Debug.Log("그림 = " + stage.stageList[j].name);

            // 그리드 내부 그리기
            for (int i = 0; i < length; i++)
                for (int k = 0; k < length; k++)
                {
                    if (stage.stageList[j].stage[i, k] == 1)
                    {
                        GL.Vertex(new Vector3((tempStartGrid.x + tmpgap * k) / Screen.width, 
                            (tempStartGrid.y + tmpgap * length - tmpgap * i) / Screen.height));
                        GL.Vertex(new Vector3((tempStartGrid.x + tmpgap * k) / Screen.width, 
                            (tempStartGrid.y - tmpgap + tmpgap * length - tmpgap * i) / Screen.height));
                        GL.Vertex(new Vector3((tempStartGrid.x + tmpgap + tmpgap * k) / Screen.width, 
                            (tempStartGrid.y + tmpgap * length - tmpgap - tmpgap * i) / Screen.height));
                        GL.Vertex(new Vector3((tempStartGrid.x + tmpgap + tmpgap * k) / Screen.width, 
                            (tempStartGrid.y + tmpgap * length - tmpgap * i) / Screen.height));

                    }
                }
            GL.End();

            GL.Begin(GL.LINES);
            // 테두리 그리기
            GL.Color(Color.black);
            for (int i = 0; i < boldWidth; i++) // 도감 왼쪽 테두리
            {
                GL.Vertex(new Vector3((tempStartGrid.x + i - boldWidth / 2) / Screen.width,
                    tempStartGrid.y / Screen.height));
                GL.Vertex(new Vector3((tempStartGrid.x + i - boldWidth / 2) / Screen.width, 
                    (tempStartGrid.y + gap * 5) / Screen.height));
            }
            for (int i = 0; i < boldWidth; i++) // 도감 오른쪽 테두리
            {
                GL.Vertex(new Vector3((tempStartGrid.x + gap * 5 - boldWidth / 2 + i) / Screen.width, 
                    tempStartGrid.y / Screen.height));
                GL.Vertex(new Vector3((tempStartGrid.x + gap * 5 - boldWidth / 2 + i) / Screen.width, 
                    (tempStartGrid.y + gap * 5) / Screen.height));
            }
            for (int i = 0; i < boldWidth; i++) // 도감 아래쪽 테두리
            {
                GL.Vertex(new Vector3((tempStartGrid.x - 3) / Screen.width, 
                    (tempStartGrid.y + i - boldWidth / 2) / Screen.height));
                GL.Vertex(new Vector3((tempStartGrid.x + gap * 5 + 2) / Screen.width, 
                    (tempStartGrid.y + i - boldWidth / 2) / Screen.height));
            }
            for (int i = 0; i < boldWidth; i++) // 도감 위쪽 테두리
            {
                GL.Vertex(new Vector3((tempStartGrid.x - 3) / Screen.width,
                    (tempStartGrid.y + gap * 5 + i - boldWidth / 2) / Screen.height));
                GL.Vertex(new Vector3((tempStartGrid.x + gap * 5 + 2) / Screen.width, 
                    (tempStartGrid.y + gap * 5 + i - boldWidth / 2) / Screen.height));
            }
            GL.End();
        }
        GL.PopMatrix();
    }
}

