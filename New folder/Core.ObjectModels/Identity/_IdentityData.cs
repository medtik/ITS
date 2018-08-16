namespace Core.ObjectModels.Identity
{
    using System.Collections.Generic;

    public class _IdentityData
    {
        public object Data { get; set; }

        private List<string> _errors;

        public List<string> Errors => _errors ?? (_errors = new List<string>());

        public bool IsError => Errors.Count > 0;
    }
}