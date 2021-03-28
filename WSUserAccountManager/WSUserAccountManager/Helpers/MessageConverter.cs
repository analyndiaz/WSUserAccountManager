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
            var propertyObj = CreateMessageObj("command", operation);

            foreach (var item in value.Result)
            {
                propertyObj.Add(item.Key.ToLower(), item.Value);
            }

            propertyObj.Add("success", value.Success);

            var jsonDataObj = CreateMessageObj("jsonData", propertyObj);

            return JsonConvert.SerializeObject(jsonDataObj);
        }

        private static IDictionary<string, object> CreateMessageObj(string propertyName, object value) 
        {
            try
            {
                dynamic jsonDataObj = new ExpandoObject() as IDictionary<string, object>;
                IDictionary<string, object> jsonData = jsonDataObj;

                jsonData.Add(propertyName, value);

                return jsonDataObj;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
