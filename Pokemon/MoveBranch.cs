using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBranch : MonoBehaviour
{
    //基本的にtakeDamageで呼ぶはずだから
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    [System.NonSerialized] public List<bool> playerTypeChangeList = new List<bool> { false,false,false};
    [System.NonSerialized] public List<bool> enemyTypeChangeList = new List<bool> { false, false, false };

    bool preTurnShield = false;


    public void moveBool(string moveName, BattleUnit battleUnit)
    {
        int TurnNum = battleSystem.TurnNum;
        List<List<string>> attackerChoiceRecord;
        List<bool> attackerTypeChangeList;
        int attackerFlontIndex;
        Move defenderMove;
        Pokemon attacker;
        Dictionary<string, bool> moveBoolDic;


        if (battleUnit == playerUnit)
        {
            attackerFlontIndex = battleSystem.playerFlontIndex;
            attackerChoiceRecord = battleSystem.playerChoiceRecord;
            attackerTypeChangeList = playerTypeChangeList;
            defenderMove = battleSystem.comMove;
            attacker = playerUnit.Pokemon;
            moveBoolDic = playerMoveBoolDic;
        }
        else
        {
            attackerFlontIndex = battleSystem.comFlontIndex;
            attackerChoiceRecord = battleSystem.enemyChoiceRecord;
            attackerTypeChangeList = enemyTypeChangeList;
            defenderMove = battleSystem.playerMove;
            attacker = enemyUnit.Pokemon;
            moveBoolDic = enemyMoveBoolDic;


        }

        float attackerType = newTypeChart.GetEffectiveness(defenderMove.Base.Type, attacker.Base.Type1, battleSystem.isTypeChange) *
            newTypeChart.GetEffectiveness(defenderMove.Base.Type, attacker.Base.Type2, battleSystem.isTypeChange);

        List<string> keyList = new List<string>(moveDialogDic.Keys);

        switch (moveName)
        {
            case "シールド":
                if(TurnNum >= 2)
                {
                    if (attackerChoiceRecord[TurnNum - 2][0] == attackerChoiceRecord[TurnNum - 1][0] &&
                    attackerChoiceRecord[TurnNum - 2][1] == "シールド" && attackerChoiceRecord[TurnNum - 1][1] == "シールド")
                    {
                        if(!preTurnShield)
                        {
                            preTurnShield = true;
                            foreach (string key in keyList)
                            {
                                if (moveName == key)
                                    moveBoolDic[key] = true;
                                else
                                    moveBoolDic[key] = false;
                            }
                        }
                        else
                        {
                            preTurnShield = false;
                            foreach (string key in keyList)
                            {
                                
                                moveBoolDic[key] = false;
                            }
                        }

                        

                       

                    }
                    else
                    {
                        preTurnShield = true;
                        foreach (string key in keyList)
                        {
                            if(moveName == key)
                                moveBoolDic[key] = true;
                            else
                                moveBoolDic[key] = false;
                        }

                    }

                }
                else
                {
                    preTurnShield = true;
                    foreach (string key in keyList)
                    {
                        if (moveName == key)
                            moveBoolDic[key] = true;
                        else
                            moveBoolDic[key] = false;
                    }
                }
                break;

            case "ブラックホール":
                if(attackerType >= 2f)
                {
                    foreach (string key in keyList)
                    {
                        if (moveName == key)
                            moveBoolDic[key] = true;
                        else
                            moveBoolDic[key] = false;
                    }
                }
                else
                {
                    foreach (string key in keyList)
                    {
                        moveBoolDic[key] = false;
                    }
                }
                break;

            case "チェンジ":
                if (attackerTypeChangeList[attackerFlontIndex])
                {
                    foreach (string key in keyList)
                    {
                        moveBoolDic[key] = false;
                    }
                }
                else
                {
                    attackerTypeChangeList[attackerFlontIndex] = true;
                    foreach (string key in keyList)
                    {
                        if (moveName == key)
                            moveBoolDic[key] = true;
                        else
                            moveBoolDic[key] = false;
                    }
                }
                break;
            default:
                foreach (string key in keyList)
                {
                    moveBoolDic[key] = false;
                }
                break;
        }
    }
    //falseだったらメッセージを返すだったら,チェンジの成功の場合　falseにしないといけないのでは　
    Dictionary<string, string> moveDialogDic = new Dictionary<string, string>()
    {
        {"シールド", "シールドは失敗した"},
        {"ブラックホール", "しかし抜群ではない"},
        {"チェンジ", "タイプが逆転した"}
    };

    



    public Dictionary<string, bool> playerMoveBoolDic = new Dictionary<string, bool>()
    {
        {"シールド", false},
        {"ブラックホール", false},
        {"チェンジ", false}
    };

    public Dictionary<string, bool> enemyMoveBoolDic = new Dictionary<string, bool>()
    {
        {"シールド", false},
        {"ブラックホール", false},
        {"チェンジ", false}
    };

    public IEnumerator moveDatails(string moveName, Dictionary<string, bool> moveBoolDic)
    {
        bool yes = false;
        foreach(string key in moveBoolDic.Keys)
        {
            if (key == moveName)
                yes = true;
        }

        if (yes)
        {
            print($"moveBoolDic[チェンジ]{moveBoolDic["チェンジ"]}");
            if (moveName == "チェンジ")
            {
                print($"moveBoolDic[チェンジ]{moveBoolDic["チェンジ"]}");
                if (moveBoolDic["チェンジ"])
                {

                    yield return dialogBox.TypeDialog(moveDialogDic[moveName]);
                    yield return new WaitForSeconds(0.3f);
                }

            }
            else if (!moveBoolDic[moveName])
            {
                yield return dialogBox.TypeDialog(moveDialogDic[moveName]);
                yield return new WaitForSeconds(0.3f);
            }

        }
        


    }

}


