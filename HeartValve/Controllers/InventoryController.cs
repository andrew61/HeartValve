using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Web.Mvc;


namespace HeartValve.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InventoryController : ApplicationController
    {
        //GET: Inventory
        public ActionResult AddEquipment()
        {
            return View();
        }
        public ActionResult AssignEquipment()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult Inventory_Read([DataSourceRequest]DataSourceRequest request)
        //{

        //    var faqs = db.GetvailableInventory().ToList();
        //    return Json(faqs.AsQueryable().ToDataSourceResult(request, r => r), JsonRequestBehavior.AllowGet);

        //}

        //[HttpPost]
        //public ActionResult Inventory_Create([DataSourceRequest]DataSourceRequest request, FAQ_GetResult type)
        //{

        //    int? faqID = null;
        //    int? order = null;
        //    db.FAQ_Insert(type.question, type.answer, ref faqID, ref order);

        //    type.faqID = (int)faqID;
        //    type.order = (int)order;


        //    return Json(new[] { type }.ToDataSourceResult(request));
        //}

        //[HttpPost]
        //public ActionResult Inventory_Update([DataSourceRequest]DataSourceRequest request, FAQ_GetResult type)
        //{

        //    db.FAQ_Update(type.faqID, type.order, type.question, type.answer);


        //    return Json(new[] { type }.ToDataSourceResult(request));
        //}

        //[HttpPost]
        //public ActionResult Inventory_Destroy([DataSourceRequest]DataSourceRequest request, FAQ_GetResult type)
        //{

        //    db.FAQ_Delete(type.faqID);


        //    return Json(new[] { type }.ToDataSourceResult(request));
        //}

        //[HttpPost]
        //public void Inventory_ChangeOrder(int faqID, int targetOrder)
        //{

        //    db.FAQ_ChangeOrder(faqID, targetOrder);
        //}

    }


}