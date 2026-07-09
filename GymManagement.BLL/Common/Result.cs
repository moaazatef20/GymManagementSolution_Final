using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Common
{
    public record Result(bool Success,string? ErrorMassage = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result Ok() => new Result(true);
        public static Result Fail(string errorMassage ,ResultKind kind = ResultKind.Conflict) => new Result(false,errorMassage,kind);
        public static Result NotFound(string errorMassage = "Not Found") => new Result(false, errorMassage, ResultKind.NotFound);
        public static Result Validation(string errorMassage) => new Result(false, errorMassage, ResultKind.ValidationFaild);

    }
    public record Result<T>(bool Success , T? value, string? ErrorMassage = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new(true,value);
        public static Result<T> Fail(string errorMassage, ResultKind kind = ResultKind.Conflict) => new (false,default, errorMassage, kind);
        public static Result<T> NotFound(string errorMassage = "Not Found") => new (false, default,errorMassage, ResultKind.NotFound);
        public static Result<T> Validation(string errorMassage) => new (false, default,errorMassage, ResultKind.ValidationFaild);

    }
    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        ValidationFaild,
        Forbidden
    }
}
