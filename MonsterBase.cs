using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonsterBase : ScriptableObject
{
    [SerializeField] string _name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite theSprite;

    [SerializeField] MonsterType type1;
    [SerializeField] MonsterType type2;

    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defence;
    [SerializeField] int spAttack;
    [SerializeField] int spDefence;
    [SerializeField] int speed;
    [SerializeField] int cost;

    [SerializeField] List<LearnableMove> learnableMoves ;

    public string Name
    {
        get { return _name; }
    }

    public string Description
    {
        get { return description; }
    }


    public Sprite TheSprite
    {
        get { return theSprite; }
    }

    public MonsterType Type1
    {
        get { return type1; }
    }

    public MonsterType Type2
    {
        get { return type2; }
    }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defence
    {
        get { return defence; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int SpDefence
    {
        get { return spDefence; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public int Cost
    {
        get { return cost; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }

}

[System.Serializable]

public class LearnableMove
{
    [SerializeField] MoveBase moveBase;

    public MoveBase Base
    {
        get { return moveBase; }
    }
}


public enum MonsterType
{
    None,
    God,
    Human,
    Sun,
    Earth,
    Moon,
}


public class newTypeChart
{
    

    static float[][] chart1 =
    {
        //                    GOD   HUM    SUN   EAR   MOO   
        /*GOD*/  new float[]{   2f, 0.5f,   1f,   1f,   1f,},
        /*HUM*/  new float[]{   3f, 0.5f,   1f,   1f,   1f,},
        /*SUN*/  new float[]{ 0.5f,   2f,   1f,   2f, 0.5f,},
        /*EAR*/  new float[]{ 0.5f,   2f, 0.5f,   1f,   2f,},
        /*MOO*/  new float[]{ 0.5f,   2f,   2f, 0.5f,   1f,},
    };

    static float[][] chart2 =
    {
        //                    GOD   HUM    SUN   EAR   MOO   
        /*GOD*/  new float[]{ 0.5f,   2f,   1f,   1f,   1f,},
        /*HUM*/  new float[]{ 0.5f,   2f,   1f,   1f,   1f,},
        /*SUN*/  new float[]{   2f, 0.5f,   1f, 0.5f,   2f,},
        /*EAR*/  new float[]{   2f, 0.5f,   2f,   1f, 0.5f,},
        /*MOO*/  new float[]{   2f, 0.5f, 0.5f,   2f,   1f,},
    };

    

    public static float GetEffectiveness(MonsterType attackType, MonsterType defenceType,bool isTypeChange)
    {
        if (attackType == MonsterType.None || defenceType == MonsterType.None)
            return 1;

        

        int row = (int)attackType - 1;
        int col = (int)defenceType - 1;

        if (isTypeChange)
            return chart2[row][col];

        return chart1[row][col];

    }

    

}
