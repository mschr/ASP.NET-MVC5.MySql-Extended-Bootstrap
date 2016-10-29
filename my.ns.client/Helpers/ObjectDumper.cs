using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace my.ns.client.Helpers
{
    public class ObjectDumper
    {
        private int _level;
        private readonly int _indentSize;
        private readonly StringBuilder _stringBuilder;
        private readonly List<int> _hashListOfFoundElements;
        private BindingFlags flags = BindingFlags.Default;
        private bool flattened = false;
        private ObjectDumper(int indentSize)
        {
            _indentSize = indentSize;
            _stringBuilder = new StringBuilder();
            _hashListOfFoundElements = new List<int>();
        }

        public static string Dump(object element, bool flattened = false, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            return Dump(element, 2, flattened, flags);
        }

        public static string Dump(object element, int indentSize, bool flattened = false, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {

            var instance = new ObjectDumper(indentSize);
            instance.flags = flags;
            instance.flattened = flattened;
            return instance.DumpElement(element);
        }

        private string DumpElement(object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                Write(FormatValue(element));
            }
            else
            {
                var objectType = element.GetType();
                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    Write("{{{0}}}", objectType.FullName);
                    _hashListOfFoundElements.Add(element.GetHashCode());
                    _level++;
                }

                var enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            _level++;
                            DumpElement(item);
                            _level--;
                        }
                        else
                        {
                            if (!AlreadyTouched(item))
                                DumpElement(item);
                            else
                                Write("{{{0}}} <-- bidirectional reference found", item.GetType().FullName);
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(flags);
                    foreach (var memberInfo in members)
                    {
                        var fieldInfo = memberInfo as FieldInfo;
                        var propertyInfo = memberInfo as PropertyInfo;

                        if (fieldInfo == null && propertyInfo == null)
                            continue;

                        var type = fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;
                        try
                        {
                            object value = fieldInfo != null
                                               ? fieldInfo.GetValue(element)
                                               : propertyInfo.GetValue(element, null);

                            if (type.IsValueType || type == typeof(string))
                            {
                                Write("{0}: {1}", memberInfo.Name, FormatValue(value));
                            }
                            else if (!flattened)
                            {
                                var isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);
                                Write("{0}: {1}", memberInfo.Name, isEnumerable ? "..." : "{ }");

                                var alreadyTouched = !isEnumerable && AlreadyTouched(value);
                                _level++;
                                if (!alreadyTouched)
                                    DumpElement(value);
                                else
                                    Write("{{{0}}} <-- bidirectional reference found", value.GetType().FullName);
                                _level--;
                            }
                        }
                        catch (Exception e)
                        {
                            Write("{0}: {1}", memberInfo.Name, e.Message);
                            continue;
                        }
                    }
                }

                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    _level--;
                }
            }

            return _stringBuilder.ToString();
        }

        private bool AlreadyTouched(object value)
        {
            if (value == null)
                return false;

            var hash = value.GetHashCode();
            for (var i = 0; i < _hashListOfFoundElements.Count; i++)
            {
                if (_hashListOfFoundElements[i] == hash)
                    return true;
            }
            return false;
        }

        private void Write(string value, params object[] args)
        {
            var space = new string(' ', _level * _indentSize);

            if (args != null)
                value = string.Format(value, args);
            if (_indentSize > 0)
            {
                _stringBuilder.AppendLine(space + value);
            }
            else {
                _stringBuilder.Append(" " + value);

            }
        }

        private string FormatValue(object o)
        {
            if (o == null)
                return ("null");

            if (o is DateTime)
                return (((DateTime)o).ToShortDateString());

            if (o is string)
                return string.Format("\"{0}\"", o);

            if (o is char && (char)o == '\0')
                return string.Empty;

            if (o is ValueType)
                return (o.ToString());

            if (o is IEnumerable)
                return ("...");

            return ("{ }");
        }
    }

    internal static class DumpEnvironmentMiddleware
    {
        public static string BuildHtmlTable(Microsoft.Owin.IOwinContext context)
        {
            var builder = new StringBuilder();
            builder.Append("<table border='1'><tr><th>Key</th><th>Value</th></tr>");
            List<string> keys = context.Environment.Keys.OrderBy(key => key)
                                       .ToList();
            foreach (var key in keys)
            {
                var value = context.Environment[key];
                var valueDictionary = value as IDictionary<string, string[]>;
                if (valueDictionary == null)
                {
                    builder.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", key, value);
                }
                else {
                    builder.AppendFormat("<tr><td>{0}</td><td>count ={1}</td></tr>", key, valueDictionary.Count);
                    if (valueDictionary.Count == 0)
                    {
                        continue;
                    }
                    builder.Append("<tr><td>&nbsp;</td><td><table border='1'><tr><th>Key</th><th>Value</th></tr>");
                    List<string> valueKeys = valueDictionary.Keys.OrderBy(key2 => key2)
                                                            .ToList();
                    foreach (var valueKey in valueKeys)
                    {
                        builder.AppendFormat("<tr><td>{0}</td><td>{1}</td><tr>", valueKey, string.Join("<br />", valueDictionary[valueKey]));
                    }
                    builder.Append("</table></td></tr>");
                }
            }
            builder.Append("</table>");
            return builder.ToString();
        }
    }
}