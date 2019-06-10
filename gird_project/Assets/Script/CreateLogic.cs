using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateLogic : MonoBehaviour {

    public int[,] grid; // 그리드 배열
    public int length; // 그리드 길이
    public int gap; // 그리드 간격
    public int mapGap; // 미니 그리드 간격
    Vector3 startDrawgrid; // 그리드 시작 위치
    Vector3 startDrawMap; // 미니 그리드 시작 위치
    int boldWidth = 5; // 테두리 두께
    GUIText text;
    int count = 0; // 횟수
    int level = 1; // 난이도
    private static Material mat; //마테리얼
    string name = ""; // 로직 이름
    public InputField InputField;
    public Text fieldText;
    // Use this for initialization
    void Start () // 로직을 그리기 위한 초기화
    {
        length = EditMenu.length;

        grid = new int[length, length];
        gap = Screen.height / (length + 2);
        Debug.Log(gap + "");
        startDrawgrid = new Vector3(30 , gap);
        text = GetComponent<GUIText>();
        mapGap = gap / 2;
        startDrawMap = new Vector3(startDrawgrid.x + gap * (length * 1.3f), startDrawgrid.y);
        text.fontSize = (int)(gap * ((float)(length) / 15));
        text.pixelOffset = new Vector3(startDrawgrid.x + gap * (length * 1.3f), 
            startDrawgrid.y + gap * length / 2 + gap * ((float)(length) / 10) * 5);

        InputField.transform.position = new Vector2(startDrawgrid.x + gap * (length * 1.55f), 
            (startDrawMap.y + mapGap * length + gap * ((float)(length) / 10) * 1.1f) + gap * 1.5f * (length) / 10);
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

    void Update () { // 마우스 버튼 입력
        Vector2 pos = Input.mousePosition;
        name = fieldText.text;
        if (pos.x >= startDrawgrid.x && pos.x <= startDrawgrid.x + gap * length
            && pos.y >= startDrawgrid.y && pos.y <= startDrawgrid.y + gap * length)//마우스가 그리드 안에 존재할시
            if (Input.GetMouseButtonDown(0)) // 마우스 입력시 그리드에 저장 
            { 
                pos.x -= startDrawgrid.x;
                pos.y -= startDrawgrid.y;
                int k = (int)(pos.x / gap);
                int i = (int)(pos.y / gap);
                i = (length - 1) - i;
                if (grid[i, k] == 0)
                {
                    grid[i, k] = 1;
                    count++;
                }
                else
                {
                    grid[i, k] = 0;
                    count--;
                }
            }

        if (Input.GetMouseButtonDown(0))
            for (int i=0;i<5;i++)
            {
                if (pos.x >= startDrawgrid.x + gap * (length * 1.3f) + i * gap * ((float)(length) / 10) &&
                    pos.x <= startDrawgrid.x + gap * (length * 1.3f) + i * gap * ((float)(length) / 10) + gap * ((float)(length) / 10) &&
                    pos.y >= (startDrawMap.y + mapGap * length + gap * ((float)(length) / 10) * 1.1f) - gap * ((float)(length) / 10) &&
                    pos.y <= (startDrawMap.y + mapGap * length + gap * ((float)(length) / 10) * 1.1f) )
                    level = i + 1;
            }
    }
    private void OnGUI() // 안내 텍스트 출력
    {
        var Style = GUI.skin.GetStyle("Button");
        Style.fontSize = (int)(gap / 3 * ((float)(length) / 10));
        Style.fontStyle = FontStyle.Bold;
        if (GUI.Button(new Rect(Screen.width - gap * 3 * ((float)(length) / 10), gap / 2 * ((float)(length) / 10),
            gap * 2 * ((float)(length) / 10), gap * ((float)(length) / 10)), "뒤로가기", Style))
            SceneManager.LoadScene("EditScene");

        if(count != 0)
            if (GUI.Button(new Rect(Screen.width - gap * 3 * ((float)(length) / 10) - gap * 3 * ((float)(length) / 10) ,
                gap / 2 * ((float)(length) / 10),
                 gap * 2 * ((float)(length) / 10), gap * ((float)(length) / 10)), "로직 생성", Style))
            {
                stage.stageList.Add(new stageData(level, (int[,])grid.Clone(), name,0,0));
                stage.saveStage();
                SceneManager.LoadScene("EditScene");
            }
        var Style2 = GUI.skin.GetStyle("Label");
        Style2.fontSize = (int)(gap/2 * ((float)(length) / 10));
        Style2.alignment = TextAnchor.MiddleCenter;
        Style2.fontStyle = FontStyle.Bold;
        for (int i=0;i<5;i++)
        {
            string str = null;
            if (level > i)
                str = "★";
            else
                str = "☆";
            GUI.color = Color.black;
            GUI.Label(new Rect(startDrawgrid.x + gap * (length * 1.3f) + i * gap * ((float)(length) / 10),
                    Screen.height - (startDrawMap.y + mapGap * length + gap * ((float)(length) / 10) * 1.0f),
                    gap * ((float)(length) / 10), gap * ((float)(length) / 10)), str, Style2);
        }
        GUI.Label(new Rect(startDrawgrid.x + gap * (length * 1.3f),
                    Screen.height - (startDrawMap.y + mapGap * length + gap * ((float)(length) / 10) * 0.6f)
                    - gap* (length) / 10, gap * ((float)(length) / 10)*5, gap * ((float)(length) / 10)), "난이도 설정", Style2);
        GUI.Label(new Rect(startDrawgrid.x + gap * (length * 1.3f),
            Screen.height - (startDrawMap.y + mapGap * length + gap * ((float)(length) / 10) * 0.2f) 
            - gap*2 *(length) / 10, gap * ((float)(length) / 10) * 5, gap * ((float)(length) / 10)), "개수 : "+count, Style2);

        GUI.Label(new Rect(startDrawgrid.x + gap * (length * 1.3f),
            Screen.height - (startDrawMap.y + mapGap * length - gap * ((float)(length) / 10) * 0.2f) - 
    gap * 4 * (length) / 10, gap * ((float)(length) / 10) * 5, gap * ((float)(length) / 10)), "제목 : "+ name, Style2);
    }
    private void OnPostRender() // GL을 이용하여 로직 그리기
    {
        if (!mat)
        {
            Debug.LogError("마테리얼 없음!");
            return;
        }
        GL.PushMatrix();

        mat.SetPass(0);
        GL.LoadOrtho();

        GL.Begin(GL.QUADS);
        GL.Color(Color.gray);
        for (int i = 0; i < length; i++)
            for (int k = 0; k < length; k++)
            {
                if (grid[i, k] == 1)
                {
                    GL.Vertex(new Vector3((startDrawgrid.x + gap * k) / Screen.width, 
                        (startDrawgrid.y + gap * length - gap * i) / Screen.height));
                    GL.Vertex(new Vector3((startDrawgrid.x + gap * k) / Screen.width, 
                        (startDrawgrid.y - gap + gap * length - gap * i - 1) / Screen.height));
                    GL.Vertex(new Vector3((startDrawgrid.x + gap + gap * k) / Screen.width,
                        (startDrawgrid.y + gap * length - gap - gap * i - 1) / Screen.height));
                    GL.Vertex(new Vector3((startDrawgrid.x + gap + gap * k) / Screen.width, 
                        (startDrawgrid.y + gap * length - gap * i) / Screen.height));
                }
            }
        GL.End();

        GL.Begin(GL.LINES);

        GL.Color(Color.black);
        for (int i = 0; i < length + 1; i++) // 세로 격자
        {
            GL.Vertex(new Vector3((startDrawgrid.x + gap * i) / Screen.width, (startDrawgrid.y) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * i) / Screen.width, (startDrawgrid.y + gap * length) / Screen.height));
        }
        for (int i = 0; i < length + 1; i++) // 가로 격자
        {
            GL.Vertex(new Vector3((startDrawgrid.x) / Screen.width, (startDrawgrid.y + gap * i) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length) / Screen.width, (startDrawgrid.y + gap * i) / Screen.height));
        }

        for (int i = 0; i < boldWidth; i++) // 왼쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawgrid.x + i - boldWidth / 2) / Screen.width, startDrawgrid.y / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + i - boldWidth / 2) / Screen.width, (startDrawgrid.y + gap * length) / Screen.height));
        }
        for (int i = 0; i < boldWidth; i++) // 오른쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length - boldWidth / 2 + i) / Screen.width, startDrawgrid.y / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length - boldWidth / 2 + i) / Screen.width, (startDrawgrid.y + gap * length) / Screen.height));
        }
        for (int i = 0; i < boldWidth; i++) // 아래쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawgrid.x - 3) / Screen.width, (startDrawgrid.y + i - boldWidth / 2) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length + 2) / Screen.width, (startDrawgrid.y + i - boldWidth / 2) / Screen.height));
        }
        for (int i = 0; i < boldWidth; i++) // 위쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawgrid.x - 3) / Screen.width, (startDrawgrid.y + gap * length + i - boldWidth / 2) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length + 2) / Screen.width, (startDrawgrid.y + gap * length + i - boldWidth / 2) / Screen.height));
        }

        for (int k = 1; k < length / 5; k++)
            for (int i = 0; i < boldWidth; i++) // 센터 가로 테두리
            {
                GL.Vertex(new Vector3((startDrawgrid.x - 3) / Screen.width, (startDrawgrid.y + gap * k * 5 + i - boldWidth / 2) / Screen.height));
                GL.Vertex(new Vector3((startDrawgrid.x + gap * length + 2) / Screen.width, (startDrawgrid.y + gap * k * 5 + i - boldWidth / 2) / Screen.height));
            }
        for (int k = 1; k < length / 5; k++)
            for (int i = 0; i < boldWidth; i++) // 센터 세로 테두리
            {
                GL.Vertex(new Vector3((startDrawgrid.x + gap * k * 5 - boldWidth / 2 + i) / Screen.width, startDrawgrid.y / Screen.height));
                GL.Vertex(new Vector3((startDrawgrid.x + gap * k * 5 - boldWidth / 2 + i) / Screen.width, (startDrawgrid.y + gap * length) / Screen.height));
            }
        GL.End();
        GL.Begin(GL.QUADS); // 미니맵 그리기
        GL.Color(Color.gray);
        for (int i = 0; i < length; i++)
            for (int k = 0; k < length; k++)
            {
                if (grid[i, k] == 1)
                {
                    GL.Vertex(new Vector3((startDrawMap.x + mapGap * k) / Screen.width,
                        (startDrawMap.y + mapGap * length - mapGap * i) / Screen.height));
                    GL.Vertex(new Vector3((startDrawMap.x + mapGap * k) / Screen.width,
                        (startDrawMap.y - mapGap + mapGap * length - mapGap * i) / Screen.height));
                    GL.Vertex(new Vector3((startDrawMap.x + mapGap + mapGap * k) / Screen.width,
                        (startDrawMap.y + mapGap * length - mapGap - mapGap * i) / Screen.height));
                    GL.Vertex(new Vector3((startDrawMap.x + mapGap + mapGap * k) / Screen.width,
                        (startDrawMap.y + mapGap * length - mapGap * i) / Screen.height));

                }
            }
        GL.End();

        GL.Begin(GL.LINES);

        GL.Color(Color.black);
        for (int i = 0; i < boldWidth; i++) // 미니맵 왼쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawMap.x + i - boldWidth / 2) / Screen.width, 
                startDrawMap.y / Screen.height));
            GL.Vertex(new Vector3((startDrawMap.x + i - boldWidth / 2) / Screen.width,
                (startDrawMap.y + mapGap * length) / Screen.height));
        }
        for (int i = 0; i < boldWidth; i++) // 미니맵 오른쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawMap.x + mapGap * length - boldWidth / 2 + i) / Screen.width,
                startDrawMap.y / Screen.height));
            GL.Vertex(new Vector3((startDrawMap.x + mapGap * length - boldWidth / 2 + i) / Screen.width,
                (startDrawMap.y + mapGap * length) / Screen.height));
        }
        for (int i = 0; i < boldWidth; i++) // 미니맵 아래쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawMap.x - 3) / Screen.width,
                (startDrawMap.y + i - boldWidth / 2) / Screen.height));
            GL.Vertex(new Vector3((startDrawMap.x + mapGap * length + 2) / Screen.width,
                (startDrawMap.y + i - boldWidth / 2) / Screen.height));
        }
        for (int i = 0; i < boldWidth; i++) // 미니맵 위쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawMap.x - 3) / Screen.width,
                (startDrawMap.y + mapGap * length + i - boldWidth / 2) / Screen.height));
            GL.Vertex(new Vector3((startDrawMap.x + mapGap * length + 2) / Screen.width,
                (startDrawMap.y + mapGap * length + i - boldWidth / 2) / Screen.height));
        }
        GL.End();
        GL.PopMatrix();
    }
}

