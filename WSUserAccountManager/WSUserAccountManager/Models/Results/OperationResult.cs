using System.Collections.Generic;
using System.Linq;

namespace WSUserAccountManager.Models
{
    public class OperationResult
    {
        public OperationResult()
        {
            Errors = new List<string>();
            Result = new Dictionary<string, object>();
        }

        public bool Success { get { return !Errors.Any(); } }

        public List<string> Errors { get; }

        public Dictionary<string, object> Result { get; set; }

        public void AddError(string errMessage)
        {
            Errors.Add(errMessage);
        }

        public void AddResult(string property, object value)
        {
            Result.Add(property, value);
        }
    }
}