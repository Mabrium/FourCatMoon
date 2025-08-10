using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillText", menuName = "CatData/SkillText", order = int.MinValue)]
public class SkillText : ScriptableObject
{
    public string[] skillName;
    //[TextArea(4, 10)]
    public string[] skillExplain;
}
