using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class costListList : MonoBehaviour
{
    [SerializeField] ChoiceScreen choiceScreen;
    public List<MonsterBase> cost1List;
    public List<MonsterBase> cost2List;
    public List<MonsterBase> cost3List;
    public List<MonsterBase> cost4List;
    public List<MonsterBase> cost5List;
    public List<MonsterBase> cost6List;
    public List<MonsterBase> cost7List;
    public List<MonsterBase> cost8List;
    public List<MonsterBase> cost9List;
    public List<MonsterBase> cost10List;

    public void MakeEmptyCostCostList_p()
    {

        choiceScreen.playerCostListList = new List<List<MonsterBase>>();


        choiceScreen.playerCostListList.Add(new(cost1List));

        choiceScreen.playerCostListList.Add(new(cost2List));
        choiceScreen.playerCostListList.Add(new(cost3List));
        choiceScreen.playerCostListList.Add(new(cost4List));
        choiceScreen.playerCostListList.Add(new(cost5List));
        choiceScreen.playerCostListList.Add(new(cost6List));
        choiceScreen.playerCostListList.Add(new(cost7List));
        choiceScreen.playerCostListList.Add(new(cost8List));
        choiceScreen.playerCostListList.Add(new(cost9List));
        choiceScreen.playerCostListList.Add(new(cost10List));

    }

    public void MakeEmptyCostCostList_c()
    {

        choiceScreen.comCostListList = new List<List<MonsterBase>>();
        choiceScreen.comCostListList.Add(new(cost1List));
        choiceScreen.comCostListList.Add(new(cost2List));
        choiceScreen.comCostListList.Add(new(cost3List));
        choiceScreen.comCostListList.Add(new(cost4List));
        choiceScreen.comCostListList.Add(new(cost5List));
        choiceScreen.comCostListList.Add(new(cost6List));
        choiceScreen.comCostListList.Add(new(cost7List));
        choiceScreen.comCostListList.Add(new(cost8List));
        choiceScreen.comCostListList.Add(new(cost9List));
        choiceScreen.comCostListList.Add(new(cost10List));

    }


}
