using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chair : MonoBehaviour
{
    //public List<Vector3> wayPoint;        // 현재 위치까지 오는 루트를 의자마다 지니고 있음.

    public bool use = false;    // 앉음 당했는가?

    public GameObject tray;

    [Header("말풍선?")]
    public GameObject myOrder;
    public Image red_Bubble;
    [Header("대기중인 음식의 아이콘을 넣는 곳")]
    public Image OrderIcon;
    //public Image 


    [Header("의자의 방향")]
    [Tooltip("세로일 경우 체크")]
    [SerializeField]
    private bool is_dir;    // 의자의 방향

    // vector3.up 업벡터에 소환하면 되지 않을까?

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
