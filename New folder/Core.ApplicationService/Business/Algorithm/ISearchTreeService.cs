using System.Collections.Generic;
using Core.ObjectModels.Algorithm;
using Core.ObjectModels.Entities;

namespace Core.ApplicationService.Business.Algorithm
{
    public interface ISearchTreeService
    {
        Tree BuildTree(ICollection<Location> locations);
        ICollection<int> SearchTree(ICollection<Tag> tags, Tree tree);

        void WriteTree(string path, Tree tree);
        Tree ReadTree(string path);
    }
}