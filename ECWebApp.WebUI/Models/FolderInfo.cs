using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models
{
    public class FolderInfo
    {
        public System.Guid ProductFolderId { get; set; }

        [Required(ErrorMessage = "Folder Name cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Folder Name:")]
        public string ProductFolderName { get; set; }

        [Required(ErrorMessage = "Folder Description cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Folder Description:")]
        public string ProductFolderDescription { get; set; }
        public System.Guid ProductFolderFrom { get; set; }

    }
}