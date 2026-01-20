using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class DialogueTree
{
    public string TreeName;

    public List<DialogueNode> Dialogues;

    internal DialogueNode CurrentNode;

    internal Dictionary<int, DialogueNode> AllNodes = new Dictionary<int, DialogueNode>();

    internal DialogueNode AddNode(int ID)
    {
        DialogueTree ThisTree = this;
        AllNodes.Add(ID, new DialogueNode(ID, ref ThisTree));

        return AllNodes[AllNodes.Count - 1];
    }

    internal void RemoveNode(int ID)
    {
        if (!AllNodes.ContainsKey(ID))
            return;

        AllNodes.Remove(ID);
    }

    internal void ClearTree()
    {
        AllNodes.Clear();
    }

    internal DialogueNode Move(int IndexKey)
    {
        CurrentNode = CurrentNode.GetChoice(IndexKey);

        return CurrentNode;
    }

    internal DialogueNode Jump(int IndexKey)
    {
        if (!AllNodes.ContainsKey(IndexKey))
            return null;

        CurrentNode = AllNodes[IndexKey];

        return CurrentNode;
    }
}

[System.Serializable]
public class DialogueNode
{
    public int ID;

    private DialogueTree Tree;

    public Image CharacterImage = null;

    public string Character;

    public string Dialogue;

    public List<Choice> Choices = new List<Choice>();

    public Dictionary<int, DialogueNode> ParentDialogues = new Dictionary<int, DialogueNode>();

    public DialogueNode(int id, ref DialogueTree tree, string dialogue = null, DialogueNode Parent = null)
    {
        ID = id;

        Tree = tree;

        Dialogue = dialogue;

        if (Parent != null)
            ParentDialogues.Add(Parent.ID, Parent);
    }

    public DialogueNode AddChoice(int NodeID, string ChoiceName = "")
    {
        if (Choices.Count == 10 || !Tree.AllNodes.ContainsKey(NodeID))
            return null;

        Choices.Add(new Choice(ChoiceName, ID, ref Tree));

        Tree.AllNodes[Choices[Choices.Count - 1].Links[0].LinkedNodeID].ParentDialogues.Add(ID, this);

        return Tree.AllNodes[Choices[Choices.Count - 1].Links[0].LinkedNodeID];
    }

    public void RemoveChoice(int ChoiceKey = 0)
    {
        if (ChoiceKey < Choices.Count || ChoiceKey > Choices.Count)
            return;

        Tree.AllNodes[Choices[ChoiceKey].Links[0].LinkedNodeID].ParentDialogues.Remove(ID);

        for (int i = 0; i < Choices[ChoiceKey].Links.Count; i++)
            Tree.AllNodes[Choices[ChoiceKey].Links[i].LinkedNodeID].ParentDialogues.Remove(ID);

        Choices.RemoveAt(ChoiceKey);
    }

    public DialogueNode GetChoice(int ChoiceID = 0)
    {
        if (ChoiceID < Choices.Count || ChoiceID > Choices.Count)
            return null;

        else
        {
            for (int i = 0; i < Choices[ChoiceID].Links.Count; i++)
            {
                if (DialogueManager.Instance.CompareConditions(Choices[ChoiceID].Links[i].Conditions.ToArray()))
                    return Tree.AllNodes[Choices[ChoiceID].Links[i].LinkedNodeID];
            }

            return Tree.AllNodes[Choices[ChoiceID].Links[0].LinkedNodeID];
        }
    }
}

[System.Serializable]
public class Choice
{
    public string ChoiceName;

    public List<Link> Links = new List<Link>();

    int ParentNodeID;

    DialogueTree Tree;

    public Choice(string choiceName, int parentNodeID, ref DialogueTree tree)
    {
        ChoiceName = choiceName;
        ParentNodeID = parentNodeID;
        Tree = tree;
    }

    public void AddVariation(int NodeID)
    {
        if (!Tree.AllNodes.ContainsKey(NodeID))
            return;

        Links.Add(new Link(NodeID));

        Tree.AllNodes[Links[Links.Count - 1].LinkedNodeID].ParentDialogues.Add(ParentNodeID, Tree.AllNodes[ParentNodeID]);
    }

    public void RemoveVariation(int variationID = 0)
    {
        Tree.AllNodes[Links[variationID].LinkedNodeID].ParentDialogues.Remove(ParentNodeID);

        Links.RemoveAt(variationID);
    }
}

[System.Serializable]
public class Link
{
    public int LinkedNodeID;

    public List<Condition> Conditions = new List<Condition>();

    public Link(int NodeID, List<Condition> conditions = null)
    {
        LinkedNodeID = NodeID;
        Conditions = conditions;
    }

    public void AddCondition(int ChoiceID, int variationID, int CharacterID, int ConditionNum, string op, float value)
    {
        Conditions.Add(new Condition(CharacterID, ConditionNum, op, value));
    }

    public void RemoveCondition(int ChoiceID, int variationID, int ConditionID)
    {
        Conditions.RemoveAt(ConditionID);
    }
}

[System.Serializable]
public class Condition
{
    public int CharacterID;

    public int FactorID;

    public string ComparisonOperator;

    public float CompareValue;

    public Condition(int characterID, int factorID, string comparisonOperator, float compareValue)
    {
        CharacterID = characterID;
        FactorID = factorID;
        ComparisonOperator = comparisonOperator;
        CompareValue = compareValue;
    }
}