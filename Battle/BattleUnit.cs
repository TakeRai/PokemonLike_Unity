using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] MonsterBase _base;
    [SerializeField] bool isPlayerUnit;
    [SerializeField] ChoiceScreen choiceScreen;
    [SerializeField] BattleSystem battleSystem;



    

    public Pokemon Pokemon { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;


    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void SetUp()
    {

        List<MonsterBase> choiceBases = choiceScreen.ChoiceMonsters;
        List<MonsterBase> comPartys = choiceScreen.ComParty;

        if (isPlayerUnit)
        {
            battleSystem.playerHPList.Clear();
            Pokemon = new Pokemon(choiceBases[0]);
            for (var i = 0; i < 3; i++)
            {
                //battleSystem.playerHPList[i] = new Pokemon(choiceBases[i]).MaxHp;
                //battleSystem.playerHPList[i] = new Pokemon(choiceBases[i]).MaxHp;
                battleSystem.playerHPList.Add(new Pokemon(choiceBases[i]).MaxHp);
            }

        }
        else
        {
            battleSystem.comHPList.Clear();
            Pokemon = new Pokemon(comPartys[0]);
            for (var i = 0; i < 3; i++)
            {
                //battleSystem.comHPList[i] = new Pokemon(comPartys[i]).MaxHp;
                battleSystem.comHPList.Add(new Pokemon(comPartys[i]).MaxHp);
            }
        }
        
        

        image.sprite = Pokemon.Base.TheSprite;

        image.color = originalColor; 
        PlayerEnterAnimation();
            
    }

    public void SetUp_ForParty(int index)
    {

        List<MonsterBase> choiceBases = choiceScreen.ChoiceMonsters;
        battleSystem.playerFlontIndex = index;

        Pokemon = new Pokemon(choiceBases[index]);
        Pokemon.HP = battleSystem.playerHPList[battleSystem.playerFlontIndex];
        //else
        //{
        //    //今の所indexはplayer側のみで
        //    Pokemon = new Pokemon(comPartys[0]);
        //}
        image.sprite = Pokemon.Base.TheSprite;
        image.transform.localPosition = new Vector3(originalPos.x, originalPos.y);

        image.color = originalColor;

        battleSystem.playerChange = true;

    }

    

    public void PlayerEnterAnimation()
    {
        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(500f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayerHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f ,0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }

    
}
