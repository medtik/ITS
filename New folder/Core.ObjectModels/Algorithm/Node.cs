using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Core.ObjectModels.Algorithm
{
    public class Node
    {
        public int Key;
        public bool IsRoot;
        public Collection<int> Data;
        public Collection<Node> Childs;

        public Node(int tagIds)
        {
            Key = tagIds;
            Data = new Collection<int>();
            Childs = new Collection<Node>();
            IsRoot = false;
        }

        public Node()
        {
            IsRoot = true;
            Childs = new Collection<Node>();
        }

        public void AddNode(Queue<int> tagIds, int locationId)
        {
            if (!IsRoot)
            {
                Data.Add(locationId);
            }

            if (tagIds.Count > 0)
            {
                int tagId = tagIds.Dequeue();
                Node nodeToAdd = Childs.SingleOrDefault(node => tagId == node.Key);
                if (nodeToAdd == null)
                {
                    nodeToAdd = new Node(tagId);
                    Childs.Add(nodeToAdd);
                }

                nodeToAdd.AddNode(tagIds, locationId);
            }
        }

        public void Search(Queue<int> tagIds, Collection<int> resultLocationIds)
        {
            if (tagIds.Count > 0)
            {
                int tagId = tagIds.Dequeue();
                foreach (Node node in Childs)
                {
                    if (node.Key == tagId)
                    {
                        foreach (int locationId in node.Data)
                        {
                            resultLocationIds.Add(locationId);
                        }
                        node.Search(tagIds,resultLocationIds);
                    }
                }
            }
        }

        public override string ToString()
        {
            return JObject.FromObject(this).ToString();
        }
    }
}