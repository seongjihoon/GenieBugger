using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chair : MonoBehaviour
{
    //public List<Vector3> wayPoint;        // ���� ��ġ���� ���� ��Ʈ�� ���ڸ��� ���ϰ� ����.

    public bool use = false;    // ���� ���ߴ°�?

    public GameObject tray;

    [Header("��ǳ��?")]
    public GameObject myOrder;
    public Image red_Bubble;
    [Header("������� ������ �������� �ִ� ��")]
    public Image OrderIcon;
    //public Image 


    [Header("������ ����")]
    [Tooltip("������ ��� üũ")]
    [SerializeField]
    private bool is_dir;    // ������ ����

    // vector3.up �����Ϳ� ��ȯ�ϸ� ���� ������?

    private void Start()
    {
        tray .SetActive(false);
        if (myOrder != null)
            myOrder.SetActive(false);
        else
            Debug.Log(this.gameObject.name + "error");
    }

    private void Update()
    {
        

    }

    public bool rt_dir()
    {
        return is_dir;
    }

    public void SetOrder(bool active, Sprite sp)
    {
        if (myOrder != null)
            myOrder.SetActive(active);
        else
            Debug.Log("not have myOrder " + this.gameObject.name);
        OrderIcon.sprite = sp;
    }

    public void Set()
    {

    }

}
