using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PerpetuumSoft.Reporting.DOM;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {

        SqlConnection con = new SqlConnection(@"Data Source=localhost,1433;Initial Catalog=DangKyThiTuyen;User ID=SA;Password=04101997");
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}/{typeReport}")]
        public ActionResult GetFile(string id, string typeReport)
        {
            byte[] pdf = null;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "Select * from DangKyThongTin where Id = 1";
            SqlDataReader abc = cmd.ExecuteReader();
            while (abc.Read())
            {
                string col = abc["Ho"].ToString();
            }

            string path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");
            try
            {
                string json = "";
                string type = "";
                if (typeReport.Equals("DANG_KY_DU_TUYEN"))
                {
                    type = "DANG_KY_DU_TUYEN";
                    DangKyThongTin dangKyThongTin = new DangKyThongTin();
                    dangKyThongTin.Id = 0;
                    dangKyThongTin.ViTriDuTuyen = "ViTriDuTuyen";
                    dangKyThongTin.DonViDuTuyen = "DonViDuTuyen";
                    dangKyThongTin.HoTen = "Trần Văn Hậu ABC";
                    dangKyThongTin.NgaySinh = "24/05/1997";
                    dangKyThongTin.MaDuThi = "ABC";
                    dangKyThongTin.GioiTinh = true;
                    dangKyThongTin.DanToc = "Kinh";
                    dangKyThongTin.TonGiao = "TC";
                    dangKyThongTin.CMND = "163363955";
                    dangKyThongTin.NgayCapCMNDString = DateTime.Now.ToString("dd/MM/yyyy");
                    dangKyThongTin.NoiCapCMND = "NAM DINH";
                    dangKyThongTin.DienThoaiLienHe = "0123456789";
                    dangKyThongTin.Email = "hauvantran97@gmail.com";
                    dangKyThongTin.QueQuan = "Nam Dinh";
                    dangKyThongTin.HoKhauThuongTru = "Nam Dinh";
                    dangKyThongTin.ChoOHienNay = "Ha Dong Ha Noi";
                    dangKyThongTin.SucKhoe = "Tot";
                    dangKyThongTin.ChieuCao = "123";
                    dangKyThongTin.CanNang = "123";
                    dangKyThongTin.BanThanHienNay = "ABC";
                    dangKyThongTin.TrinhDoVanHoa = "BCD";
                    dangKyThongTin.TrinhDoChuyenMon = "BCD";
                    dangKyThongTin.LoaiHinhDaoTao = "LoaiHinhDaoTao";
                    dangKyThongTin.NgayDangKyString = ".........., ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month.ToString() + " " + DateTime.Now.Year.ToString();

                    json = JsonConvert.SerializeObject(dangKyThongTin, Newtonsoft.Json.Formatting.None,
                                                                                        new JsonSerializerSettings
                                                                                        {
                                                                                            NullValueHandling = NullValueHandling.Ignore
                                                                                        });
                }
                else
                {
                    type = "PHIEU_HEN";
                    PhieuHen phieuHen = new PhieuHen();
                    phieuHen.id = 0;
                    phieuHen.MaHoSo = "ABC";
                    phieuHen.HoVaTen = "Tran Van Hau";
                    phieuHen.NgaySinh = "24/05/1997";
                    phieuHen.CMND = "163363955";
                    phieuHen.DienThoai = "123456789";
                    phieuHen.Email = "hvt@gmail.com";
                    phieuHen.ViTriDuTuyen = "ViTriDuTuyen";
                    phieuHen.DonViDuTuyen = "DonViDuTuyen";
                    phieuHen.NgayHenNopHoSo = DateTime.Now.ToString("dd/MM/yyyy");


                    json = JsonConvert.SerializeObject(phieuHen, Newtonsoft.Json.Formatting.None,
                                                                                        new JsonSerializerSettings
                                                                                        {
                                                                                            NullValueHandling = NullValueHandling.Ignore
                                                                                        });
                }
                string base64 = Utils.Base64Encode(json);
                var process = Process.Start(path + "\\pdf\\App.exe", type + " " + base64 + " " + id);
                Console.WriteLine(path);
                if (process == null) // failed to start
                {
                    //return InternalServerError();
                }
                else // Started, wait for it to finish
                {
                    process.WaitForExit();
                    pdf = System.IO.File.ReadAllBytes(path + "\\pdf\\" + id + "value.pdf");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //return response;
            byte[] byteArray = null;
            if (System.IO.File.Exists(path + "\\pdf\\" + id + "value.pdf"))
            {
                byteArray = System.IO.File.ReadAllBytes(path + "\\pdf\\" + id + "value.pdf");
                System.IO.File.Delete(path + "\\pdf\\" + id + "value.pdf");
            }
            return new FileContentResult(byteArray, "application/pdf");
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
            String t = "";
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public IActionResult Report()
        {
            ViewData["Message"] = "Your Report page.";

            return View();
        }
    }
}
