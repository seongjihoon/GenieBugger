using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;





public class FileManager : MonoBehaviour
{
    [System.Serializable]
    public class RoundLv                // ���� �� ���̵�?
    {
        public int Open_Food;                   // ���� �� �ִ� ���ĵ�
        public float Customer_Respawn_Time;     // �մ��� ������ �ð�
        public int Clear_Customer;              // Ŭ���� ����
        public float Wait_Time_Value;
        public void init(string[] str)
        {
            Open_Food = int.Parse(str[1]);
            Customer_Respawn_Time = float.Parse(str[2]);
            Clear_Customer = int.Parse(str[3]);
            Wait_Time_Value = float.Parse(str[4]);
        }
    }

    [System.Serializable]
    public class Customer_Master
    {
        // ���� Customer_Code�� Key������ ���
        public int Round_Code;          // ���� ���庸�� ���ų� ���� ��� ����� ������.
        public int Customer_Type;       // �ճ� ��
        public int Wait_Time;           // �ճ� ��� �ð�
        public bool Customer_Detail;    // ������ �ֳ�?
        public List<Food.FoodType> FoodCode;    // �ճ��� �ֹ��� ����

        public void Init(string[] str)
        {
            FoodCode = new List<Food.FoodType>();
            Round_Code = int.Parse(str[0]);
            Customer_Type = int.Parse(str[1]);

            if (str[2] == "1")
                Customer_Detail = true;
            else
                Customer_Detail = false;

            if (str[4] == "N/A")
                Wait_Time = 0;
            else
                Wait_Time = int.Parse(str[4]);

            for(int i = 5; i < str.Length && str[i] != ""; i++)
            {
                FoodCode.Add((Food.FoodType)(int.Parse(str[i])));
            }
        }
    }

    [System.Serializable]
    public class Customer :SerializableDictionary<int, Customer_Master>
    {

    }

    [System.Serializable]
    public class Rounds : SerializableDictionary<int, RoundLv>
    {

    }

    public Rounds d_round;  // key = level, value = open food, respawnTime, clear customer
    //SerializableDictionary<int, Customer_Master> d_customer; // key = Customer_Code == 2, value ��Ÿ���
    public Customer d_customer;

    public List<Customer_Master> m_listcustomer;                            // ���� ���� �����ϰ� �� ����� ��
    public List<Customer_Master> m_listSonNom;                              // ���� �մ� ���� ��
    public int curRound = 0;
    public int curList = 0;

    static FileManager instance;
    public static FileManager getInstance()
    {
        if(instance == null)
            instance = new FileManager();
        return instance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        m_listcustomer = new List<Customer_Master>();
        IO_GetRound();
        IO_GetLvMaster();
        int key = d_customer.Count;
        Debug.Log(key.ToString());
        GameManager.getInstance().OpenFood(d_round[1].Open_Food);
    }

    public void IO_R()
    {
        // ���� �����

    }

    void IO_GetRound()
    {
        TextAsset _text = (TextAsset)Resources.Load("Round_Master");
        string testFile = _text.text;
        bool endOfFile = false;
        var data_values = testFile.Split('\n');
        int count1 = 0;
        while (!endOfFile)
        {
            RoundLv roundLv = new RoundLv();
            if (count1 == 0)
            {
                count1++;
                continue;
            }
            var data_value = data_values[count1].Split(',');
            if (data_value == null)
            {
                endOfFile = true;
                break;
            }
            if (data_value[0] == "")
            {
                endOfFile = true;
                break;
            }
            roundLv.init(data_value);

            d_round.Add(int.Parse(data_value[0]), roundLv);
            count1++;
        }
    }

    void IO_GetLvMaster()
    {
        TextAsset _text = (TextAsset)Resources.Load("Customer_Master");
        string testFile = _text.text;
        bool endOfFile = false;
        var data_values = testFile.Split('\n'); int count = 0;
        while (!endOfFile)
        {
            Customer_Master cm = new Customer_Master();
            if (count == 0)
            {
                count++;
                continue;
            }
            var data_value = data_values[count].Split(',');
            if (data_value == null)
            {
                endOfFile = true;
                break;
            }
            if (data_value[0] == "")
            {
                endOfFile = true;
                break;
            }
            cm.Init(data_value);
            d_customer.Add(int.Parse(data_value[3]), cm);
            count++;
        }
    }

    void GetRound()
    {
        int rounded = GameManager.getInstance().round;
        if (rounded != curRound)
        {
            for (int cur = 0; cur < d_customer.Count ; cur++)
            {
                Customer_Master cus;
                d_customer.TryGetValue(cur + 1, out cus );
                try
                {
                    if (cus.Round_Code == rounded)
                    {
                        if (cus.Customer_Detail == false)
                            m_listcustomer.Add(cus);
                        else if (cus.Customer_Detail == true)
                            m_listSonNom.Add(cus);
                    }
                }
                catch(System.Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            curRound++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.getInstance().round != curRound)
        {
            GetRound();
        }
    }

    public bool GetPercent(float _percent)
    {
        if(_percent < 0.0001f)
            _percent = 0.0001f;

        bool Success = false;
        int RandAccuracy = 10000;
        float RandHitRanges = _percent * RandAccuracy;
        int Rand = Random.Range(1, RandAccuracy + 1);
        if(Rand <= RandHitRanges)
        {
            Success = true;
        }
        return Success;
    }

    public Customer_Master getDataTable(ref bool guest)
    {
        Customer_Master cus = null;
        int a;
        if (GetPercent(NPCSpawner.GetInstance().getPercent()))
        {
            a = Random.Range(0, m_listSonNom.Count);
            cus = m_listSonNom[a];
            guest = false;
        }
        else
        {
            a = Random.Range(0, m_listcustomer.Count);
            cus = m_listcustomer[a];
            guest = true;
        }
        return cus;
    }

    public int RoundCheck()
    {
        RoundLv rl;
        d_round.TryGetValue(GameManager.getInstance().round, out rl);
        return rl.Clear_Customer;
    }

    public float getRoundSpawnTime()
    {
        RoundLv rl;
        d_round.TryGetValue(GameManager.getInstance().round, out rl);
        return rl.Customer_Respawn_Time ;
    }

    public int getRoundFood()
    {
        RoundLv rl;
        d_round.TryGetValue(GameManager.getInstance().round, out rl);
        return rl.Open_Food;
    }

}
