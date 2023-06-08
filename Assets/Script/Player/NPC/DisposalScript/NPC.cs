using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum NPC_STATE
{
    NPC_NONE,   // ���̺��
    NPC_WAITING,    // ġ������ ���� ���̺�� �̵����� ��
    NPC_ORDER,  // �ֹ�
    NPC_EAT,    // �Դ� �ִϸ��̼� ���
    NPC_CALC,   // ���
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
    [Header("�ֹ��� ���� ����")]
    public Food.FoodType foodType;

    //public int myOrder;         // enum���� �ٲ� ���� ���� �ֹ��� ������ �����ϰ� ��ٸ��� ����

    [Header("Ÿ�̸�(���콺 �÷��� ��� Ȯ��)")]
    [Tooltip("��ٸ��� �ð� �Ѱ���.")]
    public float limited = 5;         // �Ѱ���
    [Tooltip("�Դ� �ð�")]
    public float eat_Time = 3;
    public Animator _npcAnim;

    [Header("�ֹ� �ð�ȭ ����")]
    public List<Sprite> orderSprite;
    public List<Sprite> feverSprite;

    public NPC_STATE state;
    [Space(10f)]
    [Header("�߱���")]
    public List<GameObject> _feverLights;
    public List<Material> _feverMaterial;
    [SerializeField]
    private int curLight = 0;
    [Space(10f)]
    [Header("�ճ� ��")]
    public GameObject npc_Face;
    public List<Material> _faceMaterial; 


    // private 
    [Header("�Ʒ��� private. �ǵ�� �ȵ�...")]
    [SerializeField]
    private float waiting = 0;         // ��ٸ��� �ð� n�� �� ȭ���鼭 ����.

    [Header("���� �ɾ��ִ� ���̺� ��ġ")]
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
                Debug.Log("���̸� ������?");
                // ���� ��
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
                    // ���� �� ��� ��������
                    TargetTable.GuestOut(this);
                    Destroy(gameObject);
                    if (GameManager.getInstance().LossingNPC()) // ������ ��ġ
                        UIManager.getInstance().SetPanel("GameOver");
                }
                // �ۼ�Ʈ�� ����

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

        foodType = type;   // �ֹ��� ���� ����
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
        if (state != NPC_STATE.NPC_EAT)                             // �԰� �ִ� ���°� �ƴ� ��� ������ �ֱ�
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
        //��ũ�� �¿� ȸ���� ����
        angleInDegrees += transform.eulerAngles.y;
        //��� ���Ͱ� ��ȯ
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
            Debug.Log("��ø �߰� �ؾ���");
        }
        myFood.tag = "Food";
        PlayAnimTrigger("Eat");
        //myFood.GetComponent<Collider>().isTrigger = false;
        // ���� �Դ� �ִϸ��̼� ���
        // n�� �� Calc���� State ��ȯ // ienumerator�� ���� ����

        yield return new WaitForSeconds(eat_Time);      // 3�� ��
        if (myFood != null)
        {
            Destroy(myFood.gameObject);
            state = NPC_STATE.NPC_CALC;     // ����ϰ� ����.
            TargetTable.GuestOut(this);
            myChair.tray.SetActive(false);
            Destroy(gameObject);
        }

    }

    #endregion
}
