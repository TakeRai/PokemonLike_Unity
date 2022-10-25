using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] ChoiceScreen choiceScreen;
    public BattleUnit playerUnit;
    public BattleUnit enemyUnit;
    [SerializeField] BattleHud PlayerHud;
    [SerializeField] BattleHud EnemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] MoveEffect playersideMoveEffect;
    [SerializeField] MoveEffect enemysideMoveEffect;
    [SerializeField] List< Button> moveButtons;
    [SerializeField] List<Button> buttonActions;
    [SerializeField] List<Button> AfterButtleButtons;
    [SerializeField] SoundManager soundManager;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] Text floorNumber;
    [SerializeField] MoveBranch moveBranch;
    public  Ailments ailments;
    [SerializeField] MoveBase zzz;

    

    public event Action<bool> onBattleOver;

    //BattleState state;

    int currentAction;
    [System.NonSerialized] public Move playerMove;
    int playerMoveIndex;
    [System.NonSerialized] public bool playerChange = false;
    [System.NonSerialized] public Move comMove;
    int comMoveIndex;
    [System.NonSerialized] public bool comChange;
    [System.NonSerialized] public int wincount = 0;
    bool playerFirst;
    //hp処理をなくして以下で統合しよう
    [System.NonSerialized] public List<int> playerHPList = new List<int>();
    [System.NonSerialized] public List<int> comHPList = new List<int>();
    [System.NonSerialized] public int playerFlontIndex = 0;
    [System.NonSerialized] public int comFlontIndex = 0;
    List<MonsterBase> surviveComParty = new List<MonsterBase>();

    [System.NonSerialized] public int TurnNum = 0;
    [System.NonSerialized] public List<List<string>> playerChoiceRecord = new List<List<string>>();
    [System.NonSerialized] public List<List<string>> enemyChoiceRecord = new List<List<string>>();
    [System.NonSerialized] public bool isTypeChange = false;
    

    

    private void Start()
    {
        for (var num = 0; num < moveButtons.Count; num++)
        {
            var count = num;
            moveButtons[count].onClick.AddListener(() => ButtonMoveClick(count));
        }

        for (var num = 0; num < buttonActions.Count; num++)
        {
            var count = num;
            buttonActions[count].onClick.AddListener(() => buttonActionClick(count));
        }

        for (var num = 0; num < AfterButtleButtons.Count; num++)
        {
            var count = num;
            AfterButtleButtons[count].onClick.AddListener(() => AfterBattleFunc(count));
        }


    }


    public void StartBattle()
    {
        ResetVariables();
        //StartCoroutine(SetUpBattle());
        SetUpBattle();
        
    }

    //public IEnumerator SetUpBattle()
    public void SetUpBattle()
    {
        playerUnit.SetUp();
        SetInfo(PlayerHud, playerUnit);
        //PlayerHud.SetData(playerUnit.Pokemon);
        enemyUnit.SetUp();
        EnemyHud.SetData(enemyUnit.Pokemon);

        //dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        //setMoveColors(playerUnit.Pokemon.Moves);

        dialogBox.EnableDialogText(false);
        //yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} が現れた");

        PlayerAction();
        comFlontIndex = 0;
    }

    public void SetInfo(BattleHud battleHud, BattleUnit battleUnit)
    {
        battleHud.SetData(battleUnit.Pokemon);
        dialogBox.SetMoveFronts(battleUnit.Pokemon.Moves,moveButtons);
        //setMoveColors(battleUnit.Pokemon.Moves,moveButtons);
    }

    void PlayerAction()
    {
        TurnNum += 1;
         //StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableDialogText(false);
        dialogBox.EnableActionSelector(true);
        playerChange = false;
        comChange = false;

        for(var i=0; i<5; i++)
        {
            print($"playerRankList[i]{playerRankList[i]}");
            print($"enemyRankList[i]{enemyRankList[i]}");
        }
    } 

    void PlayerMove() {
        //state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        //dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);

        
    }

    IEnumerator PerformPlayerMove()
    {
        //state = BattleState.Busy;
        DamageDetails damageDetails = new DamageDetails { Fainted = false, Shield = false, TypeEffectiveness = 0 };
        yield return ailments.AilTurnProcess(playerUnit);
        if (ailments.playerAilList[playerFlontIndex] != ailments.ailmentList[0])
        {
            playerMove.PP--;
            dialogBox.UpdatePP(playerMoveIndex, playerMove);
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} の {playerMove.Base.Name}");


            if (playerMove.Base.SelfSide == true)
            {
                playersideMoveEffect.playMoveEffect(playerMove.Base.Name);
            }
            else
            {
                enemysideMoveEffect.playMoveEffect(playerMove.Base.Name);
            }


            soundManager.PlaySingle(playerMove.Base.MoveSe);

            yield return new WaitForSeconds(1f);


            enemyUnit.PlayerHitAnimation();
            //MoveProcessで変化技ならそもそも技処理不要とかにしてしまえ
            damageDetails = enemyUnit.Pokemon.TakeDamage(playerUnit, this, moveBranch);
            yield return moveBranch.moveDatails(playerMove.Base.Name, moveBranch.playerMoveBoolDic);
            yield return EnemyHud.UpdateHP(playerMove.Base.IsSituation);
            //追加
            yield return PlayerHud.UpdateHP(playerMove.Base.IsSituation);
            yield return MoveProcess(playerUnit, playerMove, comMove);
            yield return ailments.AilmentStateChange(playerMove, playerUnit);
            yield return showDamageDetails(damageDetails);
            yield return new WaitForSeconds(0.3f);

        }
        else
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}はなまけて動けない");
            soundManager.PlaySingle(ailments.ailmentList[0].moveSe);
            playersideMoveEffect.playMoveEffect(ailments.ailmentList[0].Name);

            yield return new WaitForSeconds(1f);
        }
        
        
        

        if (damageDetails.Fainted)
        {
            enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(1f);


            if (comHPList.All(i => i <= 0))
            {
                wincount++;
                yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} は倒れた 勝利数は {wincount}");

                dialogBox.EnableDialogText(false);
                dialogBox.EnableAfterBattleSelector(true);
            }
            else
            {
                yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} は倒れた");
                dialogBox.EnableDialogText(false);
                StartCoroutine(changeComparty_dead());
            }
            
        }
        else
        {

            if (playerFirst == true)
            {
                StartCoroutine(EnemyMove(false));
            }
            else
            {
                //ここにターン終了時の状態異常処理
                yield return ailments.TurnEndAilments();

                PlayerAction();
            }
        }
    }

    public IEnumerator EnemyMove(bool isChange)
    {
        print("夜襲");
        //state = BattleState.EnemyMove;
        DamageDetails damageDetails = new DamageDetails { Fainted = false, Shield = false, TypeEffectiveness = 0 };
        yield return ailments.AilTurnProcess(enemyUnit);
        if (ailments.enemyAilList[comFlontIndex] != ailments.ailmentList[0])
        {
            comMove.PP--;
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} の {comMove.Base.Name}");

            //enemyUnit.PlayAttackAnimation();
            //yield return new WaitForSeconds(1f);

            if (comMove.Base.SelfSide == true)
            {
                enemysideMoveEffect.playMoveEffect(comMove.Base.Name);
            }
            else
            {
                playersideMoveEffect.playMoveEffect(comMove.Base.Name);
            }
            soundManager.PlaySingle(comMove.Base.MoveSe);
            yield return new WaitForSeconds(1f);


            playerUnit.PlayerHitAnimation();


            damageDetails = playerUnit.Pokemon.TakeDamage(enemyUnit, this, moveBranch);
            yield return moveBranch.moveDatails(comMove.Base.Name, moveBranch.enemyMoveBoolDic);
            yield return PlayerHud.UpdateHP(comMove.Base.IsSituation);
            //追加
            yield return EnemyHud.UpdateHP(comMove.Base.IsSituation);
            yield return MoveProcess(enemyUnit, comMove, playerMove);
            yield return ailments.AilmentStateChange(comMove, enemyUnit);
            yield return showDamageDetails(damageDetails);
            //倒れた時に表示でもいいか
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}はなまけて動けない");
            soundManager.PlaySingle(ailments.ailmentList[0].moveSe);
            enemysideMoveEffect.playMoveEffect(ailments.ailmentList[0].Name);
            yield return new WaitForSeconds(1f);
        }


        


        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} は倒れた");
            playerUnit.PlayFaintAnimation();


            yield return new WaitForSeconds(1f);

            //onBattleOver(false);

            dialogBox.EnableDialogText(false);
            if (playerHPList.All(i => i <= 0))
            {

                dialogBox.EnableAfterBattleSelector(true);
            }
            else
            {
                gameController.GoParty(isDead:true);
            }


        }
        else
        {
            //PlayerAction();

            if (playerFirst == true || isChange == true)
            {
                yield return ailments.TurnEndAilments();

                PlayerAction();
            }
            else
            {
                StartCoroutine(PerformPlayerMove());
            }
        }
    }

    //プレイヤー交換
    public IEnumerator changeFrontSystem_forPlayer(bool isDead)
    {
        if (!isDead)
        {
            DevideComMove();

            //List<string> MonsterAndMove = new List<string>();
            //MonsterAndMove.Add(choiceScreen.choiceMonsters[playerFlontIndex].Name);
            //MonsterAndMove.Add(playerMove.Base.Name);
            //playerChoiceRecord.Add(MonsterAndMove);


            //playerChoiceRecord
            playerMove = new Move(zzz);
        }

        ailments.playerBurstCount = 0;
        
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(true);

        for (var i = 0; i < playerRankList.Count; i++)
        {
            playerRankList[i] = 0;
        }

        yield return dialogBox.TypeDialog($"プレイヤーは交換した");
        partyScreen.changeParty();
        ailments.SetAilArea(true);
        yield return new WaitForSeconds(0.5f);
        if (isDead)
        {
            PlayerAction();
        }
        else
        {
            StartCoroutine(EnemyMove(true));
        }
        
    }

    //エネミー(Com)交換
    public IEnumerator changeComparty_dead()
    {
        //DevideComMove();
        //dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(true);
        for(var i=0; i< enemyRankList.Count; i++)
        {
            enemyRankList[i] = 0;
        }

        ailments.enemyBurstCount = 0;

        yield return dialogBox.TypeDialog($"comは繰り出した");
        SetUp_ForComparty();
        ailments.SetAilArea(false);

        yield return new WaitForSeconds(0.5f);
        PlayerAction();
    }

    IEnumerator showDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Shield)
            yield return dialogBox.TypeDialog("身を守った!");
        else if (damageDetails.TypeEffectiveness > 1f)
            yield return dialogBox.TypeDialog("効果は抜群だ!");
        else if (damageDetails.TypeEffectiveness < 1f)
            yield return dialogBox.TypeDialog("効果半減");

    }



    private void ButtonMoveClick(int num)
    {
        List<string> MonsterAndMove = new List<string>();
        //これ不要かも
        //MonsterAndMove.Clear();

        //技選択時にしかcomMoveが決まらないのでplayer交換時にはエラー
        playerMoveIndex = num;

        //これplayerMove消してRecordにすべきかな
        playerMove = playerUnit.Pokemon.Moves[playerMoveIndex];

        MonsterAndMove.Add(choiceScreen.choiceMonsters[playerFlontIndex].Name);
        MonsterAndMove.Add(playerMove.Base.Name);
        playerChoiceRecord.Add(MonsterAndMove);
        for(var i=0; i < playerChoiceRecord.Count; i++)
        {
            print(playerChoiceRecord[i][0]);
            print(playerChoiceRecord[i][1]);
        }

        DevideComMove();
        print($"comMoveは{comMove}");

        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);

        
        moveBranch.moveBool(playerMove.Base.Name, playerUnit);
        moveBranch.moveBool(comMove.Base.Name, enemyUnit);


        if (playerMove.Base.Priority > comMove.Base.Priority)
        {
            playerFirst = true;
            StartCoroutine(PerformPlayerMove());
        }
        else if (playerMove.Base.Priority < comMove.Base.Priority)
        {
            playerFirst = false;
            StartCoroutine(EnemyMove(false));
        }
        else if (playerUnit.Pokemon.Speed * PlayerSpeedRankStatus * ailments.playerAilStatusList[playerFlontIndex][4] >
            enemyUnit.Pokemon.Speed * EnemySpeedRankStatus * ailments.enemyAilStatusList[comFlontIndex][4])
        {
            playerFirst = true;
            StartCoroutine(PerformPlayerMove());
        }
        else if (playerUnit.Pokemon.Speed * PlayerSpeedRankStatus * ailments.playerAilStatusList[playerFlontIndex][4] <
            enemyUnit.Pokemon.Speed * EnemySpeedRankStatus * ailments.enemyAilStatusList[comFlontIndex][4])
        {
            playerFirst = false;
            StartCoroutine(EnemyMove(false));
        }
        else
        {
            int dice = UnityEngine.Random.Range(0, 2);
            switch (dice)
            {
                case 0:
                    playerFirst = true;
                    StartCoroutine(PerformPlayerMove());
                    break;
                case 1:
                    playerFirst = false;
                    StartCoroutine(EnemyMove(false));
                    break;
                default:
                    playerFirst = true;
                    StartCoroutine(PerformPlayerMove());
                    break;
            }
        }

    }

    private void buttonActionClick(int num)
    {
        currentAction = num;
        print(currentAction);
        if (currentAction == 0)
        {
            PlayerMove();
        }
        else if (currentAction == 1)
        {

        }

    }

    private void AfterBattleFunc(int num)
    {
        if (num == 0)
        {
            wincount = 0;
            onBattleOver(false);

        }
        else
        {
        }
    }

    //private void setMoveColors(List<Move> moves, List<Button> buttonMoves)
    //{
    //    string colorString;
    //    Color newColor;


    //    for (int i = 0; i < buttonMoves.Count; i++)
    //    {
    //        switch (moves[i].Base.Type)
    //        {
    //            case MonsterType.God:
    //                colorString = "#7D00FF";
    //                ColorUtility.TryParseHtmlString(colorString, out newColor);
    //                buttonMoves[i].GetComponent<Image>().color = newColor;
    //                buttonMoves[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
    //                break;
    //            case MonsterType.Human:
    //                colorString = "#FFC5AB";
    //                ColorUtility.TryParseHtmlString(colorString, out newColor);
    //                buttonMoves[i].GetComponent<Image>().color = newColor;
    //                buttonMoves[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
    //                break;
    //            case MonsterType.Sun:
    //                colorString = "#FF4F00";
    //                ColorUtility.TryParseHtmlString(colorString, out newColor);
    //                buttonMoves[i].GetComponent<Image>().color = newColor;

    //                buttonMoves[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;

    //                break;
    //            case MonsterType.Earth:
    //                colorString = "#19FFE5";
    //                ColorUtility.TryParseHtmlString(colorString, out newColor);
    //                buttonMoves[i].GetComponent<Image>().color = newColor;
    //                buttonMoves[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
    //                break;
    //            case MonsterType.Moon:
    //                colorString = "#FFFF00";
    //                ColorUtility.TryParseHtmlString(colorString, out newColor);
    //                buttonMoves[i].GetComponent<Image>().color = newColor;
    //                buttonMoves[i].transform.GetChild(0).GetComponent<Text>().color = Color.black;
    //                break;
    //            case MonsterType.None:
    //                buttonMoves[i].GetComponent<Image>().color = Color.white;
    //                buttonMoves[i].transform.GetChild(0).GetComponent<Text>().color = Color.black;
    //                break;
    //        }
    //    }
    //}

    public void FloorNumberChange()
    {
        print("FloorNumberChange");
        floorNumber.text = $"{wincount + 1}F";
    }

    public void DevideComMove()
    {

        comMoveIndex = UnityEngine.Random.Range(0, 4);
        comMove = enemyUnit.Pokemon.Moves[comMoveIndex];

        List<string> MonsterAndMove = new List<string>();

        MonsterAndMove.Add(choiceScreen.comParty[comFlontIndex].Name);
        MonsterAndMove.Add(comMove.Base.Name);
        enemyChoiceRecord.Add(MonsterAndMove);
        for (var i = 0; i < playerChoiceRecord.Count; i++)
        {
            print(enemyChoiceRecord[i][0]);
            print(enemyChoiceRecord[i][1]);
        }
    }

    //今からcomが倒れた時の処理をやるぞ
    //まず生き残ってるあのhpリストをとってこないといけない
    //そうだ！！
    //生存ポケの擬似リストを作ろう

    void SetUp_ForComparty()
    {
        Image image = enemyUnit.GetComponent<Image>();
        Vector3 originalPos = image.transform.localPosition;
        //Color originalColor = Color.white;

        surviveComParty.Clear();
        for (var i = 0; i < comHPList.Count; i++)
        {
            print(comHPList[i]);
            if (comHPList[i] > 0)
            {
                surviveComParty.Add(choiceScreen.ComParty[i]);

            }

        }
        //print(surviveComParty.Count);
        for (var i = 0; i < surviveComParty.Count; i++)
        {
            print($"surviveComParty[i]{i}{surviveComParty[i]}");
        }

        int r = UnityEngine.Random.Range(0, surviveComParty.Count);
        MonsterBase rMonster = surviveComParty[r];
        comFlontIndex = choiceScreen.ComParty.IndexOf(rMonster);
        print($"comFlontIndex{comFlontIndex}");

        enemyUnit.Pokemon = new Pokemon(rMonster);
        //再戦時にcomHPListの更新か
        enemyUnit.Pokemon.HP = comHPList[comFlontIndex];
        print($"newPokemon.HP{enemyUnit.Pokemon.HP}");

        image.sprite = enemyUnit.Pokemon.Base.TheSprite;
        image.color = Color.white;
        image.transform.localPosition = new Vector3(originalPos.x, originalPos.y + 150);

        //これ原因はenemyUnitのpokemonが更新されてないこと
        EnemyHud.SetData(enemyUnit.Pokemon);
        print(enemyUnit.Pokemon.HP);


    }

    IEnumerator MoveProcess(BattleUnit attakerUnit,  Move attackerMove,Move defenderMove)
    {
        List<int> attackerRankList;
        List<int> preAttackerRankList = new List<int> { 0,0,0,0,0};
        List<int> defenderRankList;
        List<int> preDefenderRankList = new List<int> { 0, 0, 0, 0, 0 };
        //var waitSecond = 0.4f;

        Pokemon attacker;
        string attackerName;
        string defenderName;
        //bool isShield = false;
        Dictionary<string, bool> attackerMoveBoolDic;
        Dictionary<string, bool> defenderMoveBoolDic;




        if (attakerUnit == playerUnit)
        {
            attackerRankList = playerRankList;
            defenderRankList = enemyRankList;
            attacker = playerUnit.Pokemon;
            attackerName = playerUnit.Pokemon.Base.Name;
            defenderName = enemyUnit.Pokemon.Base.Name;
            attackerMoveBoolDic = moveBranch.playerMoveBoolDic;
            defenderMoveBoolDic = moveBranch.enemyMoveBoolDic;
        }
        else
        {
            attackerRankList = enemyRankList;
            defenderRankList = playerRankList;
            attacker = enemyUnit.Pokemon;
            attackerName = enemyUnit.Pokemon.Base.Name;
            defenderName = playerUnit.Pokemon.Base.Name;
            attackerMoveBoolDic = moveBranch.enemyMoveBoolDic;
            defenderMoveBoolDic = moveBranch.playerMoveBoolDic;
        }

        for (var i=0; i< 5; i++)
        {
            preAttackerRankList[i] = attackerRankList[i];
            preDefenderRankList[i] = defenderRankList[i];
        }

        if(attackerMoveBoolDic["チェンジ"])
        {
            isTypeChange = !isTypeChange;
        }





        if (attackerMove.Base.Name != "ブラックホール" || attackerMoveBoolDic["ブラックホール"])
        {
            preAttackerRankList[0] += attackerMove.Base.attackLevel_A;
            preAttackerRankList[1] += attackerMove.Base.defenceLevel_A;
            preAttackerRankList[2] += attackerMove.Base.spAttackLevel_A;
            preAttackerRankList[3] += attackerMove.Base.spDefenceLevel_A;
            preAttackerRankList[4] += attackerMove.Base.speedLevel_A;

            for(var i=0; i<5; i++)
            {
                if(preAttackerRankList[i] > 6)
                {
                    preAttackerRankList[i] = 6;
                }else if (preAttackerRankList[i] < -6)
                {
                    preAttackerRankList[i] = -6;
                }
            }
        }

        //if (defenderMove.Base.Name != "シールド")
        if(!defenderMoveBoolDic["シールド"])
        {
            preDefenderRankList[0] += attackerMove.Base.attackLevel_D;
            preDefenderRankList[1] += attackerMove.Base.defenceLevel_D;
            preDefenderRankList[2] += attackerMove.Base.spAttackLevel_D;
            preDefenderRankList[3] += attackerMove.Base.spDefenceLevel_D;
            preDefenderRankList[4] += attackerMove.Base.speedLevel_D;


            for (var i = 0; i < 5; i++)
            {
                if (preDefenderRankList[i] > 6)
                {
                    preDefenderRankList[i] = 6;
                }else if(preDefenderRankList[i] < -6)
                {
                    preDefenderRankList[i] = -6;
                }
            }

        }
        //else
        //{
        //    isShield = true;
        //}

        string statusName = "";
        for(var i=0; i<5; i++)
        {
            switch (i)
            {
                case 0:
                    statusName = "こうげき";
                    break;
                case 1:
                    statusName = "ぼうぎょ";
                    break;
                case 2:
                    statusName = "とくこう";
                    break;
                case 3:
                    statusName = "とくぼう";
                    break;
                case 4:
                    statusName = "すばやさ";
                    break;
            }

            if(preAttackerRankList[i] > attackerRankList[i])
            {
                yield return dialogBox.TypeDialog($"{attackerName}の{statusName}があがった");
                yield return new WaitForSeconds(0.2f);
            }else if(preAttackerRankList[i] < attackerRankList[i])
            {
                yield return dialogBox.TypeDialog($"{attackerName}の{statusName}がさがった");
                yield return new WaitForSeconds(0.2f);

            }
        }

        for (var i = 0; i < 5; i++)
        {
            switch (i)
            {
                case 0:
                    statusName = "こうげき";
                    break;
                case 1:
                    statusName = "ぼうぎょ";
                    break;
                case 2:
                    statusName = "とくこう";
                    break;
                case 3:
                    statusName = "とくぼう";
                    break;
                case 4:
                    statusName = "すばやさ";
                    break;
            }

            if (preDefenderRankList[i] > defenderRankList[i])
            {
                yield return dialogBox.TypeDialog($"{defenderName}の{statusName}があがった");
                yield return new WaitForSeconds(0.2f);
            }
            else if (preDefenderRankList[i] < defenderRankList[i])
            {
                yield return dialogBox.TypeDialog($"{defenderName}の{statusName}がさがった");
                yield return new WaitForSeconds(0.2f);

            }
        }



        for (var i = 0; i < 5; i++)
        {
            attackerRankList[i] = preAttackerRankList[i];
            defenderRankList[i] = preDefenderRankList[i];
        }


    }


    List<int> playerRankList = new List<int> { 0, 0, 0, 0, 0 };
    List<int> enemyRankList = new List<int> { 0, 0, 0, 0, 0 };

    float statusChangeFromRank(int Rank)
    {
        switch (Rank)
        {
            case 1:
                return 1.5f;
            case 2:
                return 2f;
            case 3:
                return 2.5f;
            case 4:
                return 3f;
            case 5:
                return 3.5f;
            case 6:
                return 4f;
            case -1:
                return 2 / 3f;
            case -2:
                return 0.5f;
            case -3:
                return 0.4f;
            case -4:
                return 0.33f;
            case -5:
                return 0.28f;
            case -6:
                return 0.25f;
            default:
                return 1f;

        }
    }

    public float PlayerAttackRankStatus
    {

        get
        {
            return statusChangeFromRank(playerRankList[0]);
        }
    }

    public float PlayerDefenceRankStatus
    {
        get
        {
            return statusChangeFromRank(playerRankList[1]);
        }

    }

    public float PlayerspAttackRankStatus
    {
        get
        {
            return statusChangeFromRank(playerRankList[2]);
        }

    }

    public float PlayerspDefenceRankStatus
    {
        get
        {
            return statusChangeFromRank(playerRankList[3]);
        }

    }

    public float PlayerSpeedRankStatus
    {
        get
        {
            return statusChangeFromRank(playerRankList[4]);
        }
    }

    public float EnemyAttackRankStatus
    {

        get
        {
            return statusChangeFromRank(enemyRankList[0]);
        }
    }

    public float EnemyDefenceRankStatus
    {
        get
        {
            return statusChangeFromRank(enemyRankList[1]);
        }

    }

    public float EnemyspAttackRankStatus
    {
        get
        {
            return statusChangeFromRank(enemyRankList[2]);
        }

    }

    public float EnemyspDefenceRankStatus
    {
        get
        {
            return statusChangeFromRank(enemyRankList[3]);
        }

    }

    public float EnemySpeedRankStatus
    {
        get
        {
            return statusChangeFromRank(enemyRankList[4]);
        }
    }



    public void ResetVariables()
    {
        playerFlontIndex = 0;
        comFlontIndex = 0;

        for (var i = 0; i < 5; i++)
        {
            playerRankList[i] = 0;
            enemyRankList[i] = 0;
        }

        TurnNum = 0;
        playerChoiceRecord = new List<List<string>>();
        enemyChoiceRecord = new List<List<string>>();
        isTypeChange = false;
        ailments.AilReset();

    }



}




