using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace MultiTasks.RT
{
    public class MtJson : IEventEmitter
    {
        private EventEmitter _eventEmitter = new EventEmitter();

        private bool _started = false;

        #region IEventEmitter

        void IEventEmitter.Raise(string eventName, object[] args)
        {
            (_eventEmitter as IEventEmitter).Raise(eventName, args);
        }

        void IEventEmitter.On(string eventName, Action<object[]> action)
        {
            (_eventEmitter as IEventEmitter).On(eventName, action);
        }
        
        #endregion

        private void Read(JsonTextReader reader)
        {
            var hasStuff = reader.Read();
            if (hasStuff)
            {
                if (!_started)
                {
                    _started = true;
                    (this as IEventEmitter).Raise("start", new object[] { this });                    
                }

                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:

                        break;
                    case JsonToken.EndObject:

                        break;
                    case JsonToken.StartArray:

                        break;
                    case JsonToken.EndArray:

                        break;
                    case JsonToken.Boolean:

                        break;
                    case JsonToken.Bytes:

                        break;
                    case JsonToken.Date:

                        break;
                    case JsonToken.Float:

                        break;
                    case JsonToken.Integer:

                        break;
                    case JsonToken.PropertyName:

                        break;
                    case JsonToken.Null:

                        break;
                    case JsonToken.Raw:

                        break;
                    case JsonToken.String:

                        break;                   
                }

                Read(reader);
            }
            else
            {
                reader.Close();
                (this as IEventEmitter).Raise("end", new object[]{ this });
            }
        }

        public void Read(String s)
        {
            var reader = new JsonTextReader(new StringReader(s));           
            Read(reader);
        }


    }
}
