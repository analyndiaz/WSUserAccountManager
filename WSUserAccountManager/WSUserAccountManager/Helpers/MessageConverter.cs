using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using WSUserAccountManager.Enums;
using WSUserAccountManager.Models;

namespace WSUserAccountManager.Helpers
{
    public static class MessageConverter
    {
        public static T ToModel<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string ToMessage(OperationResult value, string operation)
        {
            dynamic dynamicObj = new ExpandoObject() as IDictionary<string, object>;
            IDictionary<string, object> returnObj = dynamicObj;

            returnObj.Add("command", operation);

            foreach (var item in value.Result)
            {
                returnObj.Add(item.Key.ToLower(), item.Value);
            }

            returnObj.Add("success", value.Success);

            return JsonConvert.SerializeObject(returnObj);
        }
    }
}
