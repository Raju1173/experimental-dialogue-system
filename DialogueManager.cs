using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [ReorderableList]
    public List<TreeGroup> TreeGroups = new List<TreeGroup>();

    [Space(20)]
    public List<FactorList> Characters = new List<FactorList>();

    [Space(20)]
    public string CurrentDialogue;
    public List<string> CurrentChoices;

    [Space(20)]
    public int CurrentCharacter = 0;
    public int CurrentGroup = 0;
    public int CurrentTree = 0;

    public static DialogueManager Instance;

    public void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        RefreshDialogue();
    }

    public void MoveTo(int ChoiceIndex)
    {
        TreeGroups[CurrentGroup].Trees[CurrentTree].Move(ChoiceIndex);

        RefreshDialogue();
    }

    public void JumpTo(int NodeID)
    {
        TreeGroups[CurrentGroup].Trees[CurrentTree].Jump(NodeID);

        RefreshDialogue();
    }

    public void RefreshDialogue()
    {
        CurrentDialogue = TreeGroups[CurrentGroup].Trees[CurrentTree].CurrentNode.Dialogue;

        CurrentChoices.Clear();

        foreach (Choice choice in TreeGroups[CurrentGroup].Trees[CurrentTree].CurrentNode.Choices)
        {
            CurrentChoices.Add(choice.ChoiceName);
        }
    }

    public bool CompareConditions(Condition[] values)
    {
        foreach(Condition val in values)
        {
            if (val.ComparisonOperator == ">" || val.ComparisonOperator == ">=")
            {
                if (Characters[val.CharacterID].Factors[val.FactorID].FactorValue >= val.CompareValue)
                    continue;

                else
                    return false;
            }

            if (val.ComparisonOperator == "<" || val.ComparisonOperator == "<=")
            {
                if (Characters[val.CharacterID].Factors[val.FactorID].FactorValue <= val.CompareValue)
                    continue;

                else
                    return false;
            }
        }

        return true;
    }
}

[System.Serializable]
public class TreeGroup
{
    public string GroupName;
    [Space(20), ReorderableList]
    [AllowNesting]
    public List<DialogueTree> Trees = new List<DialogueTree>();
}

[System.Serializable]
public class FactorList
{
    public string CharacterName;
    public Image CharacterImage;
    [Space(20)]
    public List<FactorDict> Factors = new List<FactorDict>();
}

[System.Serializable]
public class FactorDict
{
    public string FactorName;
    [Range(0f, 1f)]
    public float FactorValue = 0f;
}