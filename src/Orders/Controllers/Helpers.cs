using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Controllers
{
    public static class Helpers
    {
        public const int MAX_LIMIT_ON_PAGE = 50;
        public const int MIN_LIMIT_ON_PAGE = 1;
        public const int DEFAULT_PAGE = 0;

        public static void CorrectPageLimitValues(ref int page, ref int limit)
        {
            if (page < 0)
                page = DEFAULT_PAGE;
            if (limit > MAX_LIMIT_ON_PAGE)
                limit = MAX_LIMIT_ON_PAGE;
            if (limit < MIN_LIMIT_ON_PAGE)
                limit = MIN_LIMIT_ON_PAGE;
        }

        public static Dictionary<string, string[]> FormatModelErrors(this ModelStateDictionary modelState)
        {
            return modelState.ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
        }
    }
}
