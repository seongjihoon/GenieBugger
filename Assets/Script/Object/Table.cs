using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{


    public Chair[] ChairList;

    public List<NPC> GuestList;                 // �� ���̺� ���� NPC�� �����ϸ鼭 NPC�� �� �� �ɾҴ����� Ȯ����


    public List<Food.FoodType> FoodList;             // ���� �ֹ� ���� ������ ����

    [System.Serializable]
    public class SerializeDicVector : SerializableDictionary<Vector3, bool> { }

    public int chairCount;
    public int useCount;
    public bool using_table = false;


    // Start is called before the first frame update
    void Start()
    {
        using_table = false;    // ��� ���ΰ�?
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

    // ���̺� ��ϵ� �ֹ� ������� üũ
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

    // ���̺� ���� NPC�� �������� �ֹ� üũ 
    public bool checkOrderNPC(Food m_food, ref Food.FoodType type)
    {
        for (int i = 0; i < GuestList.Count; i++)
        {
            if ((m_food.type == GuestList[i].foodType && GuestList[i].state == NPC_STATE.NPC_ORDER) 
                || (GuestList[i].foodType == Food.FoodType.TYPE_FEVER && GuestList[i].state == NPC_STATE.NPC_ORDER))
            {
                // ���� ������ ������ �ֹ����� ���. 
                // Fever �迭�� �մ��� ���
                StartCoroutine(GuestList[i].ChangeEat(m_food));           // <- ���� ���� ����� �԰� ������.
                type = GuestList[i].foodType;
                return true;
            }
        }

        return false;   // �ٸ� ������ �ֹ��Ǿ���.
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
