using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum NPC_STATE
{
    NPC_NONE,   // 테이블로
    NPC_WAITING,    // 치워지지 않은 테이블로 이동했을 때
    NPC_ORDER,  // 주문
    NPC_EAT,    // 먹는 애니메이션 출력
    NPC_CALC,   // 계산
};

public enum NPC_FACE
{
    FACE_01,
    FACE_02,
    FACE_03,
    FACE_04,
    FACE_05,
}

public class NPC : MonoBehaviour
{
    [Header("주문한 음식 종류")]
    public Food.FoodType foodType;

    //public int myOrder;         // enum으로 바꿀 예정 내가 주문할 음식을 결정하고 기다리는 구문

    [Header("타이머(마우스 올려서 기능 확인)")]
    [Tooltip("기다리는 시간 한계점.")]
    public float limited = 5;         // 한계점
    [Tooltip("먹는 시간")]
    public float eat_Time = 3;
    public Animator _npcAnim;

    [Header("주문 시각화 관련")]
    public List<Sprite> orderSprite;
    public List<Sprite> feverSprite;

    public NPC_STATE state;
    [Space(10f)]
    [Header("야광봉")]
    public List<GameObject> _feverLights;
    public List<Material> _feverMaterial;
    [SerializeField]
    private int curLight = 0;
    [Space(10f)]
    [Header("손놈 얼굴")]
    public GameObject npc_Face;
    public List<Material> _faceMaterial; 


    // private 
    [Header("아래론 private. 건들면 안됨...")]
    [SerializeField]
    private float waiting = 0;         // 기다리는 시간 n초 후 화내면서 나감.

    [Header("현재 앉아있는 테이블 위치")]
    [SerializeField]
    private Table TargetTable;
    [SerializeField]
    private Chair myChair;
    [SerializeField]
    private Food myFood;

    
    

    private void Awake()
    {
        state = NPC_STATE.NPC_NONE;
        _npcAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        if(_feverLights.Count > 0 && !GameManager.getInstance().GetFever())
        {
            TurnFeverLight(false);
        }
    }


    public Chair getMyChair()
    {
        return myChair;
    }
    // Update is called once per frame
    void Update()
    {
        if (state == NPC_STATE.NPC_WAITING)
        {
            waiting += Time.deltaTime;
            if(limited <= waiting)
            {
                waiting = 0;
                Debug.Log("왜이리 더러워?");
                // 게임 끝
                TargetTable.GuestOut(this);
            }
        }
        else if (state == NPC_STATE.NPC_ORDER)
        {
            if(!GameManager.getInstance().GetFever())
            {
                if (_npcAnim.GetBool("is_fever"))
                {
                    PlayAnimBool("is_fever", false);
                    TurnFeverLight(false);
                }
                waiting += Time.deltaTime;
                if (limited <= waiting)
                {
                    waiting = 0;
                    // 게임 끝 기능 만들어야함
                    TargetTable.GuestOut(this);
                    Destroy(gameObject);
                    if (GameManager.getInstance().LossingNPC()) // 라이프 서치
                        UIManager.getInstance().SetPanel("GameOver");
                }
                // 퍼센트로 구분

            }
            ChangeImageAlpha((int)waiting / limited);
        }
        else
        {
            Debug.Log("NPC_STATE is anything");
        }
    }

    public void setting(Table tb, Chair ch, int m_limite, Food.FoodType type)
    {
        TargetTable = tb;
        myChair = ch;
        ch.use = true;
        state = NPC_STATE.NPC_ORDER;

        foodType = type;   // 주문할 음식 설정
        if (type == Food.FoodType.TYPE_FEVER)
            myChair.SetOrder(true, feverSprite[(int)Random.Range(0, feverSprite.Count)]);
        else
            myChair.SetOrder(true, orderSprite[(int)foodType - 1]);

        TargetTable.Guest_In(this);
        TargetTable.SetOrder(foodType);
        limited = m_limite;

    }

    public void SetFeverOrder()
    {
        if (state != NPC_STATE.NPC_EAT)                             // 먹고 있는 상태가 아닐 경우 변경해 주기
        {
            foodType = Food.FoodType.TYPE_BURGER;
            myChair.SetOrder(true, orderSprite[(int)foodType - 1]);
        }
    }

    public void GuestClear()
    {
        TargetTable.GuestOut(this);
        myChair.tray.SetActive(false);
        myChair.myOrder.SetActive(false);
        if(myFood != null)  
            Destroy(myFood.gameObject);
        Destroy(gameObject);
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        //탱크의 좌우 회전값 갱신
        angleInDegrees += transform.eulerAngles.y;
        //경계 벡터값 반환
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    public void PlayAnimTrigger(string str)
    {
        _npcAnim.SetTrigger(str);
    }

    public void PlayAnimBool(string str, bool is_b)
    {
        _npcAnim.SetBool(str, is_b);
    }

    public void ChangeImageAlpha(float value)
    {
        if (value > 1.0f)
            value = 1.0f;
        else if (value < 0.0f)
            value = 0.0f;

        Color color = myChair.red_Bubble.color;
        color.a = value;
        myChair.red_Bubble.color = color;
    }

    public void TurnFeverLight(bool m_b)
    {
        for(int i = 0; i < _feverLights.Count; i++)
        {
            _feverLights[i].SetActive(m_b);

        }
        //StartCoroutine(ChangeFeverLight());
    }


    public IEnumerator ChangeFeverLight()
    {
        while(true)
        {
            for(int i = 0; i < _feverLights.Count; i++)
            {
                Material[] mat = _feverLights[i].GetComponent<MeshRenderer>().materials;
                mat[0] = _feverMaterial[curLight];
                _feverLights[i].GetComponent<MeshRenderer>().materials = mat;
            }
            curLight++;
            
            
            if(curLight >= _feverMaterial.Count)
            {
                curLight = 0;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }













    #region stateMathod

    public IEnumerator ChangeEat(Food m_food)
    {
        state = NPC_STATE.NPC_EAT;
        //scriptBubble_forward.SetActive(false);
        myChair.SetOrder(false, null);
        myChair.tray.SetActive(true);
        TurnFeverLight(false);
        myFood = Instantiate(m_food, myChair.tray.transform.position + (Vector3.up * 0.2f), Quaternion.identity);
        if(m_food.type == Food.FoodType.TYPE_FRY)
        {
            Debug.Log("케첩 추가 해야함");
        }
        myFood.tag = "Food";
        PlayAnimTrigger("Eat");
        //myFood.GetComponent<Collider>().isTrigger = false;
        // 음식 먹는 애니메이션 재생
        // n초 후 Calc으로 State 변환 // ienumerator로 변경 예정

        yield return new WaitForSeconds(eat_Time);      // 3초 후
        if (myFood != null)
        {
            Destroy(myFood.gameObject);
            state = NPC_STATE.NPC_CALC;     // 계산하고 나감.
            TargetTable.GuestOut(this);
            myChair.tray.SetActive(false);
            Destroy(gameObject);
        }

    }

    #endregion
}
