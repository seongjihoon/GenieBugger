using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBoard : MonoBehaviour
{
    public GameObject prefabs;

    Dictionary<Point, Tile> dic_tiles = new Dictionary<Point, Tile>();

    static public Tile[,] tiles;
    [Tooltip("꼭! 무조건 순서대로 넣기!!")]
    public List<GameObject> obj_List;


    private int m_now_x;
    private int m_now_z;

    public int getMapX()
    {
        return m_now_x;
    }
    public int getMapZ()
    {
        return m_now_z;
    }
    public void DestroyStage()
    {
        for (var i = transform.childCount - 1; i >= 0; --i)
        {
            if(transform.childCount > 0)
            {
                for(var j = transform.GetChild(i).childCount -1; j >= 0; --j)
                {
                    DestroyImmediate(transform.GetChild(i).GetChild(j).gameObject);
                    Debug.Log("지울 객체 속 또 다른 객체를 삭제");
                }
            }    
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        tiles = null;
        Debug.Log("All Destroy Tiles");
    }

    public void MakingStage( int m_ix = 0,  int m_iz = 0)
    {
        if (prefabs == null)
        {
            Debug.Log("Prefabs not specified.");
            return;
        }
        
        if (tiles != null)
            DestroyStage();

        tiles = new Tile[m_ix, m_iz];

        for (int _x = 0, count = 0; _x < m_ix; _x++)
        {
            for(int _z = 0; _z < m_iz; _z++)
            {
                GameObject go = Instantiate(prefabs, new Vector3(_x, 0, _z), new Quaternion(90, 0 ,0,0 ), this.transform);
                go.name = (count++).ToString();
                go.GetComponent<Tile>().CreateSetting(_x, _z);
                tiles[_x, _z] = go.GetComponent<Tile>();
            }
        }
        m_now_z = m_iz;
        m_now_x = m_ix;
    }

    public void MakingObject(int _x, int _z, OBJECT_TYPE obj_type)
    {
        if (tiles == null)
        {
            Debug.LogError("No Floor. plz making stage");
            return;
        }
        if ((_x < 0 && _x > m_now_x )|| (_z < 0 && _z > m_now_z)) // 최대 크기보다 크거나 최소 크기보다 작을 경우 경고 문구 띄우기
        {
            Debug.LogError("out of range");
            return;
        }

        // 만약 범위를 초과하지 않았을 경우.

        if (tiles[_x, _z].transform.childCount > 0 )
        {
            for (var i = tiles[_x, _z].transform.childCount - 1; i >= 0; --i)
            {
                DestroyImmediate(tiles[_x, _z].transform.GetChild(i).gameObject);
            }
        }

        if (OBJECT_TYPE.OBJECT_MOVEABLE == obj_type)
        {
            Debug.Log("Don't Create object Type");
            tiles[_x, _z].CreateObjectOnTile(true, obj_type);
            return;
        }

        GameObject go = Instantiate(obj_List[(int)obj_type - 1], new Vector3(_x, 1, _z), Quaternion.identity, tiles[_x, _z].gameObject.transform);
        tiles[_x,_z].CreateObjectOnTile(false, obj_type);
        Debug.Log("make Object Success");

    }
}
