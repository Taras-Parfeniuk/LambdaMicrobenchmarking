using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaMicrobenchmarking
{
    public class Table
    {
        public int EntriesCount
        {
            get
            {
                return _EntriesCount;
            }
        }

        private int _EntriesCount;
        private List<String[]> Entries;
        private readonly int ColumnCount;
        private String EntryStringFormat;

        public Table(params String[] columnNames)
        {
            Entries = new List<string[]>();
            Entries.Add(columnNames);
            ColumnCount = columnNames.Length;
            EntryStringFormat = "";
            _EntriesCount = 0;
        }

        public void AddEntry(params object[] values)
        {
            String[] entry = new String[ColumnCount];

            for (int i = 0; i < values.Length || i < ColumnCount; i++)
            {
                entry[i] = values[i].ToString();
            }
            Entries.Add(entry);
            _EntriesCount++;
        }

        public void AddEntry(params string[] formatedValues)
        {
            String[] entry = new String[ColumnCount];

            for (int i = 0; i < formatedValues.Length || i < ColumnCount; i++)
            {
                entry[i] = formatedValues[i];
            }
            Entries.Add(entry);
            _EntriesCount++;
        }

        private IEnumerable<String> GetColumnByName(String value)
        {
            List<String> Column = new List<String>();
            String[] header = Entries[0];
            int n = -1;
            for (int i = 0; i < ColumnCount; i++)
            {
                if (header[i] == value)
                    n = i;
            }
            return GetColumn(n);
        }

        private IEnumerable<String> GetColumn(int columnId)
        {
            List<String> Column = new List<String>();
            String[] header = Entries[0];
            if (columnId < 0 || columnId >= ColumnCount)
                return null;

            foreach (String[] e in Entries)
            {
                Column.Add(e[columnId]);
            }
            return Column;
        }

        private int GetColumnWidth(IEnumerable<String> column)
        {
            List<String> Column = column.ToList<String>();

            int maxWidth = 0;

            foreach (String entry in Column)
            {
                if (entry.Length > maxWidth)
                    maxWidth = entry.Length;
            }
            return maxWidth;
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
            EntryStringFormat = "{0,  -" + GetColumnWidth(0) + "}\t ";

            for (int i = 1; i < ColumnCount; i++)
            {
                EntryStringFormat += "{" + i + ", " + GetColumnWidth(i) + "} ";
            }
        }

        public void WriteHead()
        {
            SetEntryStringFormat();
            Console.WriteLine(EntryStringFormat, Entries[0]);
        }

        public void WriteEntries()
        {
            SetEntryStringFormat();
            foreach (string[] e in Entries)
            {
                if (e != Entries[0])
                {
                    Console.WriteLine(EntryStringFormat, e);
                }
            }
        }
    }
}
