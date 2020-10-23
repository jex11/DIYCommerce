using ECWebApp.Domain;
using ECWebApp.Domain.Abstract;
using ECWebApp.WebUI.Areas.Admin.Models;
using ECWebApp.WebUI.Areas.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWebApp.WebUI.Areas.Admin.Controllers
{
    public class Admin_TailorController : Controller
    {
        private ICustomerRepository CustomerRepository;

        public Admin_TailorController(ICustomerRepository _CustomerRepository)
        {
            this.CustomerRepository = _CustomerRepository;
        }

        // GET: Admin/Admin_Tailor
        public ActionResult Index()
        {
            TailorViewModel output = new TailorViewModel();
            output.TailorsInfo = new List<TailorDetails>();
            List<TailorDetails> tailorDetailList = new List<TailorDetails>();
            var lstTailors = CustomerRepository.TailorsInfoList();
            foreach (var tailor in lstTailors)
            {
                TailorDetails tailorInfo = new TailorDetails();
                tailorInfo.TailorId = tailor.TailorID;
                tailorInfo.TailorName = tailor.CustomerFirstName + ' ' + tailor.CustomerLastName;
                tailorInfo.TailorEmail = tailor.CustomerEmail;
                tailorInfo.Specialization = tailor.Specialization;
                tailorInfo.AvgRating = tailor.AverageRating;
                tailorInfo.AvgElapsedTime = tailor.AverageElapsedDay;
                tailorInfo.OrderInHand = tailor.OrderInHand;
                tailorInfo.OrderDone = tailor.OrderDone;
                tailorInfo.GoodReviews = tailor.GoodRev;
                tailorInfo.BadReviews = tailor.BadRev;
                tailorInfo.Commission = tailor.Commission;
                //List<string> keywordlst = new List<string>();
                //List<OrderAssignment> oa = CustomerRepository.GetOrderAssignments(tailor.TailorID);
                //foreach (var oassign in oa)
                //{
                //    if(oassign.Keywords != null)
                //        keywordlst.Add(oassign.Keywords);
                //}
                //tailorInfo.Comments = string.Join(", ", keywordlst);
                tailorDetailList.Add(tailorInfo);
            }
            output.TailorsInfo = tailorDetailList;
            return View(output);            
        }

        public ActionResult GoodReviews(Guid TailorID)
        {
            ReviewsViewModel output = new ReviewsViewModel();
            List<vw_ReviewsDetails> oalist = new List<vw_ReviewsDetails>();
            oalist = CustomerRepository.GetLikesOrderAssignment(TailorID);
            output.OrderAssignmentList = oalist;
            return View(output);
        }

        public ActionResult BadReviews(Guid TailorID)
        {            
            ReviewsViewModel output = new ReviewsViewModel();
            List<vw_ReviewsDetails> oalist = new List<vw_ReviewsDetails>();
            oalist = CustomerRepository.GetDislikesOrderAssignment(TailorID);
            output.OrderAssignmentList = oalist;
            return View(output);
        }
    }
}