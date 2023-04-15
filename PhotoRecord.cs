using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoToCSV
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    [IgnoreEmptyLines]
    public class PhotoRecord : IComparable
    {
        [FieldQuoted]
        public string FilePath { get; set; } = "";

        // For empty fields, the datetime will be set to 2000/01/01 00:00:00.
        // That means they should sort at the beginning.
        [FieldConverter(ConverterKind.Date, "yyyy/MM/dd HH:mm:ss")]
        [FieldNullValue(typeof(DateTime), "2000/01/01 00:00:00")]
        public DateTime DateTimeOriginal { get; set; } = new DateTime();

        [FieldQuoted]
        public string Keywords { get; set; } = "";

        [FieldQuoted]
        public string Description { get; set; } = "";

        // These fields do not exist in the input file.
        // We set their value when processing the file.
        [FieldHidden]
        public string CamName { get; set; } = "";

        [FieldHidden]
        public bool Dup { get; set; } = false;

        [FieldHidden]
        public int? MaleCount { get; set; } = null;

        [FieldHidden]
        public int? FemaleCount { get; set; } = null;

        [FieldHidden]
        public int? JuvenileCount { get; set; } = null;

        [FieldHidden]
        public int? UnknownCount {  get; set; } = null;

        [FieldHidden]
        public string? Direction { get; set; }

        [FieldHidden]
        public List<string> Species { get; set; } = new List<string>();

        int IComparable.CompareTo(object? obj)
        {
            if (obj == null)
                return -1;
            PhotoRecord record = (PhotoRecord)obj;
            if (this.CamName != record.CamName)
                return this.CamName.CompareTo(record.CamName);
            else
                return this.DateTimeOriginal.CompareTo(record.DateTimeOriginal);

        }
    }
}
