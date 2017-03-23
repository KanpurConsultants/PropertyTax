using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Customize
{
    public class Constants
    {
        public class DocumentTypeIdConstants
        {
            public static int Godown = 578;
            public static int BinLocation = 1868;
            public static int Caste = 1869;
            public static int Religion = 1870;
            public static int Area = 1871;
            public static int DiscountType = 1872;
            public static int PersonRateGroup = 583;
            public static int Dimension1 = 657;
            public static int Product = 646;
            public static int Property = 1867;
            public static int PaymentMode = 1875;
            public static int Agent = 561;
            public static int Site = 596;
            public static int Collection = 1874;
            public static int OtherCollection = 1878;
        }



        public class ProductGroupConstants
        {
            public static int ConstructionType = 1;
        }

        public class ExtensionConstants
        {
            public const string PDF = ".pdf";
            public const string JPEG = ".jpeg";
            public const string JPG = ".jpg";
            public const string PNG = ".png";
            public const string BMP = ".bmp";
            public const string XLS = ".xls";
            public const string XLSX = ".xlsx";
            public const string DOC = ".doc";
            public const string NA = "NA";
        }

        public enum FileTypeConstants
        {
            Image,
            Other
        }
    }
}
