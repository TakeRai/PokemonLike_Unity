using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class Ailments : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] MoveEffect playersideMoveEffect;
    [SerializeField] MoveEffect enemysideMoveEffect;
    [SerializeField] GameController gameController;
    [SerializeField] CanvasGroup playerAilmentArea;
    [SerializeField] CanvasGroup enemyAilmentArea;

    [SerializeField] List<Image> ailmentArea_Party;

    public List<AilmentBase> ailmentList;

    //AilListは現在の状態を示すだけね
    [System.NonSerialized] public int playerBurstCount;
    [System.NonSerialized] public int enemyBurstCount;

    [System.NonSerialized] public List<AilmentBase> playerAilList;
    [System.NonSerialized] public List<AilmentBase> enemyAilList;
    [System.NonSerialized] public List<List<float>> playerAilStatusList = new List<List<float>>
    {
        new List<float>{1,1,1,1,1},new List<float>{1,1,1,1,1},new List<float>{1,1,1,1,1},
    };

    [System.NonSerialized] public List<List<float>> enemyAilStatusList = new List<List<float>>
    {
        new List<float>{1,1,1,1,1},new List<float>{1,1,1,1,1},new List<float>{1,1,1,1,1},
    };

    List<List<string>> playerAilCount = new List<List<string>>
    {
        new List<string>(),new List<string>(),new List<string>()
    };

    List<List<string>> enemyAilCount = new List<List<string>>
    {
        new List<string>(),new List<string>(),new List<string>()
    };

    List<int> playerAilTurn = new List<int>{0,0,0};
    List<int> enemyAilTurn = new List<int> { 0, 0, 0 };

    void Start()
    {
        playerAilList = new List<AilmentBase> { null, null, null };
        enemyAilList = new List<AilmentBase> { null, null, null };

        
        FlashAilArea(playerAilmentArea);
        FlashAilArea(enemyAilmentArea);
    }

    void FlashAilArea(CanvasGroup ailArea)
    {
        CanvasGroup canvasGroup = ailArea.GetComponent<CanvasGroup>();

        //if(ailArea.gameObject.activeSelf)
        canvasGroup.DOFade(0.0f, 1.5f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);

    }
    
    //addしてく形式の方がいいよな

    public IEnumerator AilmentStateChange(Move move,BattleUnit battleUnit)
    {


        if(move.Base.ailmentBase != null)
        {
            if(battleUnit == battleSystem.playerUnit)
            {
                if(enemyAilList[battleSystem.comFlontIndex] == null)
                {
                    int resistAil = enemyAilCount[battleSystem.comFlontIndex].Count(name => name == move.Base.ailmentBase.Name);
                    print($"enemyresistAil{resistAil}");
                    if (resistAil < move.Base.ailmentBase.recoveryTurn)
                    {
                        enemyAilTurn[battleSystem.comFlontIndex] = move.Base.ailmentBase.recoveryTurn - resistAil;
                        enemyAilList[battleSystem.comFlontIndex] = move.Base.ailmentBase;
                        enemyAilCount[battleSystem.comFlontIndex].Add(move.Base.ailmentBase.Name);
                        AilStatusChange(false);
                        yield return dialogBox.TypeDialog($"{battleSystem.enemyUnit.Pokemon.Base.Name}は{move.Base.ailmentBase.Name}になった");
                        SetAilArea(false);
                    }
                    else
                    {
                        yield return dialogBox.TypeDialog($"{battleSystem.enemyUnit.Pokemon.Base.Name}は{move.Base.ailmentBase.Name}にならない");
                    }

                }
                else
                {
                    yield return dialogBox.TypeDialog($"{battleSystem.enemyUnit.Pokemon.Base.Name}はもう{enemyAilList[battleSystem.comFlontIndex].Name}だ");
                }
            }
            else
            {
                
                if (playerAilList[battleSystem.playerFlontIndex] == null)
                {

                    int resistAil = playerAilCount[battleSystem.playerFlontIndex].Count(name => name == move.Base.ailmentBase.Name);
                    print($"playerresistAil{resistAil}");
                    if(resistAil < move.Base.ailmentBase.recoveryTurn)
                    {
                        playerAilList[battleSystem.playerFlontIndex] = move.Base.ailmentBase;
                        playerAilCount[battleSystem.playerFlontIndex].Add(move.Base.ailmentBase.Name);
                        playerAilTurn[battleSystem.playerFlontIndex] = move.Base.ailmentBase.recoveryTurn - resistAil;
                        AilStatusChange(true);
                        yield return dialogBox.TypeDialog($"{battleSystem.playerUnit.Pokemon.Base.Name}は{move.Base.ailmentBase.Name}になった");
                        SetAilArea(true);

                    }
                    else
                    {
                        yield return dialogBox.TypeDialog($"{battleSystem.playerUnit.Pokemon.Base.Name}は{move.Base.ailmentBase.Name}にならない");
                    }
                }
                else
                {
                    yield return dialogBox.TypeDialog($"{battleSystem.playerUnit.Pokemon.Base.Name}はもう{playerAilList[battleSystem.playerFlontIndex].Name}だ");
                }
            }
        }
        
    }

    public IEnumerator TurnEndAilments()
    {
        Pokemon playerMonster = battleSystem.playerUnit.Pokemon;
        Pokemon enemyMonster = battleSystem.enemyUnit.Pokemon;

        float playerSpeed = playerMonster.Speed * battleSystem.PlayerSpeedRankStatus;
        float enemySpeed = enemyMonster.Speed * battleSystem.EnemySpeedRankStatus;
        //あかんこのままじゃ共倒れ処理できてない
        if (playerSpeed > enemySpeed)
        {
            yield return HpAndDialog(battleSystem.playerUnit);
            yield return HpAndDialog(battleSystem.enemyUnit);
            yield return AfterAilment(battleSystem.playerUnit);
            yield return AfterAilment(battleSystem.enemyUnit);

        }
        else if(playerSpeed == enemySpeed)
        {
            int r = Random.Range(0, 2);
            if (r == 0)
            {
                yield return HpAndDialog(battleSystem.playerUnit);
                yield return HpAndDialog(battleSystem.enemyUnit);
                yield return AfterAilment(battleSystem.playerUnit);
                yield return AfterAilment(battleSystem.enemyUnit);

            }
            else
            {
                yield return HpAndDialog(battleSystem.enemyUnit);
                yield return HpAndDialog(battleSystem.playerUnit);
                yield return AfterAilment(battleSystem.enemyUnit);
                yield return AfterAilment(battleSystem.playerUnit);

            }
        }
        else
        {
            yield return HpAndDialog(battleSystem.enemyUnit);
            yield return HpAndDialog(battleSystem.playerUnit);
            yield return AfterAilment(battleSystem.enemyUnit);
            yield return AfterAilment(battleSystem.playerUnit);

        }

        //ここがHPの処理
        IEnumerator HpAndDialog(BattleUnit battleUnit)
        {
            AilmentBase ailment = null;
            BattleHud battleHud;
            MoveEffect moveEffect;
            int flontIndex;
            List<int> HPList;
            int burstCount = 0 ;

            if(battleUnit == battleSystem.playerUnit )
            {
                battleHud = playerHud;
                //もしや例えばmoveEffectはplayersideMoveEffectは値を参照するだけでplayersideMoveEffectではないつまりmoveEffectを
                //変えてもplayersideMoveEffectは変わらない説
                moveEffect = playersideMoveEffect;
                flontIndex = battleSystem.playerFlontIndex;
                HPList = battleSystem.playerHPList;
                
                if(playerAilList[battleSystem.playerFlontIndex] != null)
                {
                    ailment = playerAilList[battleSystem.playerFlontIndex];
                    if (ailment.Name == "炎上")
                    {
                        
                        burstCount = playerBurstCount;
                    }
                    
                        
                }

            } else
            {
                battleHud = enemyHud;
                moveEffect = enemysideMoveEffect;
                flontIndex = battleSystem.comFlontIndex;
                HPList = battleSystem.comHPList;
                if (enemyAilList[battleSystem.comFlontIndex] != null)
                {
                    ailment = enemyAilList[battleSystem.comFlontIndex];
                    if (ailment.Name == "炎上")
                    {
                        print("good");
                        burstCount = enemyBurstCount;
                    }
                    


                }

            }

            if (ailment != null)
            {
                yield return dialogBox.TypeDialog($"{battleUnit.Pokemon.Base.Name}は{ailment.Name}のダメージを受けた");
                //HPListの処理ってここで書く必要ないっけ？？？
                //burstCountの処理はターン終了時にダメージくらってからでいいや
                


                float constantdamage_here = ailment.constantDamage;
                print($"burstCount{burstCount}");
                switch (burstCount)
                {
                    case 1:
                        constantdamage_here = 0.167f;
                        break;
                    case 2:
                        constantdamage_here = 0.25f;
                        break;
                    case 3:
                        constantdamage_here = 0.25f;
                        break;
                    default:
                        break;

                }

                battleUnit.Pokemon.HP -= Mathf.FloorToInt(constantdamage_here * battleUnit.Pokemon.MaxHp);

                if (ailment.Name == "炎上")
                {
                    if(battleUnit == battleSystem.playerUnit)
                        playerBurstCount++;
                    else
                        enemyBurstCount++;
                }


                if (battleUnit.Pokemon.HP < 0)
                {
                    battleUnit.Pokemon.HP = 0;
                }else if(battleUnit.Pokemon.HP > battleUnit.Pokemon.MaxHp)
                {
                    battleUnit.Pokemon.HP = battleUnit.Pokemon.MaxHp;
                }

                HPList[flontIndex] = battleUnit.Pokemon.HP;

                moveEffect.playMoveEffect(ailment.Name);
                soundManager.PlaySingle(ailment.moveSe);
                yield return battleHud.UpdateHP(false) ;
                yield return new WaitForSeconds(1f);

                //dead処理は二者のailment終わりでいいか
                

            }

            
            
        }

        //
        IEnumerator AfterAilment(BattleUnit battleUnit)
        {
            if (battleUnit.Pokemon.HP <= 0)
            {
                if (battleUnit == battleSystem.playerUnit)
                {
                    yield return dialogBox.TypeDialog($"{ battleUnit.Pokemon.Base.Name} は倒れた");
                    battleUnit.PlayFaintAnimation();
                    yield return new WaitForSeconds(1f);

                    dialogBox.EnableDialogText(false);
                    if (battleSystem.playerHPList.All(i => i <= 0))
                    {

                        dialogBox.EnableAfterBattleSelector(true);
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        gameController.GoParty(isDead: true);
                        yield return new WaitForSeconds(1f);
                    }
                }
                else
                {
                    battleUnit.PlayFaintAnimation();
                    yield return new WaitForSeconds(1f);

                    if (battleSystem.comHPList.All(i => i <= 0))
                    {
                        battleSystem.wincount++;
                        yield return dialogBox.TypeDialog($"{battleSystem.enemyUnit.Pokemon.Base.Name} は倒れた 勝利数は {battleSystem.wincount}");

                        dialogBox.EnableDialogText(false);
                        dialogBox.EnableAfterBattleSelector(true);
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        yield return dialogBox.TypeDialog($"{battleSystem.enemyUnit.Pokemon.Base.Name} は倒れた");
                        dialogBox.EnableDialogText(false);
                        StartCoroutine(battleSystem.changeComparty_dead());
                        yield return new WaitForSeconds(1f);
                    }
                }
            }
        }
    }

    void AilStatusChange(bool isPlayer)
    {
        List<AilmentBase> ailList;
        List<List<float>> ailStatusList;
        int flontIndex;

        if (isPlayer)
        {
            flontIndex = battleSystem.playerFlontIndex;
            ailStatusList = playerAilStatusList;
            ailList = playerAilList;
        }
        else
        {
            flontIndex = battleSystem.comFlontIndex;
            ailStatusList = enemyAilStatusList;
            ailList = enemyAilList;

        }
        if(ailList[flontIndex] != null)
        {
            ailStatusList[flontIndex][0] = ailList[flontIndex].ailAtt;
            ailStatusList[flontIndex][1] = ailList[flontIndex].ailDef;
            ailStatusList[flontIndex][2] = ailList[flontIndex].ailSat;
            ailStatusList[flontIndex][3] = ailList[flontIndex].ailSde;
            ailStatusList[flontIndex][4] = ailList[flontIndex].ailSpe;

        }
        else
        {
            for(var i=0; i< 5; i++)
            {
                ailStatusList[flontIndex][i] = 1;
            }
        }




        //直接　ailBaseからstatus補正を参照する説
    }

    //このまま足すと眠りはめんどくさいことになる
    public IEnumerator AilTurnProcess(BattleUnit battleUnit)
    {
        //ここで発動しないということはturnが0以下か
        print($"1enemyAilTurn{enemyAilTurn[battleSystem.comFlontIndex]}"); 
        if (battleUnit == battleSystem.playerUnit　&& playerAilTurn[battleSystem.playerFlontIndex] > 0)
        {
            print("ターン数");
            playerAilTurn[battleSystem.playerFlontIndex] -= 1;

            if(playerAilTurn[battleSystem.playerFlontIndex] <= 0)
            {
                yield return dialogBox.TypeDialog($"{battleUnit.Pokemon.Base.Name}の{playerAilList[battleSystem.playerFlontIndex].Name}が治った");
                playerAilList[battleSystem.playerFlontIndex] = null;
                playerBurstCount = 0;
                AilStatusChange(true);
                SetAilArea(true);
            }

        }
        else if(battleUnit == battleSystem.enemyUnit && enemyAilTurn[battleSystem.comFlontIndex] > 0)
        {

            print($"2enemyAilTurn{enemyAilTurn[battleSystem.comFlontIndex]}");
            enemyAilTurn[battleSystem.comFlontIndex] -= 1;
            //とりあえずailのturn処理後、メッセー二の表記とあと耐性によるターン数決定
            if (enemyAilTurn[battleSystem.comFlontIndex] <= 0)
            {
                yield return dialogBox.TypeDialog($"{battleUnit.Pokemon.Base.Name}の{enemyAilList[battleSystem.comFlontIndex].Name}が治った");
                enemyAilList[battleSystem.comFlontIndex] = null;
                enemyBurstCount = 0;
                AilStatusChange(false);
                SetAilArea(false);
            }
        }
        else
        {
            print("失敗");
        }
    }

    public void AilReset()
    {
        playerAilList = new List<AilmentBase> { null, null, null };
        enemyAilList = new List<AilmentBase> { null, null, null };

        playerAilStatusList = new List<List<float>>
        {
            new List<float>{1,1,1,1,1},new List<float>{1,1,1,1,1},new List<float>{1,1,1,1,1},
        };

        enemyAilStatusList = new List<List<float>>
        {
            new List<float>{1,1,1,1,1},new List<float>{1,1,1,1,1},new List<float>{1,1,1,1,1},
        };

        playerAilCount = new List<List<string>>
        {
            new List<string>(),new List<string>(),new List<string>()
        };

        enemyAilCount = new List<List<string>>
        {
            new List<string>(),new List<string>(),new List<string>()
        };

        playerAilTurn = new List<int> { 0, 0, 0 };
        enemyAilTurn = new List<int> { 0, 0, 0 };
        playerBurstCount = 0;
        enemyBurstCount = 0 ;

        playerAilmentArea.gameObject.SetActive(false);
        enemyAilmentArea.gameObject.SetActive(false);
    }

    public void SetAilArea(bool isPlayer)
    {

        string colorString;
        Color newColor;
        CanvasGroup ailmentArea;
        AilmentBase ail;

        
       

        

        if (isPlayer)
        {
            ailmentArea = playerAilmentArea;
            ail = playerAilList[battleSystem.playerFlontIndex];
        }
        else
        {
            ailmentArea = enemyAilmentArea;
            ail = enemyAilList[battleSystem.comFlontIndex];
        }

        if(ail != null)
        {
            ailmentArea.gameObject.SetActive(true);
            switch (ail.Name)
            {

                case "なまけ":
                    colorString = "#FFC5AB";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    ailmentArea.GetComponent<Image>().color = newColor;
                    ailmentArea.transform.GetChild(0).GetComponent<Text>().text = ail.Name;
                    ailmentArea.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                    break;

                case "炎上":
                    colorString = "#FF4F00";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    ailmentArea.GetComponent<Image>().color = newColor;
                    ailmentArea.transform.GetChild(0).GetComponent<Text>().text = ail.Name;
                    ailmentArea.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                    break;

                case "凶暴":
                    colorString = "#19FFE5";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    ailmentArea.GetComponent<Image>().color = newColor;
                    ailmentArea.transform.GetChild(0).GetComponent<Text>().text = ail.Name;
                    ailmentArea.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                    break;

                case "呪われ":
                    colorString = "#FFFF00";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    ailmentArea.GetComponent<Image>().color = newColor;
                    ailmentArea.transform.GetChild(0).GetComponent<Text>().text = ail.Name;
                    ailmentArea.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                    break;

                default:
                    break;
            }

        }
        else
        {
            ailmentArea.gameObject.SetActive(false);
        }
    }

    public void SetAilArea_Party()
    {

        string colorString;
        Color newColor;
        Image ailmentArea;
        AilmentBase ail;

        void Here()
        {
            ColorUtility.TryParseHtmlString(colorString, out newColor);
            ailmentArea.GetComponent<Image>().color = newColor;
            ailmentArea.transform.GetChild(0).GetComponent<Text>().text = ail.Name;

        }

        for (var i=0; i<3; i++)
        {
            ailmentArea = ailmentArea_Party[i];
            ail = playerAilList[i];

            if (ail != null)
            {
                ailmentArea.gameObject.SetActive(true);
                switch (ail.Name)
                {

                    case "なまけ":
                        colorString = "#FFC5AB";
                        Here();
                        break;

                    case "炎上":
                        colorString = "#FF4F00";
                        Here();
                        break;

                    case "凶暴":
                        colorString = "#19FFE5";
                        Here();
                        break;

                    case "呪われ":
                        colorString = "#FFFF00";
                        Here();
                        break;

                    default:
                        break;
                }

            }
            else
            {
                ailmentArea.gameObject.SetActive(false);
            }
        }

    }

    


}
