using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DS.Windows;
using DS.Utilities;

public class DialogueSystemEditor : EditorWindow
{
    [MenuItem("Windows/DialogueSystem/Graph")]
    public static void ShowExample()
    {
        DialogueSystemEditor wnd = GetWindow<DialogueSystemEditor>();
        wnd.titleContent = new GUIContent("Dialogues Graph");
    }

    public void CreateGUI()
    {
        AddGraphView();
        AddToolbar();

        AddStyles();
    }

    private void AddToolbar()
    {
        Toolbar toolbar = new Toolbar();

        TextField fileName = DSElementUtility.CreateTextField("DialoguesSaveFile", "Save File Name:");

        Button saveButton = DSElementUtility.CreateButton("SAVE");

        toolbar.Add(fileName);
        toolbar.Add(saveButton);

        rootVisualElement.Add(toolbar);
    }

    private void AddGraphView()
    {
        DialoguesGraph DM = new DialoguesGraph();

        DM.StretchToParentSize();

        rootVisualElement.Add(DM);
    }

    private void AddStyles()
    {
        StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DSGraphStyles.uss");

        rootVisualElement.styleSheets.Add(styleSheet);
    }
}