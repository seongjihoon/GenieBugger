using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    static ScoreManager instance;
    [SerializeField]
    public int totalscore;      // ���� ���ھ�
    [SerializeField]
    private int highScore;      // �ְ� ����
    public int score;           // ���� ���ھ�
    public Text scoreText;      // ���� ���ھ ǥ���ϴ� textUI;

    [SerializeField]

    [Header("scoreBoard")]
    public List<Sprite> numbers;
    [Header("HighScore")]
    public Image h_scoreImgae;
    public Image h_num_1;   // 100�� �ڸ�
    public Image h_num_2;   // 10�� �ڸ�
    public Image h_num_3;   // 1�� �ڸ�
    [Header("Score")]
    public Image _scoreImage;
    public List<Image> _scoreNums;
    public Image _num_1;   // 100�� �ڸ�
    public Image _num_2;   // 10�� �ڸ�
    public Image _num_3;   // 1�� �ڸ�
    //public Image num_1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "���� ����: " + totalscore.ToString();

        _num_1.gameObject.SetActive(false);
        _num_2.gameObject.SetActive(false);

        h_num_1.gameObject.SetActive(false);
        h_num_2.gameObject.SetActive(false);

        if (totalscore == 0)     // ���ھ 0���� ���
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
        if (!GameManager.getInstance().GetFever() && type != Food.FoodType.TYPE_FEVER)      // FeverŸ�� �Ǵ� Fever �մ��� ��� fever����Ʈ�� ���� ����.
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
        Debug.Log("���� ����: " + totalscore);
        //scoreText.text = "���� ����: " + totalscore.ToString();
    }

    void Numbers()   // 1�� �ڸ�
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
