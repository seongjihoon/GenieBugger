using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{

    static TableManager instance;
    public Table[] two_tables;
    public Table[] four_tables;
    public int maxTable;
    public int useTable;
    public static TableManager GetInstance() { return instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        maxTable = two_tables.Length + four_tables.Length;
        useTable = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
                
    }

    public Table getTables(int npc_Count)
    {
        Table rt_table = null;

        if (npc_Count > 0 &&npc_Count <= 2)          // 0초과 2미만
        {
            if (twotable_Check())
            {
                while (rt_table == null)
                {
                    int randnum = Random.Range(0, two_tables.Length);
                    if (two_tables[randnum].using_table == false)
                    {
                        rt_table = two_tables[randnum];
                    }
                }
            }
            if(fourtable_Check())
            {
                while (rt_table == null)
                {
                    int randnum = Random.Range(0, four_tables.Length);
                    if (four_tables[randnum].using_table == false)
                    {
                        rt_table = four_tables[randnum];
                    }
                }
            }
        }
        else if (npc_Count > 2 && npc_Count <= 4 )   // 2초과 4미만
        {
            if (fourtable_Check())
            {
                while (rt_table == null)
                {
                    int randnum = Random.Range(0, four_tables.Length);
                    if (four_tables[randnum].using_table == false)
                    {
                        rt_table = four_tables[randnum];
                    }
                }
            }
        }
        if (rt_table == null)
        {
            
            Debug.Log(npc_Count.ToString() + "인이 앉을 자리가 다 참");
            return null;

        }
        else
            useTable++;
        return rt_table;
    }

    public bool twotable_Check()
    {
        for(int i = 0; i < two_tables.Length; i++)
        {
            if (two_tables[i].using_table == false)
                return true;
        }
        return false;
    }
    public bool fourtable_Check()
    {
        for (int i = 0; i < four_tables.Length; i++)
        {
            if (four_tables[i].using_table == false)
                return true;
        }
        return false;
    }

    public bool tableCheck()
    {
        if (useTable < maxTable)
            return true;
        else if (useTable >= maxTable)
            return false;
        //Debug.LogError("Out of Range Table Count");
        return false;
    }

    public void GuestOut()
    {
        useTable--;
    }

    public void ChangeForBugger()
    {
        // 현재 주문한 음식들을 모두 햄버거로 변경하는 스크립트
        for (int tb_c = 0; tb_c < two_tables.Length; tb_c++)
        {
            for (int ch_c = 0; ch_c < two_tables[tb_c].GuestList.Count; ch_c++)
            {
                two_tables[tb_c].GuestList[ch_c].SetFeverOrder();
                two_tables[tb_c].ChangeFeverOrder();
                Debug.Log("ChangeFeverOrder");
            }
        }
        for (int tb_c = 0; tb_c < four_tables.Length; tb_c++)
        {
            for (int ch_c = 0; ch_c < four_tables[tb_c].GuestList.Count; ch_c++)
            {
                four_tables[tb_c].GuestList[ch_c].SetFeverOrder();
                four_tables[tb_c].ChangeFeverOrder();
                Debug.Log("ChangeFeverOrder");
            }
        }
    }

    public void ChangeForGuest()        // 손님 전부 교체
    {
        for (int tb_c = 0; tb_c < two_tables.Length; tb_c++)
        {
            for (int ch_c = two_tables[tb_c].GuestList.Count - 1; ch_c >= 0; ch_c--)
            {
                //two_tables[tb_c].GuestList[ch_c].SetFeverOrder();
                //two_tables[tb_c].ChangeFeverOrder();
                two_tables[tb_c].GuestList[ch_c].GuestClear();
                Debug.Log("ChangeFeverOrder");
            }
        }
        for (int tb_c = 0; tb_c < four_tables.Length; tb_c++)
        {
            for (int ch_c = four_tables[tb_c].GuestList.Count - 1; ch_c >= 0; ch_c--)
            {
                four_tables[tb_c].GuestList[ch_c].GuestClear();
                Debug.Log("ChangeFeverOrder");
            }
        }

        for (int tb_c = 0; tb_c < two_tables.Length; tb_c++)
        {
            NPCSpawner.GetInstance().GuestFull(two_tables[tb_c]);
        }
        for (int tb_c = 0; tb_c < four_tables.Length; tb_c++)
        {
            NPCSpawner.GetInstance().GuestFull(four_tables[tb_c]);

        }
    }

    public void ChangeFeverLight()
    {
        Debug.Log("FeverLight On");
        for (int tb_c = 0; tb_c < two_tables.Length; tb_c++)
        {
            for (int ch_c = two_tables[tb_c].GuestList.Count - 1; ch_c >= 0; ch_c--)
            {
                two_tables[tb_c].GuestList[ch_c].TurnFeverLight(true);
                Debug.Log("ChangeFeverOrder");
            }
        }
        for (int tb_c = 0; tb_c < four_tables.Length; tb_c++)
        {
            for (int ch_c = four_tables[tb_c].GuestList.Count - 1; ch_c >= 0; ch_c--)
            {
                four_tables[tb_c].GuestList[ch_c].TurnFeverLight(true);
                Debug.Log("ChangeFeverOrder");
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
