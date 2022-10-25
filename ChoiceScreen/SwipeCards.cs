using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwipeCards : MonoBehaviour
    ,IPointerDownHandler
    , IPointerUpHandler
{

    [SerializeField] SwipingCard swipingCard;
    [SerializeField] MonsterScroll monsterScroll;
    [SerializeField] ChoiceScreen choiceScreen;
    [System.NonSerialized] public MonsterBase monster_here;
    [System.NonSerialized] public bool pressed = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (pressed)
        {
            swipingCard.tapStartTime = Time.time;
            swipingCard.isSwipeOk = true;
            swipingCard.swipedMonster = monster_here;

        }


    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        //これで何ができるというのか wakattenaiTV
        if(swipingCard.tapDuringTime - swipingCard.tapStartTime <= 0.6f)
        {
            print("triel");
            swipingCard.isSwipeOk = false;

            swipingCard.isSwiping = false;
            swipingCard.transform.GetChild(0).gameObject.SetActive(false);
            
            monsterScroll.GetComponent<ScrollRect>().enabled = true;


        }
    }

}
