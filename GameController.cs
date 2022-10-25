using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

//public enum GameState { Start,Battle,Home}

public class GameController : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] StartScreen startScreen;
    [SerializeField] Home homeScreen;
    public ChoiceScreen choiceScreen;
    [SerializeField] ExchangeScreen exchangeScreen;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] Ailments ailments;
    [SerializeField] costListList costList;


    [SerializeField] Button startButton;
    [SerializeField] Button towerButton;
    [SerializeField] Button goButtleButton;
    [SerializeField] Button goExchangeButton;
    [SerializeField] Button goPartyButton;
    [SerializeField] Button returnBattle1Button;
    [SerializeField] Button returnBattle2Button;

    [SerializeField] Button againButtleButton;

    


    public SoundManager soundManager;
    public Background background;
    public BattleDialogBox dialogBox;

    bool isPlayerDead;


    //GameState state;

    //private void Awake()
    //{
    //    // フレームレート設定（FPS60にしたい場合）
    //    Application.targetFrameRate = 60;
    //}

    private void Start()
    {
        soundManager.startBGMplay();
        startButton.onClick.AddListener(GoHome);
        towerButton.onClick.AddListener(GoChoice);

        battleSystem.onBattleOver += EndBattle;
        goButtleButton.onClick.AddListener(GoBattle);
        goExchangeButton.onClick.AddListener(GoExchange);
        againButtleButton.onClick.AddListener(AgainBattle);
        goPartyButton.onClick.AddListener(() => GoParty(isDead:false));
        returnBattle1Button.onClick.AddListener(Return1Battle);
        returnBattle2Button.onClick.AddListener(Return2Battle);
        
    }

    void GoHome() {

        //state = GameState.Home;
        battleSystem.gameObject.SetActive(false);
        startScreen.gameObject.SetActive(false);
        homeScreen.gameObject.SetActive(true);

    }

    private void GoChoice()
    {
        choiceScreen.SetCostText();
        costList.MakeEmptyCostCostList_p();

        //choiceScreen.playerCostListList = new();
        //for (var i = 0; i < 10; i++)
        //{
        //    costList list_i;
        //    list_i = new costList(choiceScreen.modelCostListList[i].List);
        //    list_i.List.Clear();
        //    for (var j = 0; j < choiceScreen.modelCostListList[i].List.Count; j++)
        //    {
        //        list_i.List.Add(choiceScreen.modelCostListList[i].List[j]);
        //    }
        //    choiceScreen.playerCostListList.Add(list_i);
        //}
        print(choiceScreen.playerCostListList);
        choiceScreen.ComChoiceMonsters();
        choiceScreen.panelImageShuffle();
        choiceScreen.gameObject.SetActive(true);
        homeScreen.gameObject.SetActive(false); 
    }

    void GoExchange()
    {
        //choiceScreen.ResetCostListList();
        print("welcome Exchange");
        //おそらく
        choiceScreen.chooseComParty_forAgain();
        exchangeScreen.RenderExchangeMonsters();
        exchangeScreen.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);
        soundManager.startBGMplay();

    }

    public void GoParty(bool isDead)
    {
        isPlayerDead = isDead;
        ailments.SetAilArea_Party();
        partyScreen.SetStatus(0);
        partyScreen.SetMoveInfo(0);
        partyScreen.SetColor_PushedButton();
        partyScreen.RenderParty();
        partyScreen.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);
    }

    void Return1Battle()
    {

        battleSystem.gameObject.SetActive(true);
        partyScreen.gameObject.SetActive(false);

    }

    void Return2Battle()
    {
        if(battleSystem.playerHPList[partyScreen.monsterIndex] > 0)
        {
            battleSystem.gameObject.SetActive(true);
            partyScreen.gameObject.SetActive(false);
            //これコメントアウトしたからどっかでクル
            //StartCoroutine(battleSystem.EnemyMove(true));
            StartCoroutine(battleSystem.changeFrontSystem_forPlayer(isDead: isPlayerDead));

        }
        else
        {

        }

    }

    void AgainBattle()
    {
        //二戦目以降更新されないということはnewComPartyがそのままということ
        //これやな
        //choiceScreen.comParty = choiceScreen.newComParty;
        for(var i=0; i<3; i++)
        {
            choiceScreen.comParty[i] = choiceScreen.newComParty[i];
            battleSystem.comHPList[i] = new Pokemon(choiceScreen.comParty[i]).MaxHp;
        }
        exchangeScreen.devideParty();
        dialogBox.EnableAfterBattleSelector(false);
        soundManager.battleBGMplay();
        battleSystem.FloorNumberChange();
        battleSystem.gameObject.SetActive(true);
        exchangeScreen.gameObject.SetActive(false)  ;
        battleSystem.StartBattle();
    }


    void GoBattle()
    {
        if (choiceScreen.choiceMonsters.Count == 3)
        {
            dialogBox.EnableAfterBattleSelector(false);
            background.BGchoose();
            soundManager.battleBGMplay();

            battleSystem.gameObject.SetActive(true);
            //startScreen.gameObject.SetActive(false);
            choiceScreen.gameObject.SetActive(false);

            battleSystem.StartBattle();
        }

    }

    void EndBattle(bool won)
    {
        //state = GameState.Start;
        soundManager.startBGMplay();
        battleSystem.gameObject.SetActive(false);
        startScreen.gameObject.SetActive(false);
        homeScreen.gameObject.SetActive(true);
    }
}