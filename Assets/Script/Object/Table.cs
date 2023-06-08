using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{


    public Chair[] ChairList;

    public List<NPC> GuestList;                 // 현 테이블에 앉은 NPC를 구분하면서 NPC가 몇 명 앉았는지도 확인함


    public List<Food.FoodType> FoodList;             // 현재 주문 받은 음식의 갯수

    [System.Serializable]
    public class SerializeDicVector : SerializableDictionary<Vector3, bool> { }

    public int chairCount;
    public int useCount;
    public bool using_table = false;


    // Start is called before the first frame update
    void Start()
    {
        using_table = false;    // 사용 중인가?
        chairCount = ChairList.Length;
        useCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Guest_In(NPC npc)
    {
        if (npc != null)
            GuestList.Add(npc);
        else
            Debug.LogError("npc isn't Instantiate.");
    }
    public void GuestOut(NPC npc)
    {
        GuestList.Remove(npc);
        for (int i = 0; i < ChairList.Length; i++)
        {
            if (ChairList[i] == npc.getMyChair())
            {
                ChairList[i].use = false;
                ChairList[i].myOrder.SetActive(false);
            }
        }
        if (GuestList.Count <= 0)
        {
            using_table = false;

            TableManager.GetInstance().GuestOut();
        }

    }

    public void ChangeFeverOrder()
    {
        for(int i =0; i < FoodList.Count; i++)
        {
            FoodList[i] = Food.FoodType.TYPE_BURGER;
        }
    }

    public void SetOrder(Food.FoodType type)
    {
        FoodList.Add(type);
    }

    // 테이블에 등록된 주문 목록으로 체크
    public bool checkOrder(Food _food)
    {
        for (int i = 0; i < FoodList.Count; i++)
        {
            if (_food.type == FoodList[i])
            {
                FoodList.Remove(_food.type);
                return true;
            }
        }
        return false;
    }

    // 테이블에 앉은 NPC를 기준으로 주문 체크 
    public bool checkOrderNPC(Food m_food, ref Food.FoodType type)
    {
        for (int i = 0; i < GuestList.Count; i++)
        {
            if ((m_food.type == GuestList[i].foodType && GuestList[i].state == NPC_STATE.NPC_ORDER) 
                || (GuestList[i].foodType == Food.FoodType.TYPE_FEVER && GuestList[i].state == NPC_STATE.NPC_ORDER))
            {
                // 같은 종류의 음식을 주문했을 경우. 
                // Fever 계열의 손님일 경우
                StartCoroutine(GuestList[i].ChangeEat(m_food));           // <- 먼저 받은 사람이 먹고 나간다.
                type = GuestList[i].foodType;
                return true;
            }
        }

        return false;   // 다른 음식이 주문되었다.
    }

    public Chair getChair()
    {
        Chair rt_chair;
        do
        {
            int rand = Random.Range(0, ChairList.Length);
            rt_chair = ChairList[rand];
        } while (rt_chair.use);

        return rt_chair;
    }

}
