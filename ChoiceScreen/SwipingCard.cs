using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SwipingCard : MonoBehaviour
    //, IPointerUpHandler
    //, IPointerDownHandler
    //, IDragHandler
    ,IDropHandler
{
    //[System.NonSerialized] public bool swipeOff = false;
    [System.NonSerialized] public bool isSwipeOk = false;
    [System.NonSerialized] public bool isSwiping = false;
    [System.NonSerialized] public float tapStartTime;
    [System.NonSerialized] public float tapDuringTime;
    [System.NonSerialized] public MonsterBase swipedMonster;


    [SerializeField] ChoiceScreen choiceScreen;
    [SerializeField] Button goBattleButton;
    [SerializeField] ContentSizeFitter costPanels_content;
    [SerializeField] SwipingCard swipingCard;
    [SerializeField] MonsterScroll monsterScroll;
    [SerializeField] List<MoveEffect> circleEffects;
    //[SerializeField] List<Button> choicedPanels;
    [SerializeField] List<ChoicedPanel> choicedPanels;
    [SerializeField] Text remainCostText;
    [SerializeField] RectTransform costCones;
    [SerializeField] MonsterBase zzzzz;


    Vector3 mousePosition;

    // Update is called once per frame
    //そもそもactiveがoffだから
    void Update()
    {
        tapDuringTime = Time.time;

        Camera gameCamera = Camera.main;
        if (isSwipeOk)
        {
            
            if(Time.time - tapStartTime > 0.6f && !isSwiping)
            { 
                print("swipeok");
                swipingCard.transform.GetChild(0).gameObject.SetActive(true);
                swipingCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = swipedMonster.TheSprite;
                for(var i=0; i<3; i++)
                {
                    circleEffects[i].loopCircle(); 
                }
                isSwiping = true;

            }
        }

        if (isSwiping)
        {
            if(monsterScroll.GetComponent<ScrollRect>().enabled == true)
            {
                monsterScroll.GetComponent<ScrollRect>().enabled = false;
            }

            Vector3 vector_here = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            vector_here.z = 0;
            swipingCard.transform.GetChild(0).position = vector_here;


        }

    }


    public void OnDrop(PointerEventData eventData)
    {

        for(var i=0; i < 3; i++)
        {
            if (Vector2.Distance(inPanelPosition(this.transform.GetChild(0).position), inPanelPosition(choicedPanels[i].transform.position)) <= 60f)
            {
                if (choiceScreen.playerPartyCost - (swipedMonster.Cost) >= 0 &&
                !choiceScreen.choiceMonsters.Contains(swipedMonster))
                //&& choiceScreen.choiceMonsters.Contains(zzzzz) 
                {
                    choicedPanels[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = swipedMonster.TheSprite;
                    choicedPanels[i].transform.GetChild(1).gameObject.SetActive(true);
                    choiceScreen.choiceMonsters[i] = swipedMonster;
                    costPanels_content.transform.GetChild(swipedMonster.Cost - 1).GetComponent<SwipeCards>().pressed = false;

                    int prePlayerPartyCost = choiceScreen.playerPartyCost;
                    choiceScreen.playerPartyCost -= (swipedMonster.Cost);
                    remainCostText.text = $"{choiceScreen.playerPartyCost}";

                    //以下は10のパネルの当てはめ時の透過処理
                    float toukaNum = 0.2f;
                    float toukaTime = 0.3f;
                    Transform costpanel_here = costPanels_content.transform.GetChild(swipedMonster.Cost-1);
                    Transform costfront_here = costPanels_content.transform.GetChild(swipedMonster.Cost-1).GetChild(0);

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

                    for (var j = choiceScreen.playerPartyCost; j < prePlayerPartyCost; j++)
                    {

                        var index_here = j;
                        DOTween.ToAlpha(
                        () => costCones.transform.GetChild(index_here).GetChild(0).GetComponent<Image>().color,
                        color => costCones.transform.GetChild(index_here).GetChild(0).GetComponent<Image>().color = color,
                        0.2f,
                        0.4f
                        );
                    }

                }


            }
        }

        if (!choiceScreen.choiceMonsters.Contains(zzzzz))
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

        isSwipeOk = false;

        isSwiping = false;
        swipingCard.transform.GetChild(0).gameObject.SetActive(false);
        monsterScroll.GetComponent<ScrollRect>().enabled = true;
        swipingCard.stopCircling();
    }

    public void stopCircling()
    {
        for (var i = 0; i < 3; i++)
        {
            circleEffects[i].stopCircle();
        }
    }

    //パネル内の座標
    Vector3 inPanelPosition(Vector3 targetposition)
    {
        return this.transform.parent.InverseTransformPoint(targetposition);
    }
}
