// Flax Engine scripting API

// -----------------------------------------------------------------------------
// Original code from fastJSON project. https://github.com/mgholam/fastJSON
// Greetings to Mehdi Gholam
// -----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Text;

namespace fastJSON
{
    internal sealed class JSONSerializer
    {
        private readonly Dictionary<object, int> _cirobj = new Dictionary<object, int>();
        private int _current_depth;
        private readonly int _MAX_DEPTH;
        private readonly StringBuilder _output = new StringBuilder();
        private readonly JSONParameters _params;

        private readonly bool _useEscapedUnicode;

        internal JSONSerializer(JSONParameters param)
        {
            _params = param;
            _MAX_DEPTH = 8;
            _useEscapedUnicode = _params.UseEscapedUnicode;
            _MAX_DEPTH = _params.SerializerMaxDepth;
        }

        internal string ConvertToJSON(object obj)
        {
            WriteValue(obj, true);

            return _output.ToString();
        }

        private void WriteValue(object obj, bool isRoot = false)
        {
            if ((obj == null) || obj is DBNull)
                _output.Append("null");

            else if (obj is string || obj is char)
                WriteString(obj.ToString());

            else if (obj is Guid)
                WriteGuid((Guid)obj);

            else if (obj is bool)
                _output.Append((bool)obj ? "true" : "false");// conform to standard

            else if (
                obj is int || obj is long ||
                obj is decimal ||
                obj is byte || obj is short ||
                obj is sbyte || obj is ushort ||
                obj is uint || obj is ulong
            )
                _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));

            else if (obj is double)
            {
                var d = (double)obj;
                if (double.IsNaN(d))
                    _output.Append("\"NaN\"");
                else
                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
            }
            else if (obj is float)
            {
                var d = (float)obj;
                if (float.IsNaN(d))
                    _output.Append("\"NaN\"");
                else
                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
            }

            else if (obj is DateTime)
                WriteDateTime((DateTime)obj);

            else if (obj is DateTimeOffset)
                WriteDateTimeOffset((DateTimeOffset)obj);

            else if ((_params.KVStyleStringDictionary == false) && obj is IDictionary &&
                     obj.GetType().IsGenericType && (obj.GetType().GetGenericArguments()[0] == typeof(string)))

                WriteStringDictionary((IDictionary)obj);
            else if ((_params.KVStyleStringDictionary == false) && obj is ExpandoObject)
                WriteStringDictionary((IDictionary<string, object>)obj);
            else if (obj is IDictionary)
                WriteDictionary((IDictionary)obj);

            else if (obj is DataSet)
                WriteDataset((DataSet)obj);

            else if (obj is DataTable)
                WriteDataTable((DataTable)obj);

            else if (obj is byte[])
                WriteBytes((byte[])obj);

            else if (obj is StringDictionary)
                WriteSD((StringDictionary)obj);

            else if (obj is NameValueCollection)
                WriteNV((NameValueCollection)obj);

            else if (obj is IEnumerable)
                WriteArray((IEnumerable)obj);

            else if (obj is Enum)
                WriteEnum((Enum)obj);

            else if (Reflection.Instance.IsTypeRegistered(obj.GetType()))
                WriteCustom(obj);

            else
            {
                // Special case for Flax objects
                var linkedObj = obj as FlaxEngine.Object;
                if (isRoot == false && linkedObj != null)
                {
                    WriteGuid(linkedObj.id);
                }
                else
                {
                    WriteObject(obj);
                }
            }
        }

        private void WriteDateTimeOffset(DateTimeOffset d)
        {
            write_date_value(d.DateTime);
            _output.Append(" ");
            if (d.Offset.Hours > 0)
                _output.Append("+");
            else
                _output.Append("-");
            _output.Append(d.Offset.Hours.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append(":");
            _output.Append(d.Offset.Minutes);

            _output.Append('\"');
        }

        private void WriteNV(NameValueCollection nameValueCollection)
        {
            _output.Append('{');

            var pendingSeparator = false;

            foreach (string key in nameValueCollection)
                if ((_params.SerializeNullValues == false) && (nameValueCollection[key] == null))
                {
                }
                else
                {
                    if (pendingSeparator)
                        _output.Append(',');
                    WritePair(key, nameValueCollection[key]);
                    pendingSeparator = true;
                }
            _output.Append('}');
        }

        private void WriteSD(StringDictionary stringDictionary)
        {
            _output.Append('{');

            var pendingSeparator = false;

            foreach (DictionaryEntry entry in stringDictionary)
                if ((_params.SerializeNullValues == false) && (entry.Value == null))
                {
                }
                else
                {
                    if (pendingSeparator)
                        _output.Append(',');

                    var k = (string)entry.Key;
                    WritePair(k, entry.Value);
                    pendingSeparator = true;
                }
            _output.Append('}');
        }

        private void WriteCustom(object obj)
        {
            Serialize s;
            Reflection.Instance._customSerializer.TryGetValue(obj.GetType(), out s);
            WriteStringFast(s(obj));
        }

        private void WriteEnum(Enum e)
        {
            // FEATURE : optimize enum write
            if (_params.UseValuesOfEnums)
                WriteValue(Convert.ToInt32(e));
            else
                WriteStringFast(e.ToString());
        }

        private void WriteGuid(Guid g)
        {
            if (_params.UseFastGuid == false)
                WriteStringFast(g.ToString());
            else
                WriteBytes(g.ToByteArray());
        }

        private void WriteBytes(byte[] bytes)
        {
            WriteStringFast(Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None));
        }

        private void WriteDateTime(DateTime dateTime)
        {
            // datetime format standard : yyyy-MM-dd HH:mm:ss
            var dt = dateTime;
            if (_params.UseUTCDateTime)
                dt = dateTime.ToUniversalTime();

            write_date_value(dt);

            if (_params.UseUTCDateTime)
                _output.Append('Z');

            _output.Append('\"');
        }

        private void write_date_value(DateTime dt)
        {
            _output.Append('\"');
            _output.Append(dt.Year.ToString("0000", NumberFormatInfo.InvariantInfo));
            _output.Append('-');
            _output.Append(dt.Month.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append('-');
            _output.Append(dt.Day.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append('T');// strict ISO date compliance 
            _output.Append(dt.Hour.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append(':');
            _output.Append(dt.Minute.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append(':');
            _output.Append(dt.Second.ToString("00", NumberFormatInfo.InvariantInfo));
            if (_params.DateTimeMilliseconds)
            {
                _output.Append('.');
                _output.Append(dt.Millisecond.ToString("000", NumberFormatInfo.InvariantInfo));
            }
        }

        private void WriteDataset(DataSet ds)
        {
            _output.Append('{');

            var tablesep = false;
            foreach (DataTable table in ds.Tables)
            {
                if (tablesep)
                    _output.Append(',');
                tablesep = true;
                WriteDataTableData(table);
            }
            // end dataset
            _output.Append('}');
        }

        private void WriteDataTableData(DataTable table)
        {
            _output.Append('\"');
            _output.Append(table.TableName);
            _output.Append("\":[");
            var cols = table.Columns;
            var rowseparator = false;
            foreach (DataRow row in table.Rows)
            {
                if (rowseparator)
                    _output.Append(',');
                rowseparator = true;
                _output.Append('[');

                var pendingSeperator = false;
                foreach (DataColumn column in cols)
                {
                    if (pendingSeperator)
                        _output.Append(',');
                    WriteValue(row[column]);
                    pendingSeperator = true;
                }
                _output.Append(']');
            }

            _output.Append(']');
        }

        private void WriteDataTable(DataTable dt)
        {
            _output.Append('{');

            WriteDataTableData(dt);

            // end datatable
            _output.Append('}');
        }

        private void WriteObject(object obj)
        {
            int i;
            if (_cirobj.TryGetValue(obj, out i) == false)
            {
                _cirobj.Add(obj, _cirobj.Count + 1);
            }
            else
            {
                if (_current_depth > 0)
                {
                    // Circular references are not supported due to performance and Spaghetti Code!
                    _output.Append("null");
                    return;
                }
            }
            _output.Append('{');

            _current_depth++;
            if (_current_depth > _MAX_DEPTH)
                throw new Exception("Serializer encountered maximum depth of " + _MAX_DEPTH);

            var t = obj.GetType();
            var append = false;

            var fields = Reflection.Instance.GetFields(t);
            var length = fields.Length;
            for (var ii = 0; ii < length; ii++)
            {
                var field = fields[ii];

                var o = field.GetValue(obj);
                if ((_params.SerializeNullValues == false) && ((o == null) || o is DBNull))
                {
                    //append = false;
                }
                else
                {
                    if (append)
                        _output.Append(',');
                    WritePair(field.Name, o);
                    append = true;
                }
            }
            _output.Append('}');
            _current_depth--;
        }

        private void WritePair(string name, object value)
        {
            WriteString(name);

            _output.Append(':');

            WriteValue(value);
        }

        private void WriteArray(IEnumerable array)
        {
            _output.Append('[');

            var pendingSeperator = false;

            foreach (var obj in array)
            {
                if (pendingSeperator)
                    _output.Append(',');

                WriteValue(obj);

                pendingSeperator = true;
            }
            _output.Append(']');
        }

        private void WriteStringDictionary(IDictionary dic)
        {
            _output.Append('{');

            var pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
                if ((_params.SerializeNullValues == false) && (entry.Value == null))
                {
                }
                else
                {
                    if (pendingSeparator)
                        _output.Append(',');

                    var k = (string)entry.Key;
                    WritePair(k, entry.Value);
                    pendingSeparator = true;
                }
            _output.Append('}');
        }

        private void WriteStringDictionary(IDictionary<string, object> dic)
        {
            _output.Append('{');
            var pendingSeparator = false;
            foreach (var entry in dic)
                if ((_params.SerializeNullValues == false) && (entry.Value == null))
                {
                }
                else
                {
                    if (pendingSeparator)
                        _output.Append(',');
                    var k = entry.Key;

                    WritePair(k, entry.Value);
                    pendingSeparator = true;
                }
            _output.Append('}');
        }

        private void WriteDictionary(IDictionary dic)
        {
            _output.Append('[');

            var pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
            {
                if (pendingSeparator)
                    _output.Append(',');
                _output.Append('{');
                WritePair("k", entry.Key);
                _output.Append(",");
                WritePair("v", entry.Value);
                _output.Append('}');

                pendingSeparator = true;
            }
            _output.Append(']');
        }

        private void WriteStringFast(string s)
        {
            _output.Append('\"');
            _output.Append(s);
            _output.Append('\"');
        }

        private void WriteString(string s)
        {
            _output.Append('\"');

            var runIndex = -1;
            var l = s.Length;
            for (var index = 0; index < l; ++index)
            {
                var c = s[index];

                if (_useEscapedUnicode)
                {
                    if ((c >= ' ') && (c < 128) && (c != '\"') && (c != '\\'))
                    {
                        if (runIndex == -1)
                            runIndex = index;

                        continue;
                    }
                }
                else
                {
                    if ((c != '\t') && (c != '\n') && (c != '\r') && (c != '\"') && (c != '\\'))
                        // && c != ':' && c!=',')
                    {
                        if (runIndex == -1)
                            runIndex = index;

                        continue;
                    }
                }

                if (runIndex != -1)
                {
                    _output.Append(s, runIndex, index - runIndex);
                    runIndex = -1;
                }

                switch (c)
                {
                    case '\t':
                        _output.Append("\\t");
                        break;
                    case '\r':
                        _output.Append("\\r");
                        break;
                    case '\n':
                        _output.Append("\\n");
                        break;
                    case '"':
                    case '\\':
                        _output.Append('\\');
                        _output.Append(c);
                        break;
                    default:
                        if (_useEscapedUnicode)
                        {
                            _output.Append("\\u");
                            _output.Append(((int)c).ToString("X4", NumberFormatInfo.InvariantInfo));
                        }
                        else
                            _output.Append(c);

                        break;
                }
            }

            if (runIndex != -1)
                _output.Append(s, runIndex, s.Length - runIndex);

            _output.Append('\"');
        }
    }
}
