using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public enum FoodType
    {
        TYPE_NONE = 0,
        TYPE_BURGER,
        TYPE_COKE,
        TYPE_FRY,
        TYPE_HOTDOG,
        TYPE_ICECREAM,
        TYPE_FEVER,
        TYPE_END,
    }

    public FoodType type;

    public Food FoodPrefabs;

    // 음식 충돌과 관련된 스크립트는 이쪽에서

    private void OnTriggerEnter(Collider other)
    {
        if(this.gameObject.tag == "FlyingFood")
        {
            if (other.tag == "Trash")
            {
                SoundManager.getInstance().SuccessSound(Random.Range(3, 5));
                Destroy(this.gameObject);
                GameManager.getInstance().ResetFeverCount();
            }
            else if (other.tag == "Wall")
            {
                Debug.Log("음식이 벽에 닿음");
                GameManager.getInstance().EndGame();
                // end Panel 보여주기
                UIManager.getInstance().SetPanel("GameOver");
            }
            else if (other.tag == "Table")
            {
                Food.FoodType defType = FoodType.TYPE_NONE;
                if (other.GetComponent<Table>().checkOrderNPC(this.gameObject.GetComponent<Food>(), ref defType))  // 테이블에 앉은 sNPC를 기준으로 받은 음식을 체크함.
                {
                    if(GameManager.getInstance().GetFever())
                        SoundManager.getInstance().SuccessSound(2);
                    else
                        SoundManager.getInstance().SuccessSound(0);

                    Debug.Log("올바른 음식이 도착함");
                    if(defType != FoodType.TYPE_NONE)
                        ScoreManager.GetInstance().AddScore(1, defType);     // 1점 상승
                }
                else
                {
                    Debug.Log("다른 음식이 도착함");
                    SoundManager.getInstance().SuccessSound(1);
                    // end Panel 보여주기
                    if (GameManager.getInstance().LossingNPC())
                    {
                        GameManager.getInstance().EndGame();
                        UIManager.getInstance().SetPanel("GameOver");
                    }
                }
                Destroy(this.gameObject);
            }
        }
        else if(this.gameObject.tag == "Food" && other.tag == "Player")
        {
            PlayerThrow pt = other.GetComponent<PlayerThrow>(); 
            if (other.tag == "Food" && pt.is_lockon == false)
            {
                pt.is_food = true;
                Debug.Log("음식과 접촉! 잡기 버튼 활성화");
                pt.ChangeColorAlpha(1.0f);
                pt.linkedFood = other.GetComponent<Food>();
                pt.getButton.enabled = true;
            }
        }
    }


    public void FoodSet(bool T)
    {
        gameObject.GetComponent<Collider>().enabled = T;
    }

}
