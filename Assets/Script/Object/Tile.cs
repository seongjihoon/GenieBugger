using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tile : MonoBehaviour
{
    public Point pos;

    public OBJECT_TYPE tile_type;

    public bool is_move = true;

    public void CreateSetting(int _x, int _z)
    {
        pos.x = _x;
        pos.y = _z;

        is_move = true;
    }
    
    public void CreateObjectOnTile(bool _moveable, OBJECT_TYPE obj_type)
    {
        is_move = _moveable;
        tile_type = obj_type;
        //this.gameObject
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
