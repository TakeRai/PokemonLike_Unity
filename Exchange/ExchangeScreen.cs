using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.SerializableAttribute]
public class TypeTextAreas
{
    public List<GameObject> List = new List<GameObject>();

    public TypeTextAreas(List<GameObject> list)
    {
        List = list;
    }

}

public class ExchangeScreen : MonoBehaviour
{
    [SerializeField] List<Button> exPanelButtons;
    [SerializeField] Image choicedExPanel;

    [SerializeField] List<Button> holdPanelButtons;
    [SerializeField] Image choicedHoldPanel;

    [SerializeField] ChoiceScreen choiceScreen;
    [SerializeField] BattleHud battleHud;
    [SerializeField] Text remainCostText;

    MonsterBase choicedExMonster;
    MonsterBase choicedHoldMonster;

    


    void Start()
    {
        MonsterBase choicedExMonster = new MonsterBase();
        MonsterBase choicedHoldMonster = new MonsterBase();


        for (int i=0; i<exPanelButtons.Count; i++)
        {
            int num = i;
            exPanelButtons[i].onClick.AddListener(() => devideExchangePanel(num));
        }

        for (int i = 0; i < holdPanelButtons.Count; i++)
        {
            int count = i;
            holdPanelButtons[i].onClick.AddListener(() => devideHoldPanel(count));
        }
    }


    public void RenderExchangeMonsters()
    {
        for(var i=0; i<3; i++)
        {

            List<GameObject> typetextareas_i_ex = new();
            Transform exButton_i = exPanelButtons[i].transform;

            exButton_i.GetChild(0).GetComponent<Text>().text = choiceScreen.ComParty[i].Name;
            exButton_i.GetChild(1).GetComponent<Image>().sprite = choiceScreen.ComParty[i].TheSprite;
            typetextareas_i_ex.Add(exButton_i.GetChild(2).GetChild(0).gameObject);
            typetextareas_i_ex.Add(exButton_i.GetChild(2).GetChild(1).gameObject);
            battleHud.SetType(choiceScreen.ComParty[i], typetextareas_i_ex);
            exButton_i.GetChild(3).GetChild(0).GetComponent<Text>().text = choiceScreen.ComParty[i].Cost.ToString();


            List<GameObject> typetextareas_i_hold = new();
            Transform holdButton_i = holdPanelButtons[i].transform;

            holdButton_i.GetChild(0).GetComponent<Text>().text = choiceScreen.ChoiceMonsters[i].Name;
            holdButton_i.GetChild(1).GetComponent<Image>().sprite = choiceScreen.ChoiceMonsters[i].TheSprite;
            typetextareas_i_hold.Add(holdButton_i.GetChild(2).GetChild(0).gameObject);
            typetextareas_i_hold.Add(holdButton_i.GetChild(2).GetChild(1).gameObject);
            battleHud.SetType(choiceScreen.ChoiceMonsters[i], typetextareas_i_hold);
            holdButton_i.GetChild(3).GetChild(0).GetComponent<Text>().text = choiceScreen.ChoiceMonsters[i].Cost.ToString();

        }
    }

    private void devideExchangePanel(int num)
    {
        if(choicedExMonster == choiceScreen.ComParty[num])
        {
            choicedExMonster = new MonsterBase();
        }
        else
        {
            choicedExMonster = choiceScreen.ComParty[num];
        }
        choicedExPanel.sprite = choicedExMonster.TheSprite;
        print(choicedExMonster);
        print("devideEx");

    }

    private void devideHoldPanel(int num)
    {
        if(choicedHoldMonster == choiceScreen.ChoiceMonsters[num])
        {
            choicedHoldMonster = new MonsterBase();
        }
        else
        {
            choicedHoldMonster = choiceScreen.ChoiceMonsters[num];
        }
        
        choicedHoldPanel.sprite = choicedHoldMonster.TheSprite;
        print(choicedHoldMonster);
        print("devideHol");

    }

    public void devideParty()
    {

        print(choicedHoldMonster);
        if(choicedHoldMonster != null && choicedExMonster != null)
        {
            int changeHoldIndex = choiceScreen.ChoiceMonsters.IndexOf(choicedHoldMonster);
            choiceScreen.ChoiceMonsters[changeHoldIndex] = choicedExMonster;
        }
        else
        {
            print("aaa");
        }
    }
}




