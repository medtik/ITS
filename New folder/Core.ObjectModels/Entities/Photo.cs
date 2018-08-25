namespace Core.ObjectModels.Entities
{
    using Firebase.Storage;
    using Nito.AsyncEx;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Photo : _BaseEntity
    {
        public string Path
        {
            get;
            set;
        }

        public int? ReviewId { get; set; }

        public int? UserId { get; set; }

        public ICollection<AreaPhoto> Areas { get; set; }

        public ICollection<LocationPhoto> Locations { get; set; }

        public User User { get; set; }

        public Review Review { get; set; }
    }
}