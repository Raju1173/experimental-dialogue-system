using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.GraphElements
{
    using DS.Windows;
    using Utilities;

    public class GraphDialogueNode : Node
    {
        public int DialogueID { get; set; }

        public string Dialogue { get; set; }

        public List<string> Choices { get; set; }

        public DialoguesGraph Graph;

        public void Initialize(DialoguesGraph graph, int ID, Vector2 Position)
        {
            Graph = graph;

            DialogueID = ID;
            Dialogue = "";
            Choices = new List<string> { "CHOICE" };

            SetPosition(new Rect(Position, Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public void Draw()
        {
            this.title = DialogueID.ToString();

            Port InputPort = this.CreatePort("CONNECTIONS", direction:Direction.Input, capacity:Port.Capacity.Multi);

            InputPort.portColor = new Color(0, 1, 1, 1);

            inputContainer.Add(InputPort);

            Button addChoiceButton = DSElementUtility.CreateButton("ADD CHOICE", () =>
            {
                if (Choices.Count < 10)
                {
                    Port choicePort = this.CreatePort();

                    choicePort.portColor = Color.red;

                    Button deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
                    {
                        if (choicePort.connected)
                            Graph.DeleteElements(choicePort.connections);

                        Choices.Remove(choicePort.portName);
                        Graph.RemoveElement(choicePort);
                    });

                    deleteChoiceButton.AddToClassList("ds-node__button");

                    TextField choiceTextField = DSElementUtility.CreateTextField("CHOICE");

                    choiceTextField.AddToClassList("ds-node__textfield");
                    choiceTextField.AddToClassList("ds-node__choice-textfield");
                    choiceTextField.AddToClassList("ds-node__textfield__hidden");

                    choicePort.Add(choiceTextField);
                    choicePort.Add(deleteChoiceButton);

                    Choices.Add("CHOICE");

                    outputContainer.Add(choicePort);
                }
            });

            addChoiceButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceButton);

            foreach(string choice in Choices)
            {
                if (Choices.Count < 10)
                {
                    Port choicePort = this.CreatePort();

                    choicePort.portColor = Color.red;

                    Button deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
                    {
                        if (choicePort.connected)
                            Graph.DeleteElements(choicePort.connections);

                        Choices.Remove(choice);
                        Graph.RemoveElement(choicePort);
                    });

                    deleteChoiceButton.AddToClassList("ds-node__button");

                    TextField choiceTextField = DSElementUtility.CreateTextField(choice);

                    choiceTextField.AddToClassList("ds-node__textfield");
                    choiceTextField.AddToClassList("ds-node__choice-textfield");
                    choiceTextField.AddToClassList("ds-node__textfield__hidden");

                    choicePort.Add(choiceTextField);
                    choicePort.Add(deleteChoiceButton);

                    outputContainer.Add(choicePort);
                }
            }

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout DialogueFoldout = DSElementUtility.CreateFoldout("DIALOGUE");

            TextField DialogueTextField = DSElementUtility.CreateTextArea(Dialogue);

            DialogueTextField.AddToClassList("ds-node__textfield");
            DialogueTextField.AddToClassList("ds-node__quote-textfield");
            DialogueTextField.AddToClassList("ds-node__textfield");

            DialogueFoldout.Add(DialogueTextField);

            customDataContainer.Add(DialogueFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }
    }
}
