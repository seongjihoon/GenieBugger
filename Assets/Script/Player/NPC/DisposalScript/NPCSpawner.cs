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

    public List<float> SonNomRespawn;  // 라운드당 생성

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
        // n_1 ~ n_2의 시간을 랜덤으로 설정하여 해당 시간이 경과할 경우
        if(!GameManager.getInstance () .getGameState())
        {
            // 현재 진행중인 게임이 없음
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
            Debug.Log("테이블이 없소");
        }
        else
        {
            for (int i = 0; i < guestNum; i++)
            {
                int cope = Random.Range(0, npcPrefabs.Count);
                Chair chair = target_tb.getChair();     // 뽑는 김에 tray도 뽑을 수 있음 좋겠는데... 보는 방향을 트레이로 하고 음식 스폰위치도 그렇고
                NPC npcGO = Instantiate(npcPrefabs[cope], chair.transform.position - new Vector3(0.0f, 0.2f ,0.0f), Quaternion.identity); // 소환하는 위치를 받아와야함.

                npcGO.setting(target_tb, chair, cus.Wait_Time, cus.FoodCode[i]);       // 테이블은 for 문 밖에서 해결하고 for문 안에선 선택된 테이블의 의자를 target으로 선택
                //else if(GameManager.getInstance().GetFever())
                //    npcGO.setting(target_tb, chair, cus.Wait_Time, Food.FoodType.TYPE_BURGER);  // 햄버거만 주문 fever mode일 땐

                npcGO.transform.LookAt(new Vector3(chair.tray.transform.position.x,
                    npcGO.transform.position.y, chair.tray.transform.position.z));
                npcGO.name = "NPC_" + i.ToString();
                if(!normalGuest)
                {
                    Debug.Log("손놈!");
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
                Chair chair = tb.getChair();     // 뽑는 김에 tray도 뽑을 수 있음 좋겠는데... 보는 방향을 트레이로 하고 음식 스폰위치도 그렇고

                NPC npcGO = Instantiate(npcPrefabs[cope], chair.transform.position - new Vector3(0.0f, 0.2f, 0.0f), Quaternion.identity); // 소환하는 위치를 받아와야함.

                //if (!GameManager.getInstance().GetFever())
                npcGO.setting(tb, chair, 10, Food.FoodType.TYPE_FEVER);       // 테이블은 for 문 밖에서 해결하고 for문 안에선 선택된 테이블의 의자를 target으로 선택

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
