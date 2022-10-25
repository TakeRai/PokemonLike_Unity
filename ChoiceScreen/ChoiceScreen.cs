using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;

//変数作ってその中にrandom後の結果入れてやればいいのか

public class ChoiceScreen : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] BattleHud battleHud;
    [SerializeField] Button goBattleButton;
    [SerializeField] RectTransform costPanels_content;
    [SerializeField] List<Button> choicedPanels;
    [SerializeField] Text remainCostText;
    [SerializeField] RectTransform costCones;
    [SerializeField] costListList costList;
    [SerializeField] MonsterBase zzzzz;

    [System.NonSerialized] public List<MonsterBase> choiceMonsters = new();


    [System.NonSerialized] public List<List<MonsterBase>> playerCostListList;
    [System.NonSerialized] public List<List<MonsterBase>> comCostListList;

    [System.NonSerialized] public List<MonsterBase> displayMonsterBaseLists = new List<MonsterBase>();
    string choiceName;

    [System.NonSerialized] public int playerPartyCost = 15;
    [System.NonSerialized] public int comPartyCost = 15;
    

    [System.NonSerialized] public List<MonsterBase> comParty = new List<MonsterBase>();
    [System.NonSerialized] public List<MonsterBase> newComParty = new List<MonsterBase>();

    void Start() 
    {
        for(var i=0; i<3; i++)
        {
            choiceMonsters.Add(zzzzz);
        }

        for (var i=0; i < costPanels_content.transform.childCount; i++)
        {
            var num = i;

            Button costPanel_i = costPanels_content.transform.GetChild(num).GetComponent<Button>();
            costPanel_i.onClick.AddListener(() => decideMonster(num));
        }

        for (var i = 0; i < 3; i++)
        {
            var num = i;
            choicedPanels[i].onClick.AddListener(() => RemoveFromParty(num));
        }
    }

    void decideMonster(int num)
    {
        if (choiceMonsters.Contains(zzzzz) &&
            playerPartyCost - (num + 1) >= 0 &&
            !choiceMonsters.Contains(displayMonsterBaseLists[num]))

        {

            choiceMonsters.Add(displayMonsterBaseLists[num]);
            int prePlayerPartyCost = playerPartyCost;
            playerPartyCost -= (num + 1);
            remainCostText.text = $"{playerPartyCost}";

            float toukaNum = 0.2f;
            float toukaTime = 0.3f;
            Transform costpanel_here = costPanels_content.transform.GetChild(num);
            Transform costfront_here = costPanels_content.transform.GetChild(num).GetChild(0);


            for (var i=0; i<3; i++)
            {

                if(i >= choiceMonsters.Count)
                {
                    choicedPanels[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    choicedPanels[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = choiceMonsters[i].TheSprite;
                    choicedPanels[i].transform.GetChild(1).gameObject.SetActive(true);


                    //コストコーン
                    DOTween.ToAlpha(
                    () => costpanel_here.GetChild(1).GetComponent<Image>().color,
                    color => costpanel_here.GetChild(1).GetComponent<Image>().color = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costpanel_here.GetChild(1).GetChild(0).GetComponent<Text>().color,
                    color => costpanel_here.GetChild(1).GetChild(0).GetComponent<Text>().color = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costpanel_here.GetChild(1).GetChild(0).GetComponent<Outline>().effectColor,
                    color => costpanel_here.GetChild(1).GetChild(0).GetComponent<Outline>().effectColor = color,
                    toukaNum,
                    toukaTime
                    );

                    costfront_here.GetComponent<Image>().enabled = false;
                    costfront_here.GetChild(0).gameObject.SetActive(true);

                    //costFronts
                    DOTween.ToAlpha(
                    () => costfront_here.GetComponent<Image>().color,
                    color => costfront_here.GetComponent<Image>().color = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costfront_here.GetChild(1).GetChild(0).GetComponent<Text>().color,
                    color => costfront_here.GetChild(1).GetChild(0).GetComponent<Text>().color = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costfront_here.GetChild(1).GetChild(0).GetComponent<Outline>().effectColor,
                    color => costfront_here.GetChild(1).GetChild(0).GetComponent<Outline>().effectColor = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costfront_here.GetChild(2).GetChild(0).GetComponent<Image>().color,
                    color => costfront_here.GetChild(2).GetChild(0).GetComponent<Image>().color = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costfront_here.GetChild(3).GetChild(0).GetComponent<Image>().color,
                    color => costfront_here.GetChild(3).GetChild(0).GetComponent<Image>().color = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costfront_here.GetChild(3).GetChild(1).GetComponent<Image>().color,
                    color => costfront_here.GetChild(3).GetChild(1).GetComponent<Image>().color = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costfront_here.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().color,
                    color => costfront_here.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().color = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costfront_here.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().color,
                    color => costfront_here.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().color = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costfront_here.GetChild(3).GetChild(0).GetChild(0).GetComponent<Outline>().effectColor,
                    color => costfront_here.GetChild(3).GetChild(0).GetChild(0).GetComponent<Outline>().effectColor = color,
                    toukaNum,
                    toukaTime
                    );

                    DOTween.ToAlpha(
                    () => costfront_here.GetChild(3).GetChild(1).GetChild(0).GetComponent<Outline>().effectColor,
                    color => costfront_here.GetChild(3).GetChild(1).GetChild(0).GetComponent<Outline>().effectColor = color,
                    toukaNum,
                    toukaTime
                    );
                }
            }

            

           //15 - 10

            for (var i= playerPartyCost; i < prePlayerPartyCost; i++)
            {

                var index_here = i;
                DOTween.ToAlpha(
                () => costCones.transform.GetChild(index_here).GetChild(0).GetComponent<Image>().color,
                color => costCones.transform.GetChild(index_here).GetChild(0).GetComponent<Image>().color = color,
                0.2f,
                0.4f
                );
            }
            

        }
        else
        {
            print("入りません");
        }



        if(choiceMonsters.Count == 3)
        {
            DOTween.ToAlpha(
            () => goBattleButton.transform.GetChild(0).GetComponent<Image>().color,
            color => goBattleButton.GetComponent<Image>().color = color,
            0.4f,
            0.25f
            );

            DOTween.ToAlpha(
            () => goBattleButton.transform.GetChild(1).GetComponent<Image>().color,
            color => goBattleButton.transform.GetChild(1).GetComponent<Image>().color = color,
            1f,
            0.25f
            );

            DOTween.ToAlpha(
            () => goBattleButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().color,
            color => goBattleButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = color,
            1f,
            0.25f
            );

            DOTween.ToAlpha(
            () => goBattleButton.transform.GetChild(1).GetChild(1).GetComponent<Image>().color,
            color => goBattleButton.transform.GetChild(1).GetChild(1).GetComponent<Image>().color = color,
            1f,
            0.25f
            );
        }
    }

    

    public void SetCostText()
    {
        remainCostText.text = $"{playerPartyCost}";
    }
    

    //まずぬかなあかんのか
    public void panelImageShuffle()
    {
        //これ仮置きね
        costList.MakeEmptyCostCostList_p();

        //これ違う！！！コストの方だ！！！
        playerPartyCost = 15;
        choiceMonsters.Clear();
        displayMonsterBaseLists.Clear();
        //comの選んだものを排除　要はcomの選んだものが選定から外れれば良い
        for (var i=0; i<3; i++)
        {
            playerCostListList[ComParty[i].Cost - 1].Remove(ComParty[i]);
        }

        for (var i=0; i < 10; i++)
        {

            //List<MonsterBase> costList_here = costListList[i];
            //print(costListList[i].List.Count);

            
            int r = Random.Range(0, playerCostListList[i].Count);
            
            var costFronts = costPanels_content.transform.GetChild(i).GetChild(0);
            Image costImage = costFronts.GetChild(2).GetChild(0).GetComponent<Image>();
            Text costName = costFronts.GetChild(1).GetChild(0).GetComponent<Text>();
            Transform chTypeTextAreasParent = costFronts.GetChild(3);
            List<GameObject> chTypeTextAreas = new();
            chTypeTextAreas.Add(chTypeTextAreasParent.GetChild(0).gameObject);
            chTypeTextAreas.Add(chTypeTextAreasParent.GetChild(1).gameObject);

             battleHud.SetType(playerCostListList[i][r],chTypeTextAreas);
            

            costImage.sprite = playerCostListList[i][r].TheSprite;
            costName.text    = playerCostListList[i][r].Name;
            
            displayMonsterBaseLists.Add(playerCostListList[i][r]);

            SwipeCards swipeCard_i = costPanels_content.GetChild(i).GetComponent<SwipeCards>();
            swipeCard_i.monster_here = playerCostListList[i][r];


        }

        for(var i=0; i<10; i++)
        {
            print("here");
            print(costPanels_content.GetChild(i).GetComponent<SwipeCards>().monster_here);
        }
        //print(modelCostListList[1].List.Count);
    }

    public List<MonsterBase> ChoiceMonsters
    {
        get { return choiceMonsters; }
    }

    public void ComChoiceMonsters()
    {
        int rCostMaxInt;
        comPartyCost = 15;
        comParty.Clear();
        battleSystem.comHPList.Clear();
        costList.MakeEmptyCostCostList_c();

        for (var i=0; i<3; i++)
        { 
            if(comPartyCost > 10)
            {
                rCostMaxInt = 10;
            }
            else if (i == 2)//ここはcomPartyCost　> 10をスルーしているので自然と10以下を達成する
            {
                rCostMaxInt = comPartyCost;
            }
            else
            {
                rCostMaxInt = comPartyCost - 1;

                
            }
            int rCost = Random.Range(0, rCostMaxInt);
            int rMonster = Random.Range(0, comCostListList[rCost].Count);
            comParty.Add(comCostListList[rCost][rMonster]);
            comPartyCost -= (rCost + 1);

            battleSystem.comHPList.Add(new Pokemon(comParty[i]).HP);

            //これで初戦comPartyの不具合は除けた説
            comCostListList[rCost].RemoveAt(rMonster);
        }

        //3回目でかつcompartycostが10イカならrCostMaxInt = comPartyCost

        print(comParty[0]);
        print(comParty[1]);
        print(comParty[2]);



    }

    public void chooseComParty_forAgain()
    {
        costList.MakeEmptyCostCostList_c();


        for (var i = 0; i < 3; i++)
        {
            comCostListList[ComParty[i].Cost - 1].Remove(ComParty[i]);
            comCostListList[choiceMonsters[i].Cost - 1].Remove(choiceMonsters[i]);
        }
        comPartyCost = 15;
        newComParty.Clear();
        int rCostMaxInt;
        for (var i = 0; i < 3; i++)
        {
            if (comPartyCost > 10)
            {
                rCostMaxInt = 10;
            }
            else if (i == 2)//ここはcomPartyCost > 10をスルーしているので自然と10以下を達成する
            {
                rCostMaxInt = comPartyCost;
            }
            else
            {
                rCostMaxInt = comPartyCost - 1;


            }
            int rCost = Random.Range(0, rCostMaxInt);
            int rMonster = Random.Range(0, comCostListList[rCost].Count);

            newComParty.Add(comCostListList[rCost][rMonster]);
            comPartyCost -= (rCost + 1);

            //ここで既にComParty = newComPartyが聞いている
            //ということは
            print(ComParty[i]);
            print(newComParty[i]);

        }
        //ここで既にもうcomPartyが変化してる

        print($"comParty[0]は{comParty[0]}");
        print($"comParty[1]は{comParty[1]}");
        print($"comParty[2]は{comParty[2]}");
        print($"newComParty[0]は{newComParty[0]}");
        print($"newComParty[1]は{newComParty[1]}");
        print($"newComParty[2]は{newComParty[2]}");

    }

    public List<MonsterBase> ComParty
    {
        get { return comParty; }
    }

    private void RemoveFromParty(int num)
    {
        if(num < choiceMonsters.Count)
        {
            print(choiceMonsters[num]);
            int prePlayerPartyCost = playerPartyCost;
            playerPartyCost += choiceMonsters[num].Cost;
            remainCostText.text = $"{playerPartyCost}";
            //costpanels[choiceMonsters[num].Cost - 1].transform.GetChild(2).gameObject.SetActive(true);

            float toukaNum = 1f;
            float toukaTime = 0.3f;
            Transform costpanel_here = costPanels_content.GetChild(choiceMonsters[num].Cost - 1);
            Transform costfront_here = costPanels_content.GetChild(choiceMonsters[num].Cost - 1).GetChild(0);


            //コストコーン
            DOTween.ToAlpha(
            () => costpanel_here.GetChild(1).GetComponent<Image>().color,
            color => costpanel_here.GetChild(1).GetComponent<Image>().color = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costpanel_here.GetChild(1).GetChild(0).GetComponent<Text>().color,
            color => costpanel_here.GetChild(1).GetChild(0).GetComponent<Text>().color = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costpanel_here.GetChild(1).GetChild(0).GetComponent<Outline>().effectColor,
            color => costpanel_here.GetChild(1).GetChild(0).GetComponent<Outline>().effectColor = color,
            toukaNum,
            toukaTime
            );

            costfront_here.GetComponent<Image>().enabled = true;
            costfront_here.GetChild(0).gameObject.SetActive(false);

            DOTween.ToAlpha(
            () => costfront_here.GetComponent<Image>().color,
            color => costfront_here.GetComponent<Image>().color = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costfront_here.GetChild(1).GetChild(0).GetComponent<Text>().color,
            color => costfront_here.GetChild(1).GetChild(0).GetComponent<Text>().color = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costfront_here.GetChild(1).GetChild(0).GetComponent<Outline>().effectColor,
            color => costfront_here.GetChild(1).GetChild(0).GetComponent<Outline>().effectColor = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costfront_here.GetChild(2).GetChild(0).GetComponent<Image>().color,
            color => costfront_here.GetChild(2).GetChild(0).GetComponent<Image>().color = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costfront_here.GetChild(3).GetChild(0).GetComponent<Image>().color,
            color => costfront_here.GetChild(3).GetChild(0).GetComponent<Image>().color = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costfront_here.GetChild(3).GetChild(1).GetComponent<Image>().color,
            color => costfront_here.GetChild(3).GetChild(1).GetComponent<Image>().color = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costfront_here.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().color,
            color => costfront_here.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().color = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costfront_here.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().color,
            color => costfront_here.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().color = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costfront_here.GetChild(3).GetChild(0).GetChild(0).GetComponent<Outline>().effectColor,
            color => costfront_here.GetChild(3).GetChild(0).GetChild(0).GetComponent<Outline>().effectColor = color,
            toukaNum,
            toukaTime
            );

            DOTween.ToAlpha(
            () => costfront_here.GetChild(3).GetChild(1).GetChild(0).GetComponent<Outline>().effectColor,
            color => costfront_here.GetChild(3).GetChild(1).GetChild(0).GetComponent<Outline>().effectColor = color,
            toukaNum,
            toukaTime
            );




            choiceMonsters.RemoveAt(num);
            

            for (var i = 0; i < 3; i++)
            {
                if (i >= choiceMonsters.Count)
                {
                    //choicedPanels[i].gameObject.SetActive(false);
                    //choicedPanels[i].transform.GetChild(1).gameObject.SetActive(true);
                    //choicedPanels[i].transform.GetChild(0).gameObject.SetActive(false);

                    //choicedPanels[i].GetComponent<Image>().enabled = false;
                    //choicedPanels[i].transform.GetChild(1).gameObject.SetActive(true);
                    choicedPanels[i].transform.GetChild(1).gameObject.SetActive(  false);


                }
                else
                {

                    //choicedPanels[i].transform.GetChild(0).GetComponent<Image>().sprite = choiceMonsters[i].TheSprite;
                    //choicedPanels[i].transform.GetChild(0).gameObject.SetActive(true);
                    //choicedPanels[i].transform.GetChild(1).gameObject.SetActive(false);

                    choicedPanels[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = choiceMonsters[i].TheSprite;
                    //choicedPanels[i].GetComponent<Image>().enabled = true;

                    //choicedPanels[i].transform.GetChild(0).gameObject.SetActive(true);
                    choicedPanels[i].transform.GetChild(1).gameObject.SetActive( true);


                }
            }

            if (choiceMonsters.Count < 3)
            {


                DOTween.ToAlpha(
                () => goBattleButton.transform.GetChild(0).GetComponent<Image>().color,
                color => goBattleButton.GetComponent<Image>().color = color,
                0.08f,
                0.25f
                );

                DOTween.ToAlpha(
                () => goBattleButton.transform.GetChild(1).GetComponent<Image>().color,
                color => goBattleButton.transform.GetChild(1).GetComponent<Image>().color = color,
                0.22f,
                0.25f
                );

                DOTween.ToAlpha(
                () => goBattleButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().color,
                color => goBattleButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = color,
                0.22f,
                0.25f
                );

                DOTween.ToAlpha(
                () => goBattleButton.transform.GetChild(1).GetChild(1).GetComponent<Image>().color,
                color => goBattleButton.transform.GetChild(1).GetChild(1).GetComponent<Image>().color = color,
                0.22f,
                0.25f
                );

            }

            for (var i = prePlayerPartyCost; i < playerPartyCost; i++)
            {

                var index_here = i;
                DOTween.ToAlpha(
                () => costCones.transform.GetChild(index_here).GetChild(0).GetComponent<Image>().color,
                color => costCones.transform.GetChild(index_here).GetChild(0).GetComponent<Image>().color = color,
                1f,
                0.4f
                );
            }
        }

        

    }

    
}


