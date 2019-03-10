//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AdvantShop.Diagnostics;

namespace AdvantShop.Helpers
{
    public class XmlHelper
    {
        public static string SerializeObject<T>(object o)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                var xs = new XmlSerializer(typeof(T));
                using (var xtw = new XmlTextWriter(ms, Encoding.UTF8))
                {
                    xs.Serialize(xtw, o);
                    ms = (MemoryStream)xtw.BaseStream;
                    return StringHelper.UTF8ByteArrayToString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
            }

            return string.Empty;
        }

        public static T DeserializeObject<T>(string xml)
        {
            MemoryStream ms = null;
            try
            {
                var xs = new XmlSerializer(typeof(T));
                ms = new MemoryStream(StringHelper.StringToUTF8ByteArray(xml));
                using (var xtw = new XmlTextWriter(ms, Encoding.UTF8))
                    return (T)xs.Deserialize(ms);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
            }
            return default(T);
        }
    }
}