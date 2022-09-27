using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Dtos
{
    public class WisdomDto
    {
        public double DoubleType { get; set; }
        public int Int32Type { get; set; }
        public int? NullableInt32Type { get; set; }
        public string StringType { get; set; }
        public List<string> StringListType { get; set; }
        public bool BoolType { get; set; }
        public byte[] ByteListTypeCustom { get; set; }
        public CategoryDto CategoryModel { get; set; }
        public Dictionary<string, string> DictionaryModel { get; set; }
        public DateTime TimeStampTypeCustom { get; set; }
        public TimeSpan DurationTypeCustom { get; set; }
        public ProductDto Product { get; set; }

    }
}
