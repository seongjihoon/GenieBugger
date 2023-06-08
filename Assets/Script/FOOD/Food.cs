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

    // ���� �浹�� ���õ� ��ũ��Ʈ�� ���ʿ���

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
                Debug.Log("������ ���� ����");
                GameManager.getInstance().EndGame();
                // end Panel �����ֱ�
                UIManager.getInstance().SetPanel("GameOver");
            }
            else if (other.tag == "Table")
            {
                Food.FoodType defType = FoodType.TYPE_NONE;
                if (other.GetComponent<Table>().checkOrderNPC(this.gameObject.GetComponent<Food>(), ref defType))  // ���̺� ���� sNPC�� �������� ���� ������ üũ��.
                {
                    if(GameManager.getInstance().GetFever())
                        SoundManager.getInstance().SuccessSound(2);
                    else
                        SoundManager.getInstance().SuccessSound(0);

                    Debug.Log("�ùٸ� ������ ������");
                    if(defType != FoodType.TYPE_NONE)
                        ScoreManager.GetInstance().AddScore(1, defType);     // 1�� ���
                }
                else
                {
                    Debug.Log("�ٸ� ������ ������");
                    SoundManager.getInstance().SuccessSound(1);
                    // end Panel �����ֱ�
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
                Debug.Log("���İ� ����! ��� ��ư Ȱ��ȭ");
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
