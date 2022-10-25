using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PartyMoveButton : MonoBehaviour
    , IPointerDownHandler
    , IPointerUpHandler
    //, IPointerEnterHandler
    //, IPointerExitHandler
{

    [SerializeField] SwipeArea swipeArea;
    //bool isPage1 = true;
    //bool isDrag = false;
    //bool position = false;

    //float startPos = 0;
    //float endPos = 0;
    //float previousPosX = 0;
    //float currentPosX = 0;


    void Update()
    {

        if (swipeArea.isPage1 && swipeArea.position)
        {
            if (swipeArea.startPos - swipeArea.endPos > 100)
            {
                swipeArea.transform.DOLocalMoveX(-240, 0.3f);
                //swipeArea.transform.localPosition = new Vector3(-240, swipeArea.transform.localPosition.y, swipeArea.transform.localPosition.z);
                swipeArea.isPage1 = false;
                swipeArea.position = false;
            }
            else
            {
                swipeArea.transform.DOLocalMoveX(240, 0.3f);
                //swipeArea.transform.localPosition = new Vector3(240, swipeArea.transform.localPosition.y, swipeArea.transform.localPosition.z);
                swipeArea.position = false;
            }


        }
        else if (!swipeArea.isPage1 && swipeArea.position)
        {
            if (swipeArea.endPos - swipeArea.startPos > 100)
            {
                print("Right1");
                swipeArea.transform.DOLocalMoveX(240, 0.3f);
                //swipeArea.transform.localPosition = new Vector3(240, swipeArea.transform.localPosition.y, swipeArea.transform.localPosition.z);
                swipeArea.isPage1 = true;
                swipeArea.position = false;
            }
            else
            {
                swipeArea.transform.DOLocalMoveX(-240, 0.3f);
                print("Right2");
                //swipeArea.transform.localPosition = new Vector3(-240, swipeArea.transform.localPosition.y, swipeArea.transform.localPosition.z);
                swipeArea.position = false;
            }

        }




        if (swipeArea.isDrag)
        {
            swipeArea.currentPosX = Input.mousePosition.x;
            swipeArea.transform.localPosition += new Vector3(swipeArea.currentPosX - swipeArea.previousPosX, 0, 0);
            swipeArea.previousPosX = swipeArea.currentPosX;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        swipeArea.previousPosX = Input.mousePosition.x;
        swipeArea.startPos = Input.mousePosition.x;
        swipeArea.isDrag = true;
        //print("aaaaaaaaaaaaaa");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        swipeArea.endPos = Input.mousePosition.x;
        swipeArea.position = true;
        swipeArea.isDrag = false;
    }

}
