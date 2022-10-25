using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoicedPanel : MonoBehaviour
    , IPointerDownHandler
    , IPointerUpHandler

{

    [System.NonSerialized] public bool isEmpty = true;

    [SerializeField] ChoicedSwiping choicedSwiping;


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
        //if (!isEmpty)
        //{
        //    swipingCard.tapStartTime = Time.time;
        //    swipingCard.isSwipeOk = true;

        //}
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        //if (Time.time - swipingCard.tapStartTime <= 0.4f)
        //{
        //    print("triel");
        //    swipingCard.isSwipeOk = false;

        //    swipingCard.isSwiping = false;
        //    swipingCard.transform.GetChild(0).gameObject.SetActive(false);

        //    monsterScroll.GetComponent<ScrollRect>().enabled = true;


        //}
    }
}
