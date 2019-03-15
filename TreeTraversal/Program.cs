using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeTraversal
{
    class Program
    {
        public class Node
        {
            protected int depth = 0;
            protected Node parent = null;

            protected const int BoxWidth = 15;
            protected const int HorizontalSpaceBetweenBoxes = 8;

            public Node()
            {
            }

            public Node(string name)
            {
                Name = name;
            }

            public void AddChild(Node child)
            {
                child.parent = this;
                Children.Add(child);
            }

            public string Name { get; }
            public List<Node> Children { get; } = new List<Node>();
            public string Render(bool isFirstChild, bool isLastChild)
            {
                if (this.GetType() == typeof(Root)) return "";

                var displayName = Name.Substring(0, Math.Min(BoxWidth, Name.Length));

                var xPosition = depth * BoxWidth + depth * HorizontalSpaceBetweenBoxes;
                var horizontalSpaceBefore = isFirstChild ? Math.Min(xPosition, 6) : xPosition;
                var prefix = IsUnattached() ? "" : "------";
                var displayPrefix = Spaces(horizontalSpaceBefore - prefix.Length) + prefix;

                var verticalSpaceBefore = isFirstChild ? "" : Newlines(1);
                var verticalSpaceAfter = isLastChild ? Newlines(2) : "";
                return verticalSpaceBefore + displayPrefix + $"|{displayName,-BoxWidth}|" + verticalSpaceAfter;
            }

            private bool IsUnattached() => parent == null || parent.GetType() == typeof(Root);
            private string Spaces(int n) => new String(' ', n);
            private string Newlines(int n) => new String('\n', n);
        }

        public class Root : Node
        {
        }

        public class Department : Node
        {
            public Department(string name) : base(name) { depth = 0; }
        }

        public class Group : Node
        {
            public Group(string name) : base(name) { depth = 1; }
        }

        public class Team : Node
        {
            public Team(string name) : base(name) { depth = 2; }
        }

        static void Main(string[] args)
        {
            var attachedItemsParent = new Root();
            var dept1 = new Department("Department 1");
            var dept2 = new Department("Department 2");
            var group1 = new Group("Group 1");
            var group2 = new Group("Group 2");
            var group3 = new Group("Group 3");
            attachedItemsParent.AddChild(dept1);
            attachedItemsParent.AddChild(dept2);
            dept1.AddChild(group1);
            dept1.AddChild(group2);
            dept1.AddChild(group3);
            group1.AddChild(new Team("Team 1"));
            group1.AddChild(new Team("Team 2"));
            group1.AddChild(new Team("Team 3"));
            group1.AddChild(new Team("Team 4"));
            group1.AddChild(new Team("Team 5"));
            group2.AddChild(new Team("Team 6"));
            group2.AddChild(new Team("Team 7"));
            group2.AddChild(new Team("Team 8"));

            var unattachedItemsParent = new Root();
            var group4 = new Group("Unattached Group 1");
            group4.AddChild(new Team("Team 9"));
            group4.AddChild(new Team("Team 10"));
            unattachedItemsParent.AddChild(new Root()); // todo: workaround for "first child", think about this
            unattachedItemsParent.AddChild(group4);
            unattachedItemsParent.AddChild(new Team("Unattached Team 1"));
            unattachedItemsParent.AddChild(new Team("Unattached Team 2"));

            Render(attachedItemsParent);
            Render(unattachedItemsParent);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Hit Enter to Exit");
            Console.ReadLine();
        }

        private static void Render(Node current, bool isFirstChild = false, bool isLastChild = false)
        {
            RenderNode(current, isFirstChild, isLastChild);
            foreach (var child in current.Children)
            {
                Render(child, child == current.Children.First(), child == current.Children.Last());
            }
        }

        private static void RenderNode(Node node, bool isFirstChild, bool isLastChild)
        {
            Console.Write(node.Render(isFirstChild, isLastChild));
        }
    }
}
