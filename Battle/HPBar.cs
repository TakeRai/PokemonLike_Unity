using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    

    public void SetHP(float hpNormallized)
    {
        health.transform.localScale = new Vector3(hpNormallized, 1f);
    }

    public IEnumerator SetHPSmooth(float newHp,bool isSituation)
    {
        float curHp = health.transform.localScale.x;
        if(curHp >= newHp)
        {
            //print("SetHPSmooth 1");
            float changeAmt = curHp - newHp;
            //print("SetHPSmooth 2");
            if (!isSituation)
            {
                while (curHp - newHp > Mathf.Epsilon)
                {
                    //print("SetHPSmooth 3");
                    curHp -= changeAmt * Time.deltaTime;
                    //print("SetHPSmooth 4");
                    health.transform.localScale = new Vector3(curHp, 1f);
                    //print("SetHPSmooth 5");
                    yield return null;
                    //print("SetHPSmooth 6");
                }

            }
            health.transform.localScale = new Vector3(newHp, 1f);
        }
        else
        {
            float changeAmt = newHp - curHp;

            while (newHp - curHp > Mathf.Epsilon)
            {
                curHp += changeAmt * Time.deltaTime;
                health.transform.localScale = new Vector3(curHp, 1f);
                yield return null;
            }
            health.transform.localScale = new Vector3(newHp, 1f);

        }

    }

}
