using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace MilkTeaECommerce.Utility
{
    public static class ExcelProvider
    {
        public static DataSet Read(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // 2. Use the AsDataSet extension method
                    var result = reader.AsDataSet();
                    return result;
                    // The result of each spreadsheet is in result.Tables
                }
            }
        }

    }
}
