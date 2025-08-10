using UnityEngine;

[CreateAssetMenu(fileName = "UsingCatData", menuName = "CatData/Cat", order = int.MinValue)]
public class UsingCat : ScriptableObject
{
    public int NUMBER;
    public string NAME;
    public int LEVEL;
    public int EXP;
    public int ATK;
    public int DEF;
    public int HP;
    public int MAXHP;
    public int SPEED;
    public int SKILLPOINT;
    public int SKILL1NUMBER;
    public int SKILL2NUMBER;
    public int SKILL3NUMBER;
}
