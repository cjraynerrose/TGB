using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGB.Domain.Data
{
    /// <summary>
    /// For lowercasing GUIDs for Sqlite.
    /// </summary>
    public class GuidToTextConverter : ValueConverter<Guid, string>
    {

        public GuidToTextConverter()
        : base(guid => guid.ToString().ToLower(), text => Guid.Parse(text)) { }
        public GuidToTextConverter(ConverterMappingHints mappingHints = null)
            : base(guid => guid.ToString().ToLower(), text => Guid.Parse(text), mappingHints)
        {
        }


    }
}
