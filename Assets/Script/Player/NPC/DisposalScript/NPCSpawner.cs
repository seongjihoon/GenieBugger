using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    static NPCSpawner instance = null;

    public static NPCSpawner GetInstance()
    {
        if (instance == null)
            instance = new NPCSpawner();
        return instance;
    }

    public List<NPC> npcPrefabs;

    public float spawn_maxTimer = 10;

    public int defualtLimite = 10;

    public float time;

    public List<float> SonNomRespawn;  // ����� ����

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        NextRound();
    }

    // Update is called once per frame
    void Update()
    {
        // n_1 ~ n_2�� �ð��� �������� �����Ͽ� �ش� �ð��� ����� ���
        if(!GameManager.getInstance () .getGameState())
        {
            // ���� �������� ������ ����
            return;
        }
        if(TableManager.GetInstance().tableCheck() && !GameManager.getInstance().GetFever())
        {
            time += Time.deltaTime;
            if (spawn_maxTimer < time)
            {
                //Debug.Log("Time: " + time);
                SpawnNPC();
                time = 0;
            }
        }
    }

    public void SpawnNPC()
    {
        bool normalGuest = false;
        FileManager.Customer_Master cus = FileManager.getInstance().getDataTable(ref normalGuest);
        int guestNum = cus.Customer_Type;
        Table target_tb = TableManager.GetInstance().getTables(guestNum);

        if (target_tb == null)
        {
            Debug.Log("���̺��� ����");
        }
        else
        {
            for (int i = 0; i < guestNum; i++)
            {
                int cope = Random.Range(0, npcPrefabs.Count);
                Chair chair = target_tb.getChair();     // �̴� �迡 tray�� ���� �� ���� ���ڴµ�... ���� ������ Ʈ���̷� �ϰ� ���� ������ġ�� �׷���
                NPC npcGO = Instantiate(npcPrefabs[cope], chair.transform.position - new Vector3(0.0f, 0.2f ,0.0f), Quaternion.identity); // ��ȯ�ϴ� ��ġ�� �޾ƿ;���.

                npcGO.setting(target_tb, chair, cus.Wait_Time, cus.FoodCode[i]);       // ���̺��� for �� �ۿ��� �ذ��ϰ� for�� �ȿ��� ���õ� ���̺��� ���ڸ� target���� ����
                //else if(GameManager.getInstance().GetFever())
                //    npcGO.setting(target_tb, chair, cus.Wait_Time, Food.FoodType.TYPE_BURGER);  // �ܹ��Ÿ� �ֹ� fever mode�� ��

                npcGO.transform.LookAt(new Vector3(chair.tray.transform.position.x,
                    npcGO.transform.position.y, chair.tray.transform.position.z));
                npcGO.name = "NPC_" + i.ToString();
                if(!normalGuest)
                {
                    Debug.Log("�ճ�!");
                }
            }
            target_tb.using_table = true;
            SoundManager.getInstance().guestSound(guestNum);
        }
    }

    public void GuestFull(Table tb)
    {
        //LevelManager.Customer_Master cus = LevelManager.getInstance().getDataTable();
        
        int guestNum = Random.Range(1, tb.chairCount);
        if(guestNum > tb.chairCount)
            guestNum = tb.chairCount;
        TableManager.GetInstance().useTable = TableManager.GetInstance().maxTable;

        for (int i = 0; i < guestNum; i++)
        {
            try
            {
                int cope = Random.Range(0, npcPrefabs.Count);
                Chair chair = tb.getChair();     // �̴� �迡 tray�� ���� �� ���� ���ڴµ�... ���� ������ Ʈ���̷� �ϰ� ���� ������ġ�� �׷���

                NPC npcGO = Instantiate(npcPrefabs[cope], chair.transform.position - new Vector3(0.0f, 0.2f, 0.0f), Quaternion.identity); // ��ȯ�ϴ� ��ġ�� �޾ƿ;���.

                //if (!GameManager.getInstance().GetFever())
                npcGO.setting(tb, chair, 10, Food.FoodType.TYPE_FEVER);       // ���̺��� for �� �ۿ��� �ذ��ϰ� for�� �ȿ��� ���õ� ���̺��� ���ڸ� target���� ����

                npcGO.transform.LookAt(new Vector3(chair.tray.transform.position.x,
                    npcGO.transform.position.y, chair.tray.transform.position.z));
                npcGO.name = "NPC_" + i.ToString();
                if(GameManager.getInstance().GetFever())
                {
                    npcGO.PlayAnimBool("is_fever", true);
                    npcGO.TurnFeverLight(true);
                    StartCoroutine(npcGO.ChangeFeverLight());
                }
            }
            catch(System.Exception e)
            {
                Debug.Log(e);
            }
        }
        tb.using_table = true;

    }

    public float getPercent()
    {
        return SonNomRespawn[GameManager.getInstance().round] / 100;
        //return 100;
    }

    public void NextRound()
    {
        spawn_maxTimer = FileManager.getInstance().getRoundSpawnTime();

    }


}
