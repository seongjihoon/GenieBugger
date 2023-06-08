using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{

    static UIManager instance;

    public GameObject SettingPanel;

    public Button stopButton;

    public GameObject BlurPanel;

    public Text counts_Text;


    [Space(5f)]
    [Header("Setting & GameOver Panel")]
    public GameObject sharePanel;
    //public GameObject panelTitle;    // 현재 Panel이 어떤 역할을 하는지 보여주는 ww

    [Space(5f)]
    [Header("GameOver Options")]
    public GameObject GameOverUI;
    public Text _highScore;
    public Text _nowScore;

    [Header("FeverUi")]
    public GameObject _FeverPanel;
    public ParticleSystem particle;

    [Header("OnButtons")]
    public List<Button> Buttons;
    public List<Toggle> Toggles;

    public static UIManager getInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        if (BlurPanel != null)
            BlurPanel.SetActive(false);
        if (sharePanel != null)
            sharePanel.SetActive(false);
        //if (counts_Text != null)
        //    counts_Text.gameObject.SetActive(false);
        if (SettingPanel != null)
            SettingPanel.gameObject.SetActive(false);
        if (stopButton != null)
        {
            stopButton.onClick.AddListener(() => PlayOnUISound());
        }

        if(Buttons.Count > 0)
        {
            for(int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].onClick.AddListener(() => PlayOnUISound());
            }
        }
        if(Toggles.Count > 0)
        {
            for(int i = 0; i < Toggles.Count; i++)
            {
                Toggles[i].onValueChanged.AddListener(delegate
                {
                    PlayOnUISound();    
                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayOnUISound()
    {
        try
        {
            SoundManager.getInstance().PlaySound(0);
        }
        catch(System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void PlayOffUISound()
    {
        try {

            SoundManager.getInstance().PlaySound(1);

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void PlayUIMoveSound()
    {
        try
        {
            SoundManager.getInstance().PlaySound(2);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void StartGame()
    {
        StartCoroutine(HideObject());
    }

    public IEnumerator HideObject()
    {
        BlurPanel.SetActive(true);
        BlurPanel.GetComponent<Image>().material.SetInt("_Radius", 0);
        for (int i = 0; i < 30; i++)
        {
            int blurInt = BlurPanel.GetComponent<Image>().material.GetInt("_Radius");
            blurInt++;
            BlurPanel.GetComponent<Image>().material.SetInt("_Radius", blurInt);
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene("GenieBugger");
    }
    public void ShowObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void SetPanel(string _pType)
    {
        // 일시 정지 보여주기
        if (_pType == "Setting")
        {
            sharePanel.SetActive(true);
            Time.timeScale = 0;
            if (GameOverUI != null)
                GameOverUI.SetActive(false);
        }
        else if(_pType == "GameOver")
        {
            Time.timeScale = 0;
            SoundManager.getInstance().ChangeBGM(2);
            if (GameOverUI != null)
                GameOverUI.SetActive(true);
        }
    }

    public void ShowSetting()
    {
        SettingPanel.SetActive(true);
    }
    
    public void Hide_Setting_Panel()
    {
        sharePanel.SetActive(false);
        StartCoroutine(ThreeCount());
    }

    public IEnumerator ThreeCount()
    {
        stopButton.enabled = false;
        int count = 3;

        counts_Text.gameObject.SetActive(true);
        for(int i = 0; i <3; i++)
        {
            counts_Text.text = count.ToString();
            yield return new WaitForSecondsRealtime(1.0f);
            count--;
        }
        counts_Text.text = "Start!";
        yield return new WaitForSecondsRealtime(0.5f);

        counts_Text.gameObject.SetActive(false);
        stopButton.enabled = true;
        Time.timeScale = GameManager.getInstance().timeScales;
    }

    public void ShowFeverPanel()
    {
        SoundManager.getInstance().ChangeBGM(3);
        SoundManager.getInstance().PlaySound(5);
        StartCoroutine(showPanel());
    }

    IEnumerator showPanel()
    {
        _FeverPanel.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        particle.Play();

        yield return new WaitForSeconds(1.0f);
        _FeverPanel.SetActive(false);

        yield return new WaitForSeconds(GameManager.getInstance().feverTime -2.0f);
        SoundManager.getInstance().ChangeBGM(1);
    }

    public void GoTitle()
    {
        Time.timeScale = GameManager.getInstance().timeScales;
        SoundManager.getInstance().ChangeBGM(0);
        SceneManager.LoadScene("TitleScene");
    }

    public void Restart() {
        Time.timeScale = GameManager.getInstance().timeScales;
        SceneManager.LoadScene("GenieBugger");
    }

}
