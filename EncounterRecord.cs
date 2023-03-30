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
    public class EncounterRecord : IComparable
    {
        [FieldCaption("Camera ID")]
        public string CamName { get; set; } = "";

        // For empty fields, the datetime will be set to 2000/01/01 00:00:00.
        // That means they should sort at the beginning.
        [FieldConverter(ConverterKind.Date, "yyyy/MM/dd HH:mm:ss")]
        [FieldNullValue(typeof(DateTime), "2000/01/01 00:00:00")]
        [FieldCaption("Date Time First Image")]
        public DateTime DateTimeFirstImage { get; set; } = new DateTime();

        [FieldConverter(ConverterKind.Date, "yyyy/MM/dd HH:mm:ss")]
        [FieldNullValue(typeof(DateTime), "2000/01/01 00:00:00")]
        [FieldCaption("Date Time Last Image")]
        public DateTime DateTimeLastImage { get; set; } = new DateTime();

        [FieldCaption("Duration secs")]
        [FieldQuoted]
        public string DurationSecs { get; set; } = "=Round((C2-B2)*60*60*24,0)";

        [FieldCaption("# of images")]
        public int? ImageCount { get; set; }

        [FieldCaption("Species")]
        [FieldQuoted]
        public string? Species { get; set; }

        [FieldCaption("# Male")] 
        public int? MaleCount { get; set; }

        [FieldCaption("# Female")] 
        public int? FemaleCount { get; set; }

        [FieldCaption("# Juvenile")] 
        public int? JuvenileCount { get; set; }

        [FieldCaption("# Unknown")]
        public int? UnknownCount { get; set; }

        [FieldCaption("Total # of Animals")]
        [FieldQuoted]
        public string TotalCount { get; set; } = "=sum(G2:J2)";

        [FieldCaption("Direction of Travel (Toward or Away from River)")]
        public string? Direction { get; set; }

        [FieldCaption("Notes")]
        public string? Notes { get; set; }


        int IComparable.CompareTo(object? obj)
        {
            if (obj == null)
                return -1;
            EncounterRecord record = (EncounterRecord)obj;
            if (this.CamName != record.CamName)
                return this.CamName.CompareTo(record.CamName);
            else
                return this.DateTimeFirstImage.CompareTo(record.DateTimeFirstImage);

        }
    }
}
