using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class TaskHistoryViewModel
    {
        public Guid TailorID { get; set; }
        public List<TaskHistory> TaskRecords { get; set; }
    }
}