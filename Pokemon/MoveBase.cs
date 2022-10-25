using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]


public class MoveBase : ScriptableObject
{
    [SerializeField] string _name;

    [TextArea]
    [SerializeField] string desription;

    //[SerializeField] PokemonType type = PokemonType.Normal;
    [SerializeField] MonsterType type;
    [SerializeField] int power;
    [SerializeField] int accuracy = 100;
    [SerializeField] int pp = 10;
    [SerializeField] float recovery = 0f;
    [SerializeField] bool isMental;
    [SerializeField] bool isSituation;
    [SerializeField] int priority = 0;
    public AilmentBase ailmentBase;


    public bool selfSide;
    

    public AudioClip moveSe;


    // Listは0からこうげき　ぼうぎょ　とくこう　とくぼう　すばやさ
    //public List<int> ChangeLevelList_A;
    //public List<int> ChangeLevelList_D;

    public int attackLevel_A = 0;
    public int defenceLevel_A = 0;
    public int spAttackLevel_A = 0;
    public int spDefenceLevel_A = 0;
    public int speedLevel_A = 0;

    public int attackLevel_D = 0;
    public int defenceLevel_D = 0;
    public int spAttackLevel_D = 0;
    public int spDefenceLevel_D = 0;
    public int speedLevel_D = 0;




    public string Name
    {
        get { return _name; }
    }

    public string Description
    {
        get { return desription; }
    }

    public MonsterType Type
    { 
        get { return type; }
    }

    public int Power
    {
        get { return power; }
    }

    public int Accuracy
    {
        get { return accuracy; }
    }

    public int PP
    {
        get { return pp; }
    }

    public float Recovery
    {
        get { return recovery; }
    }

    public bool IsMental
    {
        get { return isMental; }
    }

    public bool IsSituation
    {
        get { return isSituation; }
    }

    public int Priority
    {
        get { return priority; }
    }

    public AudioClip MoveSe
    {
        get { return moveSe; }
    }

    public bool SelfSide
    {
        get { return selfSide; }
    }
}
