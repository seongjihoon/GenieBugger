using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public VariableJoystick joystick;
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    //public string shotButtonName = "Fire1";

    [SerializeField]
    public float move { get; private set; }
    public float rotate { get; private set; }
    //public bool shot { get; private set; }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.getInstance().getGameState()) // GameManager로 인한 Input을 받을지 여부
        {
            move = 0;
            rotate = 0;
            //shot = false;
            return;
        }
#if UNITY_ANDROID
        move = joystick.Vertical;
        rotate = joystick.Horizontal;
   
#else

        rotate = Input.GetAxis(rotateAxisName);
        move = Input.GetAxis(moveAxisName);

        //shot = Input.GetButton(shotButtonName);
#endif
    }

}
