using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using RefineryPrism.Models;

namespace RefineryPrism.DataAccessLayer
{
    public class DataWriter : IDataWriter
    {
        public void WriteReport(string path, IEnumerable<WorkPart> parties)
        {
            File.Delete(path);

            var fileInfo = new FileInfo(path);

            using (var package = new ExcelPackage(fileInfo))
            {
                var ws = package.Workbook.Worksheets.Add("Temp");

                ws.Cells.LoadFromDataTable(CreateDataTable(parties), true, TableStyles.None);
                ws.Column(1).Width = 10;
                ws.Column(2).Width = 20;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;

                package.Save();
            }
        }

        private static DataTable CreateDataTable(IEnumerable<WorkPart> parts)
        {
            DataTable table = new DataTable();

            const string part = "Партия";
            const string equipment = "Оборудование";
            const string startTime = "Время начала";
            const string endTime = "Время окончания";

            table.Columns.Add(new DataColumn(part));
            table.Columns.Add(new DataColumn(equipment));
            table.Columns.Add(new DataColumn(startTime));
            table.Columns.Add(new DataColumn(endTime));

            foreach (var workPart in parts)
            {
                var row = table.NewRow();

                row[part] = workPart.Part.Id;
                row[equipment] = workPart.Equipment.Name;
                row[startTime] = workPart.StartTime;
                row[endTime] = workPart.EndTime;

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
