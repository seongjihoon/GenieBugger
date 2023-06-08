using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public List<GameObject> m_listFood;

    [Header("Fever 관련")]
    public int feverCount;      // 음식을 하나 제대로 전달할 때마다 fever count + 1;  
    public int feverMax;        // Fever Gage. count를 맥스 갯수만큼 채우면 최종적으로 피버 이벤트 발생

    public float curFeverTime = 0;  // 0부터 시작하는 피버타임.
    public float feverTime = 10;   // 고정

    [SerializeField]
    private bool is_Fever = false;       // 피버 상태인지 확인

    [Space(20)]

    public float timeScales = 0f;

    public int round = 0;

    [SerializeField]
    private bool is_game = false;    // 현재 게임이 진행중인가?

    private int LifeCount = 0;

    public int lifeMax = 3;

    public List<GameObject> lifes;

    public Text countText;

    [Header("LighttingSetting")]
    public Light mainLight;
    public Light feverLight;

    [ColorUsageAttribute(true, true)]
    public Color feverGroundColor;

    [ColorUsageAttribute(true, true)]
    public Color feverEquatorColor;

    [ColorUsageAttribute(true, true)]
    public Color feverSkyBoxColor;

    [ColorUsageAttribute(true, true)]
    public Color mainGroundColor;

    [ColorUsageAttribute(true, true)]
    public Color mainEquatorColor;

    [ColorUsageAttribute(true, true)]
    public Color mainSkyBoxColor;

    public GameObject FeverSportLight;




    public bool getGameState()
    {
        return is_game;
    }

    public static GameManager getInstance()
    {
        if(instance == null)   
            instance = new GameManager();
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        is_game = false;
        SoundManager.getInstance().StopBGM();
        round++;

        Time.timeScale = timeScales;
        m_listFood.Add(GameObject.Find("Hamberger"));
        m_listFood.Add(GameObject.Find("Coke"));
        m_listFood.Add(GameObject.Find("FrenchFries"));
        m_listFood.Add(GameObject.Find("Hotdog"));
        m_listFood.Add(GameObject.Find("IceCream"));


        

        LifeCount = lifeMax;
    }

    public void SetGameState(bool m_b)
    {
        is_game = m_b;
    }

    // Start is called before the first frame update
    void Start()
    {
        countText.gameObject.SetActive(true);
        LightingSetting();
        StartCoroutine(StartCount());
    }

    // Update is called once per frame
    void Update()
    {
        if (is_Fever)    // 피버 모드가 시작되었다.
        {
            curFeverTime += Time.deltaTime;
            if (curFeverTime > feverTime)
            {
                curFeverTime = 0;
                is_Fever = false;   // 피버 타임 헤제
                Time.timeScale = 1.0f;
                LightingSetting();
                Debug.Log("unFever Mode");
                FeverSportLight.SetActive(false);
            }

        }
    }

    public void StartGame()
    {
        is_game = true;
    }

    public void EndGame()
    {
        is_game = false;
    }

    public void OpenFood(int openCount)
    {
        //switch(openCount)
        //{
        //    case 3:
        //        for(int i = 0;  i < 2; i++)
        //        {
        //            m_listFood[i].GetComponent<Collider>().enabled = true;
        //        }
        //        break;
        //    case 7:
        //        for (int i = 0; i < 3; i++)
        //        {
        //            m_listFood[i].GetComponent<Collider>().enabled = true;
        //        }
        //        break;
        //    case 15:
        //        for (int i = 0; i < 4; i++)
        //        {
        //            m_listFood[i].GetComponent<Collider>().enabled = true;
        //        }
        //        break;
        //    case 31:
        //        for (int i = 0; i < 5; i++)
        //        {
        //            m_listFood[i].GetComponent<Collider>().enabled = true;
        //        }
        //        break;
        //}
    }

    public void NextRound()
    {
        round++;
        OpenFood(FileManager.getInstance().getRoundFood());
    }

    public void SetFeverCount(int score)
    {
        feverCount += score;
        if(feverCount >= feverMax)
        {
            Debug.Log("Fever Mode!!");
            is_Fever = true;                                    // 피버 모드
            Time.timeScale = 1.2f;
            //TableManager.GetInstance().ChangeForBugger();       // 모든 음식 햄버거로 바꿔주기
            TableManager.GetInstance().ChangeForGuest();
            TableManager.GetInstance().ChangeFeverLight();
            UIManager.getInstance().ShowFeverPanel();
            //StartCoroutine(SportLight());

            FeverSportLight.SetActive(true);
            LightingSetting();
            ResetFeverCount();
        }
    }

    public bool GetFever()
    {
        return is_Fever;
    }
    
    public void ResetFeverCount()
    {
        feverCount = 0;
    }


    public bool LossingNPC()
    {
        try
        {
            if (lifes.Count > 0)
                Destroy(lifes[LifeCount - 1]);
            else if (lifes.Count <= 0)
                return true;
            LifeCount -= 1;
            if (LifeCount <= 0)
            {
                SoundManager.getInstance().PlaySound(4);
                return true;
            }
            Debug.Log("My Life is = " + LifeCount);

        }
        catch
        {

        }
        if (lifes.Count == 0)
            return true;
        return false;
    }

    public IEnumerator SportLight()
    {
        FeverSportLight.SetActive(true);

        yield return new WaitForSeconds(6.0f);
        FeverSportLight.SetActive(false);
    }

    IEnumerator StartCount()
    {
        int count = 3;
        for(int i = 0; i < 3; i++)
        {
            countText.text = count.ToString();
            SoundManager.getInstance().PlaySound(6);
            yield return new WaitForSeconds(1.0f);
            count--;
        }
        countText.text = "Start!";
        SoundManager.getInstance().PlaySound(3);
        try
        {
            SoundManager.getInstance().ChangeBGM(1);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
        yield return new WaitForSeconds(0.5f);   // 셔터 올라가는 시간
        // 셔터 올라가는 애니메이션
        countText.gameObject.SetActive(false);
        GameManager.getInstance().SetGameState(true);

    }

    public void LightingSetting()
    {

        if (GameManager.getInstance().GetFever())
        {

            RenderSettings.ambientSkyColor = feverSkyBoxColor;
            RenderSettings.ambientEquatorColor = feverEquatorColor;
            RenderSettings.ambientGroundColor = feverGroundColor;
            RenderSettings.sun = feverLight;
            mainLight.gameObject.SetActive(false);
        }
        else
        {
            RenderSettings.ambientSkyColor = mainSkyBoxColor;
            RenderSettings.ambientEquatorColor = mainEquatorColor;

            RenderSettings.ambientGroundColor = mainGroundColor;
            RenderSettings.sun = mainLight;
            feverLight.gameObject.SetActive(false);
        }

    }

}
