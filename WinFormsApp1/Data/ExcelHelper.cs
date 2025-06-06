using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp1.Data
{
    public static class ExcelHelper
    {
        public static void SaveDataTableToExcel(DataTable dataTable, string fileName = "Отчет")
        {
            try
            {
                // Настройка пути
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fullPath = Path.Combine(desktopPath, $"{fileName}_{DateTime.Now:yyyy-MM-dd_HHmmss}.xlsx");

                // Настройка Excel
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(fullPath)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Данные");

                    // Заголовки
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1].Value = dataTable.Columns[col].ColumnName;
                        worksheet.Cells[1, col + 1].Style.Font.Bold = true;
                    }

                    // Данные
                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                        }
                    }

                    // Авто-форматирование
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    package.Save();
                }

                MessageBox.Show($"Файл сохранен:\n{fullPath}", "Успех",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}
