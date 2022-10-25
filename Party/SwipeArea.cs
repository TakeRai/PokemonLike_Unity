using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SwipeArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [System.NonSerialized] public bool isPage1 = true;
    [System.NonSerialized] public float startPos = 0;
    [System.NonSerialized] public float endPos = 0;

    [System.NonSerialized] public float previousPosX = 0;
    [System.NonSerialized] public float currentPosX = 0;
    [System.NonSerialized] public bool position = false;
    [System.NonSerialized] public bool isDrag = false;

    [SerializeField] SwipeArea swipeArea  ;



    // Update is called once per frame
    void Update()
    {

       

        if(isPage1 && position)
        {
            if(startPos - endPos > 100)
            {
                swipeArea.transform.DOLocalMoveX(-240, 0.3f);
                //swipeArea.transform.localPosition = new Vector3(-240, swipeArea.transform.localPosition.y, swipeArea.transform.localPosition.z);
                isPage1 = false;
                position = false;
            }
            else
            {
                swipeArea.transform.DOLocalMoveX(240, 0.3f);
                //swipeArea.transform.localPosition = new Vector3(240, swipeArea.transform.localPosition.y, swipeArea.transform.localPosition.z);
                position = false;
            }

            
        }
        else if(!isPage1 && position)
        {
            if (endPos - startPos > 100)
            {
                print("Right1");
                swipeArea.transform.DOLocalMoveX(240, 0.3f);
                //swipeArea.transform.localPosition = new Vector3(240, swipeArea.transform.localPosition.y, swipeArea.transform.localPosition.z);
                isPage1 = true;
                position = false;
            }
            else
            {
                print("Right2");
                swipeArea.transform.DOLocalMoveX(-240, 0.3f);
                //swipeArea.transform.localPosition = new Vector3(-240, swipeArea.transform.localPosition.y, swipeArea.transform.localPosition.z);
                position = false;
            }

        }




        if (isDrag)
        {
            currentPosX = Input.mousePosition.x;
            swipeArea.transform.localPosition += new Vector3(currentPosX - previousPosX, 0, 0);
            previousPosX = currentPosX;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        previousPosX = Input.mousePosition.x;
        startPos = Input.mousePosition.x;
        isDrag = true;
        //print("aaaaaaaaaaaaaa");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        endPos = Input.mousePosition.x;
        position = true;
        isDrag = false;
    }

    
}
