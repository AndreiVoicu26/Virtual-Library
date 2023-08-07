using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonBook.Core.Repositories;

namespace PersonBook.Web.Areas.Person.Pages
{
    public class ExcelModel : PageModel
    {
        private readonly IPersonRepository _personRepository;

        public ExcelModel(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public FileResult OnGet()
        {
            var data = _personRepository.GetPersons();

            using var workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Persons");
            worksheet.Cell(1, 1).Value = "First Name";
            worksheet.Cell(1, 2).Value = "Last Name";
            worksheet.Cell(1, 3).Value = "Age";
            worksheet.Cell(1, 4).Value = "Borrowed Books";

            IXLRange range = worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(1, 4).Address);
            range.Style.Fill.SetBackgroundColor(XLColor.Almond);

            int index = 1;

            foreach (var item in data.Result)
            {
                index++;

                worksheet.Cell(index, 1).Value = item.FirstName;
                worksheet.Cell(index, 2).Value = item.LastName;
                worksheet.Cell(index, 3).Value = (DateTime.Now - item.DateOfBirth.ToDateTime(TimeOnly.MinValue)).Days / 365;

                var books = _personRepository.GetBorrowedBooks(item.Id);
                string booksString = "";
                foreach (var book in books.Result)
                {
                    booksString += book.Title + " by " + book.Author + ", ";
                }
                if (booksString.Length > 0)
                {
                    booksString = booksString.Substring(0, booksString.Length - 2);
                }

                worksheet.Cell(index, 4).Value = booksString;
            }

            for (var i = 1; i < 5; i++)
            {
                worksheet.Column(i).AdjustToContents();
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var strTime = DateTime.Now.ToString();
            string filename = string.Format($"Persons_{strTime}.xlsx");

            return File(content, contentType, filename);
        }

    }
}
