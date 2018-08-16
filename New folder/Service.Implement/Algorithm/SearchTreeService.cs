using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.ApplicationService.Business.Algorithm;
using Core.ObjectModels.Entities;

namespace Service.Implement.Algorithm
{
    public class SearchTreeService : ISearchTreeService
    {
        public Core.ObjectModels.Algorithm.Tree BuildTree(ICollection<Location> locations)
        {
            return new Core.ObjectModels.Algorithm.Tree(locations);
        }

        public ICollection<int> SearchTree(ICollection<Tag> tags, Core.ObjectModels.Algorithm.Tree tree)
        {
            return tree.Search(tags);
        }

        public void WriteTree(string path, Core.ObjectModels.Algorithm.Tree tree)
        {
            File.WriteAllText(path, tree.ToString(), Encoding.UTF8);
        }

        public Core.ObjectModels.Algorithm.Tree ReadTree(string path)
        {
            string fileContent = File.ReadAllText(path, Encoding.UTF8);
            return Core.ObjectModels.Algorithm.Tree.FromString(fileContent);
        }
    }
}