using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class AilmentBase : ScriptableObject
{
    
    [SerializeField] string _name;
    public MonsterType ailType;
    public AudioClip moveSe;
    //[SerializeField] Animation animation;
    public float constantDamage;

    public int recoveryTurn = 5;

    public float ailAtt;
    public float ailDef;
    public float ailSat;
    public float ailSde;
    public float ailSpe;

    public string Name
    {
        get { return _name; }
    }

}
