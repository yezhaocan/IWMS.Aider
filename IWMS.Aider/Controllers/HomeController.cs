using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using IWMS.Aider.Models;
using IWMS.Aider.Service;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using IWMS.Aider.Filter;
using Microsoft.AspNetCore.Authorization;

namespace IWMS.Aider.Controllers
{
    [FromAuthorize]
    public class HomeController : Controller
    {
        private IUnicodeB2BService _unicodeB2BService;

        public HomeController(IUnicodeB2BService unicodeB2BService)
        {
            _unicodeB2BService = unicodeB2BService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult CheckBillNo(string billNo, string nationSku )
        {
            var bill = _unicodeB2BService.GetById<TBillBatchOut>(billNo);
            if (bill != null)
            {
                var Data = from a in _unicodeB2BService.Get<TDefStyle>()
                           join b in _unicodeB2BService.Get<TDefSku>().Where(p => p.NationSku == nationSku && !string.IsNullOrEmpty(p.NationSku))
                           on a.Style equals b.Style
                           select new {sku=b.Sku,styleName=a.StyleName };
                var entity = Data.FirstOrDefault();
                if (entity != null)
                {
                    return Json(new { Result = true, Data = entity });
                }
                else {
                    return Json(new { Result = false, Message = "唯一码扫描出错，请核实" });
                }
            }
            else
            {
                return Json(new { Result = false, Message = "订单编号错误，请核实" });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SubmitUniCode(string param)
        {
            try
            {
                List<TBillUnicodeB2BRecord> entitys = new List<TBillUnicodeB2BRecord>();
                List<B2BModel> list = JsonConvert.DeserializeObject<List<B2BModel>>(param);
                foreach (var item in list)
                {

                    TBillUnicodeB2BRecord billUnicodeB2BRecord = new TBillUnicodeB2BRecord()
                    {
                        BillId = item.BillId,
                        Operator = "",
                        OperDate = DateTime.Now,
                        Qty = 1,
                        Sku = item.Sku,
                        UniCode = item.UniCode
                    };
                    entitys.Add(billUnicodeB2BRecord);
                }
                _unicodeB2BService.InsertRange(entitys);
                return Json(new { Result = true });
            }
            catch (Exception e)
            {
                return Json(new { Result = false,Message=e.Message  });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SearchList(string startTime, string endTime, string sku, string uniCode,int pageIndex=1)
        {
            try
            {
                var pageSize = 20;
                var data = from a in _unicodeB2BService.Get<TBillUnicodeB2BRecord>()
                           .Where(p => (string.IsNullOrEmpty(uniCode) || p.UniCode == uniCode)
                           && (string.IsNullOrEmpty(sku) || p.Sku == sku)
                           && (string.IsNullOrEmpty(startTime) || Convert.ToDateTime(startTime) <= p.OperDate)
                           && (string.IsNullOrEmpty(endTime) || Convert.ToDateTime(endTime).AddDays(1) >= p.OperDate)
                           )
                           join b in _unicodeB2BService.Get<TBillBatchOut>()
                           on a.BillId equals b.BillId
                           join c in _unicodeB2BService.Get<TDefSku>()
                           on a.Sku equals c.Sku
                           join d in _unicodeB2BService.Get<TDefStyle>()
                           on c.Style equals d.Style
                           join e in _unicodeB2BService.Get<TDefWareHouse>()
                           on b.CKID equals e.WHId
                           orderby a.Operator
                           select new
                           {
                               billId = a.BillId,
                               uniCode = a.UniCode,
                               ckName = e.WHName,
                               sku = a.Sku,
                               styleName = d.StyleName,
                               style = d.Style,
                               OperDate = a.OperDate.ToString("yyyy-MM-dd hh:mm")
                           };
                var totalCount = data.Count();
                var pageCount = (totalCount - 1) / pageSize+1;
                return Json(new { Result = true, Data = data.Skip((pageIndex-1)* pageSize).Take(pageSize).ToList(), PageCount= pageCount });
            }
            catch (Exception e)
            {

                return Json(new { Result = false, Message = e.Message });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExportList(string startTime, string endTime, string sku, string uniCode)
        {
            var data = from a in _unicodeB2BService.Get<TBillUnicodeB2BRecord>()
                              .Where(p => (string.IsNullOrEmpty(uniCode) || p.UniCode == uniCode)
                              && (string.IsNullOrEmpty(sku) || p.Sku == sku)
                              && (string.IsNullOrEmpty(startTime) || Convert.ToDateTime(startTime) <= p.OperDate)
                              && (string.IsNullOrEmpty(endTime) || Convert.ToDateTime(endTime).AddDays(1) >= p.OperDate)
                              )
                       join b in _unicodeB2BService.Get<TBillBatchOut>()
                       on a.BillId equals b.BillId
                       join c in _unicodeB2BService.Get<TDefSku>()
                       on a.Sku equals c.Sku
                       join d in _unicodeB2BService.Get<TDefStyle>()
                       on c.Style equals d.Style
                       join e in _unicodeB2BService.Get<TDefWareHouse>()
                       on b.CKID equals e.WHId
                       orderby a.Operator
                       select new B2BListModel()
                       {
                           BillId = a.BillId,
                           UniCode = a.UniCode,
                           CkName = e.WHName,
                           Sku = a.Sku,
                           StyleName = d.StyleName,
                           Style = d.Style,
                           OperDate = a.OperDate.ToString("yyyy-MM-dd")
                       };

            DataTable dt = new DataTable("B2B唯一码");
            dt.Columns.Add("订单编号", Type.GetType("System.String"));
            dt.Columns.Add("唯一码", Type.GetType("System.String"));
            dt.Columns.Add("发货仓", Type.GetType("System.String"));
            dt.Columns.Add("Sku", Type.GetType("System.String"));
            dt.Columns.Add("品名", Type.GetType("System.String"));
            dt.Columns.Add("款号", Type.GetType("System.String"));
            dt.Columns.Add("唯一码录入时间", Type.GetType("System.String"));

            DataRow dr0 = dt.NewRow();
            dr0["订单编号"] = "订单编号";
            dr0["唯一码"] = "唯一码";
            dr0["发货仓"] = "发货仓";
            dr0["Sku"] = "Sku";
            dr0["品名"] = "品名";
            dr0["款号"] = "款号";
            dr0["唯一码录入时间"] = "唯一码录入时间";
            dt.Rows.Add(dr0);
            int xnum = 0; //序号
            foreach (var item in data.ToList())
            {
                xnum++;
                DataRow dr = dt.NewRow();
                dr["订单编号"] = item.BillId;
                dr["唯一码"] = item.UniCode;
                dr["发货仓"] = item.CkName;
                dr["Sku"] = item.Sku;
                dr["品名"] = item.StyleName;
                dr["款号"] = item.Style;
                dr["唯一码录入时间"] = item.OperDate;
                dt.Rows.Add(dr);
            }
            var fileContents = ExportToXLSReturnMemoryStream(dt);

            return File(fileContents, "application/ms-excel", "B2B唯一码导出.xls");//转换成excel
        }
        public static MemoryStream ExportToXLSReturnMemoryStream(DataTable table)
        {
            MemoryStream ms = new MemoryStream();
            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();
                if (table.TableName == "" || table.TableName == null)
                {
                    table.TableName = "sheet0";
                }
                ISheet sheet = workbook.CreateSheet(table.TableName);
                int rowIndex = 0;
                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }
    }
}
