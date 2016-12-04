using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaMicrobenchmarking
{
    public class Table
    {
        public int EntriesCount => Entries.Count;

        private List<string[]> Entries { get; set; }
        private readonly int _columnCount;
        private string _entryStringFormat;

        public Table(params string[] columnNames)
        {
            Entries = new List<string[]> {columnNames};
            _columnCount = columnNames.Length;
            _entryStringFormat = "";
        }

        public void AddEntry(params object[] values)
        {
            var entry = new string[_columnCount];

            for (var i = 0; i < values.Length || i < _columnCount; i++)
            {
                entry[i] = values[i].ToString();
            }
            Entries.Add(entry);
        }

        public void AddEntry(params string[] formatedValues)
        {
            var entry = new string[_columnCount];

            for (var i = 0; i < formatedValues.Length || i < _columnCount; i++)
            {
                entry[i] = formatedValues[i];
            }
            Entries.Add(entry);
        }

        private IEnumerable<string> GetColumnByName(string value)
        {
            var header = Entries[0];
            var n = -1;
            for (var i = 0; i < _columnCount; i++)
            {
                if (header[i] == value)
                    n = i;
            }
            return GetColumn(n);
        }

        private IEnumerable<string> GetColumn(int columnId)
        {
            if (columnId < 0 || columnId >= _columnCount)
                throw new ArgumentOutOfRangeException();

            return Entries.Select(entry => entry[columnId]).ToList();
        }

        private static int GetColumnWidth(IEnumerable<string> column)
        {
            return column.Select(entry => entry.Length).Concat(new[] { 0 }).Max();
        }

        private int GetColumnWidth(int columnId)
        {
            return GetColumnWidth(GetColumn(columnId));
        } 
        
        private int GetColumnWidth(string columnName)
        {
            return GetColumnWidth(GetColumnByName(columnName));
        }

        private void SetEntryStringFormat()
        {
            _entryStringFormat = "{0,  -" + GetColumnWidth(0) + "}\t ";

            for (var i = 1; i < _columnCount; i++)
            {
                _entryStringFormat += "{" + i + ", " + GetColumnWidth(i) + "} ";
            }
        }

        public void WriteHead()
        {
            SetEntryStringFormat();
            if (Entries != null)
                Console.WriteLine(_entryStringFormat, Entries[0]);
        }

        public void WriteEntries()
        {
            SetEntryStringFormat();
            foreach (var entry in Entries)
            {
                if (entry != Entries[0] && entry != null)
                {
                    Console.WriteLine(_entryStringFormat, entry);
                }
            }
        }
    }
}
