namespace API.ITSProject_2.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract(Name = "Area")]
    public class CreateAreaViewModels
    {
        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IList<int> Questions { get; set; }
    }

    [DataContract(Name = "Area")]
    public class EditAreaViewModels
    {
        [Required]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IList<int> Questions { get; set; }
    }

    [DataContract(Name = "Area")]
    public class AreaDetailViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string CoverPhoto { get; set; }

        [DataMember]
        public ICollection<CategoriesLocationCounter> Locations { get; set; }

        [DataMember]
        public ICollection<FeaturedPlanViewModels> FeaturedPlan { get; set; }//top 10 rating then review count

        [DataMember]
        public ICollection<PlanLocationViewModels> FeaturedLocation { get; set; }//top 10 rating then review count
    }

    [DataContract(Name = "Categories")]
    public class CategoriesLocationCounter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Counter { get; set; }
    }

    [DataContract(Name = "Area")]
    public class AreaViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int LocationCount { get; set; }

        [DataMember]
        public int QuestionCount { get; set; }
    }

    [DataContract(Name = "Area")]
    public class AreaFeatureViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int LocationCount { get; set; }

        [DataMember]
        public int PlanCount { get; set; }

        [DataMember]
        public string Photo { get; set; }
    }
}