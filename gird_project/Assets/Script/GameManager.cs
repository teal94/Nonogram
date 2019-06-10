using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // 텍스처들
    public Texture search;
    public Texture erase;
    public Texture xicon;
    public Texture clearImage;
    
    public static GameManager inst;
    bool clear = false; // 클리어 체크변수
    private static Material mat; // 마테리얼

    Vector3 mousePos; // 마우스 포지션
    Vector3 startDrawgrid; // 그리드를 그릴 시작 좌표
    Vector3 startDrawMap; // 미니 그리드를 그릴 시작 좌표
    float gap = 40;     // 그리드의 간격 변수
    float mapGap = 20; // 미니그리드의 간격변수
    int boldWidth = 5; // 테두리 두께
    int count = 0;     // 클릭 횟수
    int clearTime = 0; // 클리어 시간
    GUIText text;      // 텍스트
    float startTime = 0; // 시작 시간
    float cur = 0;   // 현재 시간

    public int length = 20; // 로직 길이
    int itemNum = 5;  // 아이템 개수
    public int[,] grid;  // 그리드 배열
    public bool[] searchItem; // 아이템 배열
    public bool[] eraseItem;
    public int[] selectRow; // 선택한 개수 저장
    public int[] selectCol; 
    public int[,] curStage; // 현재 스테이지
    public List<List<int>> rowNumber; // 그리드 정보
    public List<List<int>> colNumber;

    public List<List<int>> selectRowNumber; // 각 행의 떨어져있는 선택 그리드 저장
    public List<List<int>> selectColNumber;// 각 열의 떨어져있는 선택 그리드 저장

    public int stageNum = 1;
    int rowMaxNumber = 0; // 외곽 힌트의 최대 개수
    int colMaxNumber = 0;
    public string logicName = "";

    void Start() // 초기화
    {
        inst = this;
        stageNum = SelectManager.stageNum;
        length = stage.getStage(stageNum);

        curStage = (int[,])stage.stageList[stageNum-1].stage.Clone();
        rowNumber = new List<List<int>>();
        colNumber = new List<List<int>>();
        selectRowNumber = new List<List<int>>();
        selectColNumber = new List<List<int>>();
        setLogicNumber();
        grid = new int[length, length];
        searchItem = new bool[itemNum];
        eraseItem = new bool[itemNum];
        selectRow = new int[length];
        selectCol = new int[length];
        startTime = Time.time; // 각 단계 시작한 시간

        text = GetComponent<GUIText>();
        text.text = "Time : 00:00\nCount : "+count; 

        text.color = Color.black;
        count = 0;
        int max = colMaxNumber > rowMaxNumber ? colMaxNumber : rowMaxNumber;
        gap = Screen.height / (length+ max+2);

        //gap = Screen.height / ((length + max) * 2.3f);

        startDrawgrid = new Vector3(30+ rowMaxNumber * gap, gap);

        mapGap = gap / 2;
        startDrawMap = new Vector3(startDrawgrid.x + gap * (length*1.3f), startDrawgrid.y);
        text.fontSize = (int)(gap * ((float)(length + max) / 15));
        text.pixelOffset = new Vector3(startDrawgrid.x + gap * (length*1.3f), startDrawgrid.y + gap * length/ 2 
            + gap * ((float)(length + max) / 10) * 5);
    }


    void setLogicNumber() // 외곽 로직 넘버 설정
    {
        int count = 0;
        for(int i=0;i<length;i++) // 가로 로직 넘버
        {
            rowNumber.Add(new List<int>());
            selectRowNumber.Add(new List<int>());
            for (int k = 0; k < length; k++)
            {
                if (curStage[i, k] == 1)
                    count++;
                else if(curStage[i, k] == 0 && count !=0)
                {
                    rowNumber[i].Add(count);
                    selectRowNumber[i].Add(0);
                    count = 0;
                }
            }
            if (count != 0)
            {
                rowNumber[i].Add(count);
                selectRowNumber[i].Add(0);
                count = 0;
            }
            if (rowNumber[i].Count > rowMaxNumber)
                rowMaxNumber = rowNumber[i].Count;

        }
        count = 0;
        for (int i = 0; i < length; i++) // 세로 로직 넘버
        {
            colNumber.Add(new List<int>());
            selectColNumber.Add(new List<int>());
            for (int k = 0; k < length; k++)
            {
                if (curStage[k, i] == 1)
                    count++;

                if (curStage[k, i] == 0 && count != 0)
                {
                    colNumber[i].Add(count);
                    selectColNumber[i].Add(0);
                    count = 0;
                }
            }

            if (count != 0)
            {
                colNumber[i].Add(count);
                selectColNumber[i].Add(0);
                count = 0;
            }
            if (colNumber[i].Count > colMaxNumber)
                colMaxNumber = colNumber[i].Count;
        }
    }
    void setSelectNumber() // 유저가 선택한 각 그리드 넘버
    {
        int count = 0;
        int secondCnt = 0;
        for (int i = 0; i < length; i++)
        {
            for (int k = 0; k < selectRowNumber[i].Count; k++)
                selectRowNumber[i][k] = 0;

            for (int k = 0; k < selectColNumber[i].Count; k++)
                selectColNumber[i][k] = 0;
        }

        for (int i = 0; i < length; i++) // 가로 로직 넘버
        {
            secondCnt = 0;
            for (int k = 0; k < length; k++) 
            {
                if (secondCnt == rowNumber[i].Count)
                {
                    count = 0;
                    break;
                }
                if (grid[i, k] == 1)
                    count++;
                else if ((grid[i, k] == 0 || grid[i, k] == 2) && count != 0)
                {
                    selectRowNumber[i][secondCnt++] = count;
                    count = 0;
                }

            }
            if (count != 0)
            {
                selectRowNumber[i][secondCnt++] = count;
                count = 0;
            }
        }
        count = 0;

        for (int i = 0; i < length; i++) // 세로 로직 넘버
        {
            secondCnt = 0;
            for (int k = 0; k < length; k++)
            {
                if (secondCnt == colNumber[i].Count)
                {
                    count = 0;
                    break;
                }
                if (grid[k, i] == 1)
                    count++;

                if ((grid[k, i] == 0 || grid[k, i] == 2) && count != 0)
                {
                    selectColNumber[i][secondCnt++]= count;
                    count = 0;
                }


            }
            if (count != 0)
            {
                selectColNumber[i][secondCnt++] = count;
                count = 0;
            }
        }
    }
    public static GameManager getInst()
    {
        return inst;
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

    void Update() // 마우스 버튼 입력
    {
        ShowTime();
        Vector2 pos = Input.mousePosition;
        if(clear == false)
            if (pos.x >= startDrawgrid.x && pos.x <= startDrawgrid.x+gap* length
                && pos.y >= startDrawgrid.y && pos.y <= startDrawgrid.y + gap * length)//마우스가 그리드 안에 존재할시
                if (Input.GetMouseButtonDown(0)) // 마우스 입력시 그리드에 저장 
                {
                    count++;
                    pos.x -= startDrawgrid.x;
                    pos.y -= startDrawgrid.y;
                    int k = (int)(pos.x / gap);
                    int i = (int)(pos.y / gap);
                    i = (length-1) - i;
                    if (grid[i, k] == 0 || grid[i, k] == 2)
                        grid[i, k] = 1;
                    else
                        grid[i, k] = 0;

                    setSelectNumber();
                }
                else if (Input.GetMouseButtonDown(1)) // 마우스 입력시 그리드에 저장 
                {

                    pos.x -= startDrawgrid.x;
                    pos.y -= startDrawgrid.y;
                    int k = (int)(pos.x / gap);
                    int i = (int)(pos.y / gap);
                    i = (length - 1) - i;
                    if (grid[i, k] == 0)
                    {
                        grid[i, k] = 2;
                    }
                    else if (grid[i, k] == 1)
                    {
                        grid[i, k] = 2;
                        count++;
                    }
                    else if (grid[i, k] == 2)
                        grid[i, k] = 0;

                    setSelectNumber();
                }
    }
    void ShowTime() // 시간 출력
    {

        if (clear == false)
            cur = Time.time - startTime;
        if (clear == true && clearTime == 0) //클리어하면 시간저장
            clearTime = (int)cur;
        int minutes;
        int second;
        string timeStr;

        string str = null;

        for (int i = 0; i < 5; i++)
            if (i < stage.stageList[stageNum - 1].level)
                str += "★";
            else
                str += "☆";

        minutes = (int)cur / 60;
        second = (int)cur % 60;
        text.fontStyle = FontStyle.Bold;
        timeStr = minutes.ToString("D2") + ":";
        timeStr += second.ToString("D2");
        text.text = stage.stageList[stageNum - 1].name+" "+str+ "\nTime : "+timeStr+"\nCount : " + count;
    }
    private void OnGUI()  //GL 을 이용해서 로직 그리기
    {
        for (int i = 0; i < length; i++) // x 그리기
            for (int k = 0; k < length; k++)
            {
                if (grid[i, k] == 2)
                {
                    GUI.DrawTexture(new Rect((startDrawgrid.x + gap * k), Screen.height -(startDrawgrid.y - gap * 
                        i +gap*length), gap, gap), xicon);

                }
            }
        int max = colMaxNumber > rowMaxNumber ? colMaxNumber : rowMaxNumber;
        for (int i=0;i<5;i++) // 서치 아이템 5개 그리기
        {
            if (searchItem[i] == false)
                if (GUI.Button(new Rect(startDrawgrid.x + gap * (length*1.3f) + i * gap * ((float)(length + max) / 10), 
                    Screen.height - (startDrawMap.y + mapGap * length + gap * ((float)(length + max) / 10) *2.2f), 
                    gap * ((float)(length + max) / 10), gap * ((float)(length + max) / 10)), search))
                {
                    bool ch = false;
                    for (int j = 0; j < length; j++) // 정답 남은게 있는지 체크
                        for (int k = 0; k < length; k++)
                        {
                            Debug.Log("[" + j + ", " + k + "]" + curStage[j, k]);
                            if (grid[j, k] == 0 && curStage[j, k] == 1)
                                ch = true;
                        }
                    if (ch == true)
                    {
                        while (true)
                        {
                            int a = Random.Range(0, length);
                            int b = Random.Range(0, length);

                            if (grid[a, b] == 0 || grid[a, b] == 2)
                                if (curStage[a, b] == 1)
                                {
                                    Debug.Log("[" + a + ", " + b + "]");
                                    grid[a, b] = 1;
                                    count++;
                                    setSelectNumber();
                                    break;
                                }
                        }
                        searchItem[i] = true;
                    }
                }

            if (eraseItem[i] == false) // 지우개 아이템 5개 그리기
                if (GUI.Button(new Rect(startDrawgrid.x + gap * (length * 1.3f) + i * gap * ((float)(length + max) / 10), 
                    Screen.height - (startDrawMap.y + mapGap * length + gap * ((float)(length + max) / 10) * 1.1f), 
                    gap * ((float)(length + max) / 10), gap * ((float)(length + max) / 10)), erase))
                {
                    bool ch = false;
                    for (int j = 0; j < length; j++) // 오류 남은게 있는지 체크
                        for (int k = 0; k < length; k++)
                        {
                            if (grid[j, k] == 1 && curStage[j, k] == 0)
                                ch = true;
                        }
                    if (ch == true)
                    {
                        while (true)
                        {
                            int a = Random.Range(0, length);
                            int b = Random.Range(0, length);

                            if (grid[a, b] == 1)
                                if (curStage[a, b] == 0)
                                {
                                    Debug.Log("[" + a + ", " + b + "]");
                                    grid[a, b] = 0;
                                    count++;
                                    break;
                                }
                        }
                        eraseItem[i] = true;
                    }
                }
        }
        GUI.color = Color.black;
        var centeredStyle = GUI.skin.GetStyle("Label"); // 가운데 정렬을 위한 스타일
        centeredStyle.alignment = TextAnchor.MiddleCenter;
        centeredStyle.fontSize = (int)gap/2;
        centeredStyle.fontStyle = FontStyle.Bold;

        bool clearCh = true;
        for (int i =0;i<length;i++)
        {
            for (int k=0;k<colNumber[i].Count;k++)
            {
                GUI.color = Color.black;
                if (colNumber[i][k] == selectColNumber[i][k])// 맞으면 초록색으로
                    GUI.color = Color.green;
                else
                    clearCh = false;
 
                GUI.Label(new Rect(startDrawgrid.x + gap * i,Screen.height -( startDrawgrid.y - gap * k + 
                    gap*length +gap * colNumber[i].Count) , gap, gap), colNumber[i][k]+"", centeredStyle);
            }

            for (int k = 0; k < rowNumber[i].Count; k++)
            {
                GUI.color = Color.black;
                if (rowNumber[i][k] == selectRowNumber[i][k])// 맞으면 초록색으로
                    GUI.color = Color.green;
                else
                    clearCh = false;

                GUI.Label(new Rect(startDrawgrid.x + gap * k -gap * rowNumber[i].Count, Screen.height - 
                    (startDrawgrid.y - gap * i + gap * length), gap, gap), rowNumber[i][k] + "", centeredStyle);
            }
        }

        if (clearCh == true) // 로직을 완성했으면
            clear = true;
        GUI.color = Color.white;
        var Style = GUI.skin.GetStyle("Button");
        Style.fontSize = (int)gap / 2;
        Style.fontStyle = FontStyle.Bold;
        if (clear == false)
            if (GUI.Button(new Rect(Screen.width - gap * 6, gap / 2, gap * 4, gap*2), "뒤로가기", Style)) // 셀렉트 신으로 가는 버튼
                SceneManager.LoadScene("SelectScene");



        if (clear == true) // 로직을 완성했을 경우
        {
            
            int minutes;
            int second;
            string timeStr;

            minutes = (int)clearTime / 60;
            second = (int)clearTime % 60;

            timeStr = minutes.ToString("D2") + ":";
            timeStr += second.ToString("D2");

            var Style3 = GUI.skin.GetStyle("Label");
            Style3.fontSize = (int)Screen.width / 28;     // 클리어 이미지 출력
            Style3.fontStyle = FontStyle.Bold;
            GUI.Label(new Rect(Screen.width / 13 * 1.5f, Screen.height / 6, Screen.width / 13 * 10, 
                Screen.width / 13 * 4), clearImage);
            GUI.color = Color.black;
            GUI.Label(new Rect(Screen.width / 13 * 1.5f, Screen.height / 5f, Screen.width / 13 * 10, 
                Screen.width / 13 * 4), "클리어 타임 : "+timeStr, Style3);
            GUI.Label(new Rect(Screen.width / 13 * 1.5f, Screen.height / 3.4f, Screen.width / 13 * 10, 
                Screen.width / 13 * 4), "클릭 횟수 : " + count, Style3);

            var Style4= GUI.skin.GetStyle("Button");
            Style4.fontSize = (int)Screen.width / 28;
            Style4.fontStyle = FontStyle.Bold;
            // 메인 화면으로 가는 버튼
            if (GUI.Button(new Rect(Screen.width / 13 * 4f, Screen.height / 1.3f, Screen.width / 13 * 5, 
                Screen.width / 13 * 1.2f), "메인 화면", Style4))
            {
                // 클리어 타임, 카운트 저장
                if (stage.stageList[stageNum - 1].bestTime == 0 || stage.stageList[stageNum - 1].bestTime < clearTime)
                    stage.stageList[stageNum - 1].bestTime = clearTime;

                if (stage.stageList[stageNum - 1].bestCount == 0 || stage.stageList[stageNum - 1].bestCount < count)
                    stage.stageList[stageNum - 1].bestCount = count;

                stage.saveStage();
                SceneManager.LoadScene("MenuScene");
            }   
        }
    }
    void OnPostRender() // GL을 이용하여 그리드 그리기
    {
        if (!mat)
        {
            Debug.LogError("마테리얼 없음!");
            return;
        }
        GL.PushMatrix();
 
        mat.SetPass(0);
        GL.LoadOrtho();

        Vector2 pos = Input.mousePosition;
        if (pos.x >= startDrawgrid.x && pos.x < startDrawgrid.x + gap * length
            && pos.y >= startDrawgrid.y && pos.y < startDrawgrid.y + gap * length)
        {
            GL.Begin(GL.QUADS); // 마우스를 그리드에 올렸을때 십자가 그리기
            GL.Color(new Color((float)0.9, (float)0.9, (float)0.9));

            pos.x -= startDrawgrid.x;
            pos.y -= startDrawgrid.y;
            int k = (int)(pos.x / gap);
            int i = (int)(pos.y / gap);

            GL.Vertex(new Vector3((startDrawgrid.x + gap * k) / Screen.width, 
                (startDrawgrid.y + gap * length + gap* colMaxNumber) / Screen.height)); // 세로 그리기
            GL.Vertex(new Vector3((startDrawgrid.x + gap * (k+1)) / Screen.width, 
                (startDrawgrid.y + gap * length + gap* colMaxNumber) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * (k+1)) / Screen.width, 
                (startDrawgrid.y) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * k) / Screen.width, 
                (startDrawgrid.y) / Screen.height));

            GL.Vertex(new Vector3((startDrawgrid.x + gap * length) / Screen.width, 
                (startDrawgrid.y + gap * i) / Screen.height)); // 가로 그리기
            GL.Vertex(new Vector3((startDrawgrid.x  + gap * length) / Screen.width, 
                (startDrawgrid.y + gap * (i + 1)) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x - gap* rowMaxNumber) / Screen.width, 
                (startDrawgrid.y + gap * (i + 1)) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x - gap* rowMaxNumber) / Screen.width, 
                (startDrawgrid.y + gap * i) / Screen.height));

            GL.Color(new Color((float)0.65, (float)0.65, (float)0.65)); // 정중앙 진한 사각형그리기
            GL.Vertex(new Vector3((startDrawgrid.x + gap * k) / Screen.width, 
                (startDrawgrid.y + gap * (i + 1)) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * (k + 1)) / Screen.width, 
                (startDrawgrid.y + gap * (i + 1)) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * (k + 1)) / Screen.width, 
                (startDrawgrid.y + gap * (i)) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * k) / Screen.width, 
                (startDrawgrid.y + gap * (i)) / Screen.height));

            GL.End();
        }

        // 클릭한 그리드 칠하기
        GL.Begin(GL.QUADS);
        GL.Color(Color.gray);
        for (int i = 0; i < length; i++)
            for (int k = 0; k < length; k++)
            {
                if (grid[i, k] == 1)
                {
                    GL.Vertex(new Vector3((startDrawgrid.x + gap * k) / Screen.width, 
                        (startDrawgrid.y + gap* length - gap * i) / Screen.height));
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
        for (int i = 0; i < length+1; i++) // 세로 격자
        {
            GL.Vertex(new Vector3((startDrawgrid.x + gap * i) / Screen.width, (startDrawgrid.y) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * i) / Screen.width, (startDrawgrid.y + gap * length + gap*colMaxNumber) / Screen.height));
        }
        for (int i = 0; i < length+1; i++) // 가로 격자
        {
            GL.Vertex(new Vector3((startDrawgrid.x - gap * rowMaxNumber) / Screen.width, (startDrawgrid.y+gap * i) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length) / Screen.width, (startDrawgrid.y + gap * i) / Screen.height));
        }

        for (int i = 0; i < boldWidth; i++) // 왼쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawgrid.x + i - boldWidth/2) / Screen.width, startDrawgrid.y / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + i - boldWidth / 2) / Screen.width, (startDrawgrid.y + gap * length) / Screen.height));
        }
        for (int i = 0; i < boldWidth; i++) // 오른쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length - boldWidth / 2 + i) / Screen.width, startDrawgrid.y / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length - boldWidth / 2 + i) / Screen.width, (startDrawgrid.y + gap * length) / Screen.height));
        }
        for (int i = 0; i < boldWidth; i++) // 아래쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawgrid.x-3) / Screen.width, (startDrawgrid.y + i - boldWidth / 2) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length + 2) / Screen.width, (startDrawgrid.y + i - boldWidth / 2) / Screen.height));
        }
        for (int i = 0; i < boldWidth; i++) // 위쪽 테두리
        {
            GL.Vertex(new Vector3((startDrawgrid.x-3) / Screen.width, (startDrawgrid.y + gap * length + i - boldWidth / 2) / Screen.height));
            GL.Vertex(new Vector3((startDrawgrid.x + gap * length + 2) / Screen.width, (startDrawgrid.y + gap * length + i - boldWidth / 2) / Screen.height));
        }

        for(int k=1;k<length/5;k++)
            for (int i = 0; i < boldWidth; i++) // 내부 가로 테두리
            {
                GL.Vertex(new Vector3((startDrawgrid.x - 3) / Screen.width, (startDrawgrid.y + gap * k*5+ i - boldWidth / 2) / Screen.height));
                GL.Vertex(new Vector3((startDrawgrid.x + gap * length + 2) / Screen.width, (startDrawgrid.y + gap * k * 5 + i - boldWidth / 2) / Screen.height));
            }
        for (int k = 1; k < length / 5; k++)
            for (int i = 0; i < boldWidth; i++) // 내부 세로 테두리
            {
                GL.Vertex(new Vector3((startDrawgrid.x + gap * k * 5 - boldWidth / 2 + i) / Screen.width, startDrawgrid.y / Screen.height));
                GL.Vertex(new Vector3((startDrawgrid.x + gap * k * 5 - boldWidth / 2 + i) / Screen.width, (startDrawgrid.y + gap * length) / Screen.height));
            }
        GL.End();



        GL.Begin(GL.QUADS); // 미니맵 선택한 그리드 그리기
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
            GL.Vertex(new Vector3((startDrawMap.x + mapGap * length + 2) / Screen.width, (startDrawMap.y + 
                mapGap * length + i - boldWidth / 2) / Screen.height));
        }
        GL.End();

        GL.PopMatrix();
    }
}

