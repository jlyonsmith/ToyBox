using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace ToyBox
{
    public static class PropertyListReaderV1
    {
        private static string dictAtom;
        private static string arrayAtom;
		private static string int32Atom;
        private static string dateTimeAtom;
        private static string timeSpanAtom;
        private static string booleanAtom;
        private static string plistAtom;

        public static PropertyList ReadXml(XmlReader reader)
        {
            Dictionary<string, object> dict;

            dictAtom = reader.NameTable.Add("Dictionary");
            arrayAtom = reader.NameTable.Add("Array");
            int32Atom = reader.NameTable.Add("Int32");
            dateTimeAtom = reader.NameTable.Add("DateTime");
            timeSpanAtom = reader.NameTable.Add("TimeSpan");
            booleanAtom = reader.NameTable.Add("Boolean");
            plistAtom = reader.NameTable.Add("PropertyList");

            reader.MoveToContent();
            dict = ReadPropertyList(reader);

            return new PropertyList(dict);
        }

        private static Dictionary<string, object> ReadPropertyList(XmlReader reader)
        {
            int format = int.Parse(reader.GetAttribute("Format"));

            if (format != 1)
                throw new XmlException("Invalid format version");

            reader.ReadStartElement(plistAtom);
            reader.MoveToContent();
            Dictionary<string, object> dict = ReadDict(reader);
            reader.ReadEndElement();
            reader.MoveToContent();

            return dict;
        }

        private static Dictionary<string, object> ReadDict(XmlReader reader)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            reader.ReadStartElement(dictAtom);
            reader.MoveToContent();

            while (true)
            {
                if (String.ReferenceEquals(reader.Name, dictAtom))
                {
                    reader.ReadEndElement();
                    reader.MoveToContent();
                    break;
                }

                string key;
                object value;

                ReadKeyValue(reader, out key, out value);
                dict.Add(key, value);
            }

            return dict;
        }

        private static List<object> ReadArray(XmlReader reader)
        {
            List<object> list = new List<object>();

            reader.ReadStartElement(arrayAtom);
            reader.MoveToContent();

            while (true)
            {
                if (String.ReferenceEquals(reader.Name, arrayAtom))
                {
                    reader.ReadEndElement();
                    reader.MoveToContent();
                    break;
                }

                string key;
                object value;

                ReadKeyValue(reader, out key, out value);

                list.Add(value);
            }

            return list;
        }

        private static void ReadKeyValue(XmlReader reader, out string key, out object value)
        {
            key = reader.GetAttribute("Key");

            Type t;

            if (String.ReferenceEquals(reader.Name, dictAtom))
            {
                value = ReadDict(reader);
                return;
            }
            else if (String.ReferenceEquals(reader.Name, arrayAtom))
            {
                value = ReadArray(reader);
                return;
            }
            else if (String.ReferenceEquals(reader.Name, int32Atom))
            {
                t = typeof(int);
            }
            else if (String.ReferenceEquals(reader.Name, dateTimeAtom))
            {
                t = typeof(DateTime);
            }
            else if (String.ReferenceEquals(reader.Name, timeSpanAtom))
            {
                t = typeof(TimeSpan);
            }
            else if (String.ReferenceEquals(reader.Name, booleanAtom))
            {
                t = typeof(Boolean);
            }
            else
            {
                t = typeof(string);
            }

            string s = reader.ReadElementContentAsString();
            reader.MoveToContent();

            if (t == typeof(Int32))
            {
                int result;

                value = (object)(Int32.TryParse(s, out result) ? result : 0);
            }
            else if (t == typeof(Boolean))
            {
                bool result;

                value = (object)(Boolean.TryParse(s, out result) ? result : false);
            }
            else if (t == typeof(DateTime))
            {
                DateTime result;

                value = (object)(DateTime.TryParse(s, null, DateTimeStyles.RoundtripKind, out result) ? result : DateTime.MinValue);
            }
            else if (t == typeof(TimeSpan))
            {
                TimeSpan result;

                value = (object)(TimeSpan.TryParse(s, null, out result) ? result : TimeSpan.MinValue);
            }
            else
            {
                value = s;
			}
        }
    }
}
