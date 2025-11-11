using ClosedXML.Excel;
using StudentManagement.Models;
using System.IO;

namespace StudentManagement.Services
{
    public class ExcelExportService
    {
        public async Task<bool> ExportGradesToExcelAsync(List<Grade> grades, string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Grades");
                    
                    // Headers
                    worksheet.Cell(1, 1).Value = "Student Name";
                    worksheet.Cell(1, 2).Value = "Course Name";
                    worksheet.Cell(1, 3).Value = "Grade Type";
                    worksheet.Cell(1, 4).Value = "Value";
                    worksheet.Cell(1, 5).Value = "Letter Grade";
                    worksheet.Cell(1, 6).Value = "Notes";
                    worksheet.Cell(1, 7).Value = "Updated By";
                    worksheet.Cell(1, 8).Value = "Updated At";
                    
                    // Style headers
                    var headerRange = worksheet.Range(1, 1, 1, 8);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    
                    // Data
                    int row = 2;
                    foreach (var grade in grades)
                    {
                        worksheet.Cell(row, 1).Value = grade.StudentName ?? "";
                        worksheet.Cell(row, 2).Value = grade.CourseName ?? "";
                        worksheet.Cell(row, 3).Value = grade.GradeType;
                        worksheet.Cell(row, 4).Value = grade.Value?.ToString() ?? "";
                        worksheet.Cell(row, 5).Value = grade.LetterGrade ?? "";
                        worksheet.Cell(row, 6).Value = grade.Notes ?? "";
                        worksheet.Cell(row, 7).Value = grade.UpdatedByName ?? "";
                        worksheet.Cell(row, 8).Value = grade.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                        row++;
                    }
                    
                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();
                    
                    workbook.SaveAs(filePath);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}

