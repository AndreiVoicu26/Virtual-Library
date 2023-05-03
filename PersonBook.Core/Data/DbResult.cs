using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonBook.Core.Data
{
    public class DbResult
    {
        public bool Success { get; set; }
        public string? Reason { get; set; }
        public Guid Id { get; set; }
        public static DbResult Succeed() => new() { Success = true };
        public static DbResult Succeed(Guid Id) => new() { Success = true, Id = Id };
        public static DbResult Fail() => new() { Success = false };
        public static DbResult Fail(string Reason) => new() { Success = false, Reason = Reason };
        public static DbResult Fail(Exception ex) => new() { Success = false, Reason = ex.Message };

    }
}
