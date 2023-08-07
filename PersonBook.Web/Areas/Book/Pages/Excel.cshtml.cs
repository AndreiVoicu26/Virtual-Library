using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonBook.Core.Repositories;

namespace PersonBook.Web.Areas.Book.Pages
{
    public class ExcelModel : PageModel
    { 
        private readonly IBookRepository _bookRepository;

        public ExcelModel(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public FileResult OnGet()
        {
            var data = _bookRepository.GetBooks();

            using var workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Books");
            worksheet.Cell(1, 1).Value = "Title";
            worksheet.Cell(1, 2).Value = "Author";
            worksheet.Cell(1, 3).Value = "ISBN";
            worksheet.Cell(1, 4).Value = "Year";
            worksheet.Cell(1, 5).Value = "Availability";
            worksheet.Cell(1, 6).Value = "Owners";
            for (var i = 1; i < 7; i++)
            {
                worksheet.Column(i).AdjustToContents();
            }

            IXLRange range = worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(1, 6).Address);
            range.Style.Fill.SetBackgroundColor(XLColor.Almond);

            int index = 1;

            foreach (var item in data.Result)
            {
                index++;

                worksheet.Cell(index, 1).Value = item.Title;
                worksheet.Cell(index, 2).Value = item.Author;
                worksheet.Cell(index, 3).Value = item.Isbn;
                worksheet.Cell(index, 4).Value = item.Year;

                if (item.IsAvailable)
                {
                    worksheet.Cell(index, 5).Value = "Available";
                }
                else
                {
                    worksheet.Cell(index, 5).Value = "Unavailable";
                }

                var owners = _bookRepository.GetOwnersOfBook(item.Id);
                string ownersString = "";
                foreach (var owner in owners.Result)
                {
                    ownersString += owner.FirstName + " " + owner.LastName + ", ";
                }
                if (ownersString.Length > 0)
                {
                    ownersString = ownersString.Substring(0, ownersString.Length - 2);
                }

                worksheet.Cell(index, 6).Value = ownersString;
            }

            for (var i = 1; i < 7; i++)
            {
                worksheet.Column(i).AdjustToContents();
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var strTime = DateTime.Now.ToString();
            string filename = string.Format($"Books_{strTime}.xlsx");

            return File(content, contentType, filename);
        }

    }
}
