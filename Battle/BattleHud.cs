using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    //[SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] List<GameObject> typeTextAreas;
    [SerializeField] Text maxHpText;
    [SerializeField] Text remainHpText;


    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        //levelText.text = "Lv" + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHp);

        maxHpText.text = $"{pokemon.MaxHp}";
        remainHpText.text = $"{pokemon.HP}";

        SetType(pokemon.Base, typeTextAreas);


        
    }

    public IEnumerator UpdateHP(bool isSituation)
    {
        //hpBar.SetHP((float)_pokemon.HP / _pokemon.MaxHp);
        remainHpText.text = $"{_pokemon.HP}";
        yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHp,isSituation);
    }

    public void SetType(MonsterBase pokemonBase, List<GameObject> typetextareas)
    {
        //print("通過b");
        for (var i = 0; i < typetextareas.Count; i++)
        {
            //print("通過c");
            MonsterType forSwitchType = MonsterType.None;
            if (i == 0)
            {
                print(pokemonBase);
                print(pokemonBase.Type1);
                forSwitchType = pokemonBase.Type1;
            }
            else if (i == 1)
            {
                forSwitchType = pokemonBase.Type2;
            }

            string colorString;
            Color newColor;

            var test1 = typetextareas[i].transform;
            var test2 = typetextareas[i].transform.GetChild(0);

            Text typetext = typetextareas[i].transform.GetChild(0).GetComponent<Text>();
            switch (forSwitchType)
            {
                case MonsterType.None:
                    //もしNoneからその他にする場合にtrueにされてないから第二タイプが表記されない
                    typetextareas[i].SetActive(false);
                    break;
                case MonsterType.God:
                    typetextareas[i].SetActive(true);
                    colorString = "#7D00FF";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    typetextareas[i].GetComponent<Image>().color = newColor;
                    typetext.text = "GOD";
                    break;
                case MonsterType.Human:
                    typetextareas[i].SetActive(true);
                    colorString = "#FFC5AB"; 
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    typetextareas[i].GetComponent<Image>().color = newColor;
                    typetext.text = "HUM";
                    break;
                case MonsterType.Sun:
                    typetextareas[i].SetActive(true);
                    colorString = "#FF4F00";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    typetextareas[i].GetComponent<Image>().color = newColor;
                    typetext.text = "SUN";
                    break;
                case MonsterType.Earth:
                    typetextareas[i].SetActive(true);
                    colorString = "#19FFE5";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    typetextareas[i].GetComponent<Image>().color = newColor;
                    typetext.text = "EAR";
                    break;
                case MonsterType.Moon:
                    typetextareas[i].SetActive(true);
                    colorString = "#FFFF00";
                    ColorUtility.TryParseHtmlString(colorString, out newColor);
                    typetextareas[i].GetComponent<Image>().color = newColor;
                    typetext.text = "MOO";
                    break;


            }

        }

        //void SetHPText()
        //{
            
        //}
    }
}
