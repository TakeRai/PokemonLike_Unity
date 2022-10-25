using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shapes2D;


public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightedColor;

    [SerializeField] Text dialogText;
    [SerializeField] GameObject actionSelecter;
    [SerializeField] GameObject moveSelecter;
    [SerializeField] Image moveRightArea;
    [SerializeField] GameObject afterBattleSelecter;

    [SerializeField] List<Sprite> TypeButtons;
    [SerializeField] List<Sprite> TypeIcons;

    void Start()
    {
        moveRightArea.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => StopMove());
    }

    private void StopMove()
    {
        moveSelecter.SetActive(false);
        moveRightArea.gameObject.SetActive(false);

        actionSelecter.SetActive(true);
    }


    public void SetDialog(string dialog) {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelecter.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelecter.SetActive(enabled);
        moveRightArea.gameObject.SetActive(enabled);
    }

    public void EnableAfterBattleSelector(bool enabled)
    {
        afterBattleSelecter.SetActive(enabled);
    }



    public void UpdatePP(int playerMoveIndex, Move move)
    {
        moveSelecter.transform.GetChild(playerMoveIndex).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = $"{move.PP}";

    }



    public void SetMoveFronts(List<Move> moves, List<Button> moveButtons)
    {

        Transform moveImages_i;


        for (int i = 0; i < moveButtons.Count; ++i)
        {
            moveImages_i = moveButtons[i].transform.GetChild(0);

            if (i < moves.Count)
            {
                //つまりmoves[i].Base.Nameが空ということだな！！
                moveImages_i.GetChild(0).GetComponent<Text>().text = moves[i].Base.Name;
                moveImages_i.GetChild(1).GetChild(2).GetComponent<Text>().text = $"{moves[i].Base.PP}";
                moveImages_i.GetChild(1).GetChild(0).GetComponent<Text>().text = $"{moves[i].PP}";

                switch (moves[i].Base.Type)
                {
                    case MonsterType.God:
                        Here(0);
                        break;
                    case MonsterType.Human:
                        Here(1);
                        break;
                    case MonsterType.Sun:
                        Here(2);
                        break;
                    case MonsterType.Earth:
                        Here(3);
                        break;
                    case MonsterType.Moon:
                        Here(4);
                        break;
                    case MonsterType.None:
                        Here(5);
                        break;

                }
            }
            else
                moveImages_i.GetChild(2).GetComponent<Text>().text = "-";
        }

        void Here(int num)
        {
            moveImages_i.GetComponent<Image>().sprite = TypeButtons[num];
            if(num == 5)
            {
                moveImages_i.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                moveImages_i.GetChild(2).gameObject.SetActive(true);
                moveImages_i.GetChild(2).GetComponent<Image>().sprite = TypeIcons[num];
            }
                
        }
    }

    

}
