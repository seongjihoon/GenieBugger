using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public List<GameObject> m_listFood;

    [Header("Fever ����")]
    public int feverCount;      // ������ �ϳ� ����� ������ ������ fever count + 1;  
    public int feverMax;        // Fever Gage. count�� �ƽ� ������ŭ ä��� ���������� �ǹ� �̺�Ʈ �߻�

    public float curFeverTime = 0;  // 0���� �����ϴ� �ǹ�Ÿ��.
    public float feverTime = 10;   // ����

    [SerializeField]
    private bool is_Fever = false;       // �ǹ� �������� Ȯ��

    [Space(20)]

    public float timeScales = 0f;

    public int round = 0;

    [SerializeField]
    private bool is_game = false;    // ���� ������ �������ΰ�?

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
        if (is_Fever)    // �ǹ� ��尡 ���۵Ǿ���.
        {
            curFeverTime += Time.deltaTime;
            if (curFeverTime > feverTime)
            {
                curFeverTime = 0;
                is_Fever = false;   // �ǹ� Ÿ�� ����
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
            is_Fever = true;                                    // �ǹ� ���
            Time.timeScale = 1.2f;
            //TableManager.GetInstance().ChangeForBugger();       // ��� ���� �ܹ��ŷ� �ٲ��ֱ�
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
        yield return new WaitForSeconds(0.5f);   // ���� �ö󰡴� �ð�
        // ���� �ö󰡴� �ִϸ��̼�
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
