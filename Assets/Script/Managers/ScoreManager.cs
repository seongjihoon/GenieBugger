using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    static ScoreManager instance;
    [SerializeField]
    public int totalscore;      // 최종 스코어
    [SerializeField]
    private int highScore;      // 최고 점수
    public int score;           // 라운드 스코어
    public Text scoreText;      // 최종 스코어를 표시하는 textUI;

    [SerializeField]

    [Header("scoreBoard")]
    public List<Sprite> numbers;
    [Header("HighScore")]
    public Image h_scoreImgae;
    public Image h_num_1;   // 100의 자리
    public Image h_num_2;   // 10의 자리
    public Image h_num_3;   // 1의 자리
    [Header("Score")]
    public Image _scoreImage;
    public List<Image> _scoreNums;
    public Image _num_1;   // 100의 자리
    public Image _num_2;   // 10의 자리
    public Image _num_3;   // 1의 자리
    //public Image num_1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "현재 점수: " + totalscore.ToString();

        _num_1.gameObject.SetActive(false);
        _num_2.gameObject.SetActive(false);

        h_num_1.gameObject.SetActive(false);
        h_num_2.gameObject.SetActive(false);

        if (totalscore == 0)     // 스코어가 0점일 경우
            _num_3.sprite = numbers[0];
        if (highScore == 0)
            h_num_3.sprite = numbers[0];
    }
    public static ScoreManager GetInstance(){ return instance; }

    // Update is called once per frame
    void Update()
    {


    }

    public void AddScore(int get, Food.FoodType type)
    {
        if (!GameManager.getInstance().GetFever() && type != Food.FoodType.TYPE_FEVER)      // Fever타임 또는 Fever 손님의 경우 fever포인트를 쌓지 않음.
        {
            score += get;
            GameManager.getInstance().SetFeverCount(get);
        }
        totalscore += get;
        
        if (totalscore == 10)
            _num_2.gameObject.SetActive(true);
        if(totalscore == 100)
            _num_1.gameObject.SetActive(true);
        Numbers();
        if (score >= FileManager.getInstance().RoundCheck())
        {
            Debug.Log("NextRound");
            GameManager.getInstance().NextRound();
            NPCSpawner.GetInstance().NextRound();
            score = 0;
        }
        Debug.Log("현재 점수: " + totalscore);
        //scoreText.text = "현재 점수: " + totalscore.ToString();
    }

    void Numbers()   // 1의 자리
    {
        int num = totalscore;
        Stack<int> nums = new Stack<int>();

        while (num != 0)
        {
            int quotient = num % 10;
            nums.Push(quotient);
            num /= 10;
        }

        int counts = nums.Count;
        while (counts > 0)
            _scoreNums[--counts].sprite = numbers[nums.Pop()];
    }

    void ScoreTens()
    {

    }

    void ScoreHundreds()
    {

    }


}
