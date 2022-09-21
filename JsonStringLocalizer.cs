using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading;

namespace Json_Based_Localization.Web
{
    public class JsonStringLocalizer : IStringLocalizer  
    {
        private  JsonSerializer _serializer= new JsonSerializer();

        public JsonStringLocalizer()
        {
        }

        //public JsonStringLocalizer( serializer)
        //{
        //    _serializer = serializer;
        //}

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name,value);
            }

        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualvalue= this[name];
                return !actualvalue.ResourceNotFound? new LocalizedString(name,actualvalue): actualvalue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json"; //Thread.CurrentThread.CurrentCulture.Name return name of language Name now that use 

            using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new StreamReader(stream);
            using JsonTextReader reader = new JsonTextReader(streamReader);
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    continue;
                var key = reader.Value as string;
                reader.Read();
                var value= _serializer.Deserialize<string>(reader);
                yield return new LocalizedString(key,value);
            }
            

        }

        private string GetString( string Key)
        {
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json"; //Thread.CurrentThread.CurrentCulture.Name return name of language Name now that use 
            var FullfilePath = Path.GetFullPath(filePath);
            if (File.Exists(FullfilePath))
            {
                var result = GetValue(Key,FullfilePath);
                return result;
            }

            return string.Empty;
        }
        private string GetValue(string propertyName,string filePath)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filePath))
                return string.Empty;

            using FileStream stream=  new FileStream(filePath, FileMode.Open, FileAccess.Read,FileShare.Read);
            using StreamReader streamReader= new StreamReader(stream);
            using JsonTextReader reader= new JsonTextReader(streamReader);

            while (reader.Read())
            {
                if (reader.TokenType==JsonToken.PropertyName&& reader.Value as string==propertyName  )
                {
                    reader.Read();
                    if(_serializer != null)
                    {
                        return _serializer.Deserialize<string>(reader);

                    }
                    return string.Empty;
                }

               
            }

            return string.Empty;
        }
    }
}
