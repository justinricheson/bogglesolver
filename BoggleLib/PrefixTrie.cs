using System;
using System.Collections.Generic;

namespace BoggleLib
{
    public class PrefixTrie
    {
        private Node _root;

        public PrefixTrie()
        {
            _root = new Node();
        }

        public bool WordStartsWith(string fragment)
        {
            if (string.IsNullOrEmpty(fragment))
                return false;

            var n = _root;
            foreach (char c in fragment)
            {
                string key = c.ToString();
                if (n.Nodes.ContainsKey(key))
                    n = n.Nodes[key];
                else return false;
            }

            return true;
        }
        public void AddWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return;

            var n = _root;
            foreach (char c in word)
            {
                string key = c.ToString();
                if (!n.Nodes.ContainsKey(key))
                    n.Nodes.Add(key, new Node());

                n = n.Nodes[key];
            }
        }

        class Node
        {
            public Dictionary<string, Node> Nodes { get; private set; }

            public Node()
            {
                Nodes = new Dictionary<string, Node>(
                    StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
