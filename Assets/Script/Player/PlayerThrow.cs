using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThrow : MonoBehaviour        // �� ��ũ��Ʈ�� ��ư �۵����� ���� Player�� State ��ȯ�� �ൿ�� �����Ѵ�.
{
    public bool is_food;
    public bool is_lockon;

    [Header("���ư��� �ӵ�")]
    public int ThrowPower = 5;
    [Header("�ӵ� ���� (n�� ���)")]
    public int Speed_multiple = 100;

    // ������ ��� ����־���ұ�?

    [Space(10f)]
    public Button getButton;
    public Food linkedFood;  // ������ �� ����ִ� ������ ����
    public Food myHand;     // ���� ��� �ִ� ����

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

    private void OnTriggerEnter(Collider other)         // UI�Ŵ����� ���� ��� ��ư Ȱ��ȭ �� �÷��̾��� forward.Vector ���� ȭ��ǥ UI Ȱ��ȭ
    {
        //if (other.tag == "Food" && is_lockon == false)
        //{
        //    is_food = true;
        //    Debug.Log("���İ� ����! ��� ��ư Ȱ��ȭ");
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
            Debug.Log("���İ� ����! ��� ��ư Ȱ��ȭ");
            ChangeColorAlpha(1.0f);
            linkedFood = other.GetComponent<Food>();
            getButton.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)          // UI�Ŵ����� ���� ��� ��ư �� ���⺤�� ��Ȱ��ȭ
    {
        if(other.tag =="Food" && is_lockon == false)
        {
            is_food = false;
            Debug.Log("���İ� �־���! ��� ��ư ��Ȱ��ȭ");
            ChangeColorAlpha(.5f);
            linkedFood = null;
            getButton.enabled = false;
        }
    }

    public void GetFood()
    {
        if (is_food == true && is_lockon == false)
        {
            playAudio.clip = m_playerSounds[3]; // 3��° ���� ����
            playAudio.Play();
            myHand = Instantiate(linkedFood.FoodPrefabs, ThrowPos.transform.position, Quaternion.identity);   // ��ü ����
            is_lockon = true;
            Debug.Log("�������� �������ϴ�.");
            anim.SetBool("LockOn", true);
            ChangeColorAlpha(.7f);
        }
    }

    public void Throw()                                 // ��ư�� �� ��� �ٶ󺸴� �������� ������
    {
        if (is_food == true && myHand != null)
        {
            try
            {
                myHand.GetComponent<Rigidbody>().AddForce(transform.forward * (Speed_multiple * ThrowPower));
                myHand.tag = "FlyingFood";
                Destroy(myHand.gameObject, 3f);         // �浹���� �ɼ����� ���� ����
                                                        // ������ ����
                Debug.Log("�������� �������ϴ�.");
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
