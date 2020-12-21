using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using OfficeOpenXml;
using RefineryPrism.Models;

namespace RefineryPrism.DataAccessLayer
{
    public class DataReader : IDataReader
    {
        private readonly Logger _logger;

        public DataReader()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public IEnumerable<Equipment> ReadEquipments(string path)
        {
            var equipments = new List<Equipment>();

            using (var package = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    package.Load(stream);
                }

                var workSheet = package.Workbook.Worksheets.First();

                for (int i = 2; i <= workSheet.Dimension.End.Row; i++)
                {
                    var equipment = new Equipment
                    {
                        Id = (int)(double)workSheet.Cells[i, 1].Value,
                        Name = (string)workSheet.Cells[i, 2].Value
                    };

                    equipments.Add(equipment);
                }

                if (equipments.Count != workSheet.Dimension.Rows - 1)
                {
                    _logger.Error("Считаны не все строки, на вход поданы некорретные данные");

                    return null;
                }
            }

            return equipments;
        }

        public IEnumerable<Nomenclature> ReadNomenclatures(string path)
        {
            var nomenclatures = new List<Nomenclature>();

            using (var package = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    package.Load(stream);
                }

                var workSheet = package.Workbook.Worksheets.First();

                for (int i = 2; i <= workSheet.Dimension.End.Row; i++)
                {
                    var nomenclature = new Nomenclature
                    {
                        Id = (int)(double)workSheet.Cells[i, 1].Value,
                        Name = (string)workSheet.Cells[i, 2].Value
                    };

                    nomenclatures.Add(nomenclature);
                }

                if (nomenclatures.Count != workSheet.Dimension.Rows - 1)
                {
                    _logger.Error("Считаны не все строки, на вход поданы некорретные данные");

                    return null;
                }
            }

            return nomenclatures;
        }

        public IEnumerable<Part> ReadParts(string path)
        {
            var parts = new List<Part>();

            using (var package = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    package.Load(stream);
                }

                var workSheet = package.Workbook.Worksheets.First();

                for (int i = 2; i <= workSheet.Dimension.End.Row; i++)
                {
                    var part = new Part
                    {
                        Id = (int)(double)workSheet.Cells[i, 1].Value,
                        NomenclatureId = (int)(double)workSheet.Cells[i, 2].Value
                    };

                    parts.Add(part);
                }
                
                if (parts.Count != workSheet.Dimension.Rows - 1)
                {
                    _logger.Error("Считаны не все строки, на вход поданы некорретные данные");

                    return null;
                }
            }

            return parts;
        }

        public IEnumerable<WorkTime> ReadWorkTimes(string path)
        {
            var workTimes = new List<WorkTime>();

            using (var package = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    package.Load(stream);
                }

                var workSheet = package.Workbook.Worksheets.First();

                for (int i = 2; i <= workSheet.Dimension.End.Row; i++)
                {
                    try
                    {
                        var workTime = new WorkTime
                        {
                            EquipmentId = (int) (double) workSheet.Cells[i, 1].Value,
                            NomenclatureId = (int) (double) workSheet.Cells[i, 2].Value,
                            Time = (int) (double) workSheet.Cells[i, 3].Value
                        };

                        workTimes.Add(workTime);
                    }
                    catch (Exception e)
                    {
                        _logger.Error("Ошибка при чтении WorkTimes");
                    }
                }

                if (workTimes.Count != workSheet.Dimension.Rows - 1)
                {
                    _logger.Error("Считаны не все строки, на вход поданы некорретные данные");

                    return null;
                }
            }

            return workTimes;
        }
    }
}
