using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using GraphElements;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DialoguesGraph : GraphView
    {
        public DialoguesGraph()
        {
            AddBackground();

            AddManipulators();

            AddStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port => { 
                if (startPort == port)
                    return;

                if(startPort.node == port.node)
                    return;

                if(startPort.direction == port.direction)
                    return;

                compatiblePorts.Add(port);
            } );

            return compatiblePorts;
        }

        private void AddBackground()
        {
            GridBackground Background = new GridBackground();

            Background.StretchToParentSize();

            Insert(0, Background);
        }

        private GraphDialogueNode CreateNode(Vector2 Position)
        {
            GraphDialogueNode node = new GraphDialogueNode();

            node.Initialize(this, nodes.ToList().Count != 0 ? int.Parse(nodes.Last().title) + 1 : 0, Position);
            node.Draw();

            return node;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale + 1000f);

            this.AddManipulator(CreateNodeContextualMenu());
            this.AddManipulator(CreateGroupContextualMenu());

            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(new ContentDragger());
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextMenuManipulator = new ContextualMenuManipulator(
                    menuEvent => menuEvent.menu.AppendAction("Add Tree", actionEvent => AddElement(CreateGroup("NEW TREE",actionEvent.eventInfo.localMousePosition)))
                );

            return contextMenuManipulator;
        }

        private Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title
            };

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            foreach(GraphElement selectedElement in selection)
            {
                if(!(selectedElement is GraphDialogueNode))
                {
                    continue;
                }

                GraphDialogueNode node = (GraphDialogueNode) selectedElement;

                group.AddElement(node);
            }

            return group;
        }

        private IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator contextMenuManipulator = new ContextualMenuManipulator(
                    menuEvent => menuEvent.menu.AppendAction("Add Dialogue", actionEvent => AddElement(CreateNode(actionEvent.eventInfo.mousePosition)))
                );

            return contextMenuManipulator;
        }

        private void AddStyles()
        {
            StyleSheet GraphStyleSheet = (StyleSheet)EditorGUIUtility.Load("DSGraphStyles.uss");
            StyleSheet NodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("DSNodeStyles.uss");

            styleSheets.Add(GraphStyleSheet);
            styleSheets.Add(NodeStyleSheet);
        }
    }
}
