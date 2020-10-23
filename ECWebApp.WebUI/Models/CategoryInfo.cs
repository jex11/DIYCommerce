using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models
{
    public class CategoryInfo
    {
        [Required(ErrorMessage = "Category Name cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Category Name:")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Category Description cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Category Description:")]
        public string CategoryDescription { get; set; }

        public Guid FolderID { get; set; }
    }
}