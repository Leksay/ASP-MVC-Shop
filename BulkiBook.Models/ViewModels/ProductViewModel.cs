﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkiBook.Models.ViewModels;

public class ProductViewModel
{
    public Product Product { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> CoverTypeList { get; set; }
}
