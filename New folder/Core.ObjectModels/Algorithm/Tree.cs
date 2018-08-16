using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core.ObjectModels.Entities;
using Newtonsoft.Json;

namespace Core.ObjectModels.Algorithm
{
    public class Tree
    {
        public Node Root = null;

        public Tree(ICollection<Location> locations)
        {
            Root = new Node();
            foreach (Location location in locations)
            {
                if (location.Tags == null) continue;

                int[] tagIds = location.Tags
                    .OrderBy(tag => tag.Name)
                    .Select(tag => tag.Id)
                    .ToArray();

                Collection<Queue<int>> branchs = CreateTagQueues(tagIds);

                foreach (Queue<int> branch in branchs)
                {
                    Root.AddNode(branch, location.Id);
                }
            }
        }

        private Tree(Node root)
        {
            Root = root;
        }

        public ICollection<int> Search(ICollection<Tag> tags)
        {
            int[] tagIds = tags.Select(tag => tag.Id).ToArray();
            Collection<Queue<int>> tagQueues = CreateTagQueues(tagIds);

            Collection<int> locationIds = new Collection<int>();
            foreach (Queue<int> tagQueue in tagQueues)
            {
                Root.Search(tagQueue, locationIds);
            }

            return locationIds;
        }

        public Collection<Queue<int>> CreateTagQueues(ICollection<int> tagIds)
        {
            Collection<Queue<int>> tagQueues = new Collection<Queue<int>>();
            for (int i = 0; i < tagIds.Count; i++)
            {
                Queue<int> tagsQueue = new Queue<int>(tagIds.Skip(i));
                tagQueues.Add(tagsQueue);
            }

            return tagQueues;
        }

        public override string ToString()
        {
            return Root.ToString();
        }

        public static Tree FromString(string jsonString)
        {
            Node root = JsonConvert.DeserializeObject<Node>(jsonString);
            return new Tree(root);
        }
    }
}