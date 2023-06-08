using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThrow : MonoBehaviour        // 본 스크립트는 버튼 작동으로 인한 Player의 State 변환과 행동을 관리한다.
{
    public bool is_food;
    public bool is_lockon;

    [Header("날아가는 속도")]
    public int ThrowPower = 5;
    [Header("속도 배율 (n배 상승)")]
    public int Speed_multiple = 100;

    // 음식이 어디에 들려있어야할까?

    [Space(10f)]
    public Button getButton;
    public Food linkedFood;  // 눌렀을 때 들고있는 음식의 종류
    public Food myHand;     // 현재 들고 있는 음식

    public GameObject ThrowPos;

    [Header("Audio")]
    public List<AudioClip> m_playerSounds;
    private AudioSource playAudio;

    private Animator anim;
    private void Start()
    {
        is_food = false;
        getButton.GetComponent<Button>().enabled = false;
        anim = GetComponent<Animator>();
        playAudio = GetComponent<AudioSource>();
        ChangeColorAlpha(0.5f);

    }

    public void ChangeColorAlpha(float value)
    {
        if (value > 1.0f)
            value = 1.0f;
        else if(value < 0.0f)
            value = 0.0f;

        Color color = getButton.GetComponent<Image>().color;
        color.a = value;
        getButton.GetComponent<Image>().color = color;
    }

    private void OnTriggerEnter(Collider other)         // UI매니저를 통해 잡기 버튼 활성화 및 플레이어의 forward.Vector 방향 화살표 UI 활성화
    {
        //if (other.tag == "Food" && is_lockon == false)
        //{
        //    is_food = true;
        //    Debug.Log("음식과 접촉! 잡기 버튼 활성화");
        //    ChangeColorAlpha(1.0f);
        //    linkedFood = other.GetComponent<Food>();
        //    getButton.enabled = true;
        //}
    }

    private void Update()
    {
        if(GameManager.getInstance().getGameState() && myHand != null)
        {
            myHand.transform.position = ThrowPos.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Food" && linkedFood != other.GetComponent<Food>() && is_lockon == false)
        {
            is_food = true;
            Debug.Log("음식과 접촉! 잡기 버튼 활성화");
            ChangeColorAlpha(1.0f);
            linkedFood = other.GetComponent<Food>();
            getButton.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)          // UI매니저를 통해 잡기 버튼 및 방향벡터 비활성화
    {
        if(other.tag =="Food" && is_lockon == false)
        {
            is_food = false;
            Debug.Log("음식과 멀어짐! 잡기 버튼 비활성화");
            ChangeColorAlpha(.5f);
            linkedFood = null;
            getButton.enabled = false;
        }
    }

    public void GetFood()
    {
        if (is_food == true && is_lockon == false)
        {
            playAudio.clip = m_playerSounds[3]; // 3번째 사운드 집기
            playAudio.Play();
            myHand = Instantiate(linkedFood.FoodPrefabs, ThrowPos.transform.position, Quaternion.identity);   // 물체 생성
            is_lockon = true;
            Debug.Log("아이템을 집었습니다.");
            anim.SetBool("LockOn", true);
            ChangeColorAlpha(.7f);
        }
    }

    public void Throw()                                 // 버튼을 뗄 경우 바라보는 방향으로 던지기
    {
        if (is_food == true && myHand != null)
        {
            try
            {
                myHand.GetComponent<Rigidbody>().AddForce(transform.forward * (Speed_multiple * ThrowPower));
                myHand.tag = "FlyingFood";
                Destroy(myHand.gameObject, 3f);         // 충돌관련 옵션으로 변경 예정
                                                        // 던지기 구현
                Debug.Log("아이템을 던졌습니다.");
                myHand = null;
                anim.SetTrigger("Shooting");
                StartCoroutine(PlayThrowAnim());
                ChangeColorAlpha(1.0f);
            }
            catch(System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    public IEnumerator PlayThrowAnim()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("LockOn", false);
        playAudio.clip = m_playerSounds[Random.Range(0, 3)];
        playAudio.Play();
        yield return new WaitForSeconds(0.55f);
        is_lockon = false;

    }


}        
