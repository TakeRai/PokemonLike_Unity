using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shapes2D;


[System.SerializableAttribute]
public class TypeTextAreas_party
{
    public List<GameObject> List = new List<GameObject>();

    public TypeTextAreas_party(List<GameObject> list)
    {
        List = list;
    }

}

public class PartyScreen : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] ChoiceScreen choiceScreen;
    [SerializeField] List<GameObject> PartyMonsters_object;
    [SerializeField] List<Button> PartyMonsters_button;
    [SerializeField] List<TypeTextAreas_party> typeTextAreas_party_List = new List<TypeTextAreas_party>();
    [SerializeField] List<Button> parMoveButtons;
    [SerializeField] List<Text> StatusValues;
    [SerializeField] Text powerText;
    [SerializeField] Text descriptionText;



    MonsterBase frontMonster;
    [System.NonSerialized] public int monsterIndex;
    int previousMonsterIndex = 0;
    int previousParMoveButtons = -1;
    Color previousMoveOutlineColor;
    //int moveIndex;

    

    void Start()
    {
        frontMonster = new MonsterBase();

        for(var i=0; i < PartyMonsters_button.Count; i++)
        {
            var num = i;
            PartyMonsters_button[i].onClick.AddListener(() => FrontChange(num));
        }

        for(var i=0; i < parMoveButtons.Count; i++)
        {
            var num = i;
            parMoveButtons[i].onClick.AddListener(() => SetMoveInfo(num));
        }
    }

    private void FrontChange(int num)
    {

        monsterIndex = num;
        SetColor_PushedButton();
        SetStatus(num);
        SetMoveInfo(0);
        previousMonsterIndex = monsterIndex;

    }

    public void RenderParty()
    {
        for(var i=0; i<3; i++)
        {
            Image partyImage = PartyMonsters_object[i].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
            Text partyName = PartyMonsters_object[i].transform.GetChild(1).GetChild(1).GetComponent<Text>();

            var healthBar = PartyMonsters_object[i].transform.GetChild(1).GetChild(3).GetChild(1);

            var partyHPTextArea = PartyMonsters_object[i].transform.GetChild(1).GetChild(3).GetChild(3);
            Text remainHPText = partyHPTextArea.GetChild(0).GetComponent<Text>();
            Text maxHPText = partyHPTextArea.GetChild(2).GetComponent<Text>();



            partyImage.sprite = choiceScreen.ChoiceMonsters[i].TheSprite;
            partyName.text = choiceScreen.ChoiceMonsters[i].Name;

            Pokemon pokemon = new Pokemon(choiceScreen.ChoiceMonsters[i]);

            remainHPText.text = $"{battleSystem.playerHPList[i]}";
            maxHPText.text = $"{pokemon.MaxHp}";
            healthBar.transform.localScale = new Vector3((float)battleSystem.playerHPList[i] / pokemon.MaxHp, 1f);
            
            playerHud.SetType(choiceScreen.ChoiceMonsters[i], typeTextAreas_party_List[i].List);

        }

        
    }

    public void changeParty()
    {
        playerUnit.SetUp_ForParty(monsterIndex);

        //playerHud.SetData(playerUnit.Pokemon);

        //dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        //setMoveColors(playerUnit.Pokemon.Moves);
        battleSystem.SetInfo(playerHud, playerUnit);
    }

    public void SetStatus(int num)
    {
        string colorString;
        Color newColor;

        for (int i = 0; i < parMoveButtons.Count; i++)
        {

            parMoveButtons[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = choiceScreen.ChoiceMonsters[num].LearnableMoves[i].Base.Name;


            switch (choiceScreen.ChoiceMonsters[num].LearnableMoves[i].Base.Type)
            {
                case MonsterType.God:
                    colorString = "#7D00FF";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    Here(parMoveButtons[i], newColor);
                    break;
                case MonsterType.Human:
                    colorString = "#FFC5AB";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    Here(parMoveButtons[i], newColor);
                    break;
                case MonsterType.Sun:
                    colorString = "#FF4F00";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    Here(parMoveButtons[i], newColor);
                    break;
                case MonsterType.Earth:
                    colorString = "#19FFE5";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    Here(parMoveButtons[i], newColor);
                    break;
                case MonsterType.Moon:
                    colorString = "#FFFF00";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    Here(parMoveButtons[i], newColor);
                    break;
                case MonsterType.None:
                    Here(parMoveButtons[i], Color.black);
                    break;
            }
        }

        void Here(Button parMoveButtons_i, Color outcolor)
        {
            parMoveButtons_i.transform.GetChild(1).GetComponent<Shape>().settings.outlineColor = outcolor;

        }

        Pokemon choicedPokemon = new Pokemon(choiceScreen.ChoiceMonsters[num]);
        StatusValues[0].text = $"{choicedPokemon.MaxHp}";
        StatusValues[1].text = $"{choicedPokemon.Attack}";
        StatusValues[2].text = $"{choicedPokemon.Defence}";
        StatusValues[3].text = $"{choicedPokemon.SpAttack}";
        StatusValues[4].text = $"{choicedPokemon.SpDefence}";
        StatusValues[5].text = $"{choicedPokemon.Speed}";

    }

    public void SetMoveInfo(int num)
    {
        string colorString;
        Color newColor;

        powerText.text = $"{choiceScreen.ChoiceMonsters[monsterIndex].LearnableMoves[num].Base.Power}";
        descriptionText.text = $"{choiceScreen.ChoiceMonsters[monsterIndex].LearnableMoves[num].Base.Description}";

        switch (choiceScreen.ChoiceMonsters[monsterIndex].LearnableMoves[num].Base.Type)
        {
            case MonsterType.God:
                colorString = "#7D00FF";
                ColorUtility.TryParseHtmlString(colorString, out newColor);
                Here(newColor);
                previousMoveOutlineColor = newColor;
                break;
            case MonsterType.Human:
                colorString = "#FFC5AB";
                ColorUtility.TryParseHtmlString(colorString, out newColor);
                Here(newColor);
                previousMoveOutlineColor = newColor;
                break;
            case MonsterType.Sun:
                colorString = "#FF4F00";
                ColorUtility.TryParseHtmlString(colorString, out newColor);
                Here( newColor);
                previousMoveOutlineColor = newColor;
                break;
            case MonsterType.Earth:
                colorString = "#19FFE5";
                ColorUtility.TryParseHtmlString(colorString, out newColor);
                Here(newColor);
                previousMoveOutlineColor = newColor;
                break;
            case MonsterType.Moon:
                colorString = "#FFFF00";
                ColorUtility.TryParseHtmlString(colorString, out newColor);
                Here(newColor);
                previousMoveOutlineColor = newColor;
                break;
            case MonsterType.None:
                Here(Color.black);
                previousMoveOutlineColor = Color.black;
                break;
        }



        void Here(Color color)
        {
            print("vergin");
            parMoveButtons[num].transform.GetChild(1).GetComponent<Shape>().settings.outlineColor = Color.black;
            parMoveButtons[num].transform.GetChild(1).GetComponent<Image>().color = color;
            parMoveButtons[num].transform.GetChild(1).localPosition = new Vector3(2, -5);

            if(previousParMoveButtons != -1 && previousParMoveButtons != num)
            {
                parMoveButtons[previousParMoveButtons].transform.GetChild(1).GetComponent<Image>().color = Color.white;
                parMoveButtons[previousParMoveButtons].transform.GetChild(1).GetComponent<Shape>().settings.outlineColor = Here2();
                parMoveButtons[previousParMoveButtons].transform.GetChild(1).localPosition = new Vector3(0, 0);
            }

            

        }

        Color Here2()
        {
            string hereString;
            Color hereColor;

            switch (choiceScreen.ChoiceMonsters[monsterIndex].LearnableMoves[previousParMoveButtons].Base.Type)
            {
                
                case MonsterType.God:
                    hereString = "#7D00FF";
                    ColorUtility.TryParseHtmlString(hereString, out hereColor);
                    return hereColor;
                case MonsterType.Human:
                    hereString = "#FFC5AB";
                    ColorUtility.TryParseHtmlString(hereString, out hereColor);
                    return hereColor;
                case MonsterType.Sun:
                    hereString = "#FF4F00";
                    ColorUtility.TryParseHtmlString(hereString, out hereColor);
                    return hereColor;
                case MonsterType.Earth:
                    hereString = "#19FFE5";
                    ColorUtility.TryParseHtmlString(hereString, out hereColor);
                    return hereColor;
                case MonsterType.Moon:
                    hereString = "#FFFF00";
                    ColorUtility.TryParseHtmlString(hereString, out hereColor);
                    return hereColor;
                default:
                    print("None2");
                    return Color.black;

            }
        }


        previousParMoveButtons = num;
    }

    //これおそらくgoparty時にも開かないといけない
    public void SetColor_PushedButton()
    {
        Transform partyMonButton_front = PartyMonsters_button[monsterIndex].transform.GetChild(1);
        
        partyMonButton_front.GetComponent<Image>().color = Color.black;
        partyMonButton_front.GetChild(1).GetComponent<Text>().color = Color.white;
        for(var i=0; i<3; i++)
        {
            partyMonButton_front.GetChild(3).GetChild(3).GetChild(i).GetComponent<Text>().color = Color.white;
        }
        partyMonButton_front.localPosition = new Vector3(2, -5);

        if(previousMonsterIndex != monsterIndex)
        {
            Transform prePartyMonButton_front = PartyMonsters_button[previousMonsterIndex].transform.GetChild(1);
            prePartyMonButton_front.GetComponent<Image>().color = Color.white;
            
            prePartyMonButton_front.localPosition = new Vector3(0, 0);

        }



    }



}
