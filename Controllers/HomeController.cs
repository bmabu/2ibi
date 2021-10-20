using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        //Hosted web API REST Service base url
        string Baseurl = "https://restcountries.com/v2/";
        // GET: Home
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public async Task<ActionResult> Index()
        {
            ///

            List<country> CountryList = new List<country>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("all");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var CoResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    CountryList = JsonConvert.DeserializeObject<List<country>>(CoResponse);
                }
                //returning the employee list to view
                return View(CountryList);
            }



        }


        public async Task<JsonResult> Contries()
        {
            ///

            List<country> CountryList = new List<country>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("all");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var CoResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    CountryList = JsonConvert.DeserializeObject<List<country>>(CoResponse);
                }
                //returning the employee list to view
                return Json(CountryList, JsonRequestBehavior.AllowGet);
            }



        }



        public string DownloadInXLS(List<country> nomes)
        {

            var _cont = nomes;
            // var obj = JsonConvert.DeserializeObject<List<country>>(_contries);
            // var _cont = JObject.Parse(obj.ToString());

            //var obj = JsonConvert.DeserializeObject<country>(_contries);


            //foreach (JProperty property in _cont.Properties())
            //{
            //    // ws.Cell("B4").Value = property.Value[1];
            //}
            IXLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Countries Info.");
            ws.Cell("B2").Value = "Benildo Mabunda DEV Challenge";
            ws.Cell("B2").Style.Font.Bold = true;
            ws.Cell("B2").Style.Font.FontSize = 16;
            //ws.Range("B4:Y4").Style.Font.FontSize = 14;
            ws.Cell("B4").Value = "Nome";
            ws.Cell("C4").Value = "topLevelDomain";
            ws.Cell("D4").Value = "alpha2Code";
            ws.Cell("E4").Value = "alpha3Code";
            ws.Cell("F4").Value = "callingCodes";
            ws.Cell("G4").Value = "capital";
            ws.Cell("H4").Value = "region";
            ws.Cell("I4").Value = "subregion";
            ws.Cell("J4").Value = "population";
            ws.Cell("K4").Value = "area";
            ws.Cell("L4").Value = "nativeName";
            ws.Cell("M4").Value = "flag";
            ws.Cell("N4").Value = "latlng";
            ws.Cell("O4").Value = "altSpellings";
            ws.Cell("P4").Value = "demonym";
            ws.Cell("Q4").Value = "borders";
            ws.Cell("R4").Value = "numericCode";
            ws.Cell("S4").Value = "currencies";
            ws.Cell("T4").Value = "languages";
            ws.Cell("U4").Value = "gini";
            ws.Cell("V4").Value = "translations";
            ws.Cell("X4").Value = "regionalBlocs";
            ws.Cell("W4").Value = "cioc";
            ws.Cell("Y4").Value = "TimeOne";


            for (int i = 0; i < _cont.Count; i++)
            {
                var row = i + 5;
                ws.Cell(row, 2).Value = checkif_ItsNull(_cont[i].name.ToString());
                ws.Cell(row, 3).Value = Stringify_array(_cont[i].topLevelDomain);
                ws.Cell(row, 4).Value = checkif_ItsNull(_cont[i].alpha2Code.ToString());
                ws.Cell(row, 5).Value = checkif_ItsNull( _cont[i].alpha3Code.ToString());
                ws.Cell(row, 6).Value = Stringify_array(_cont[i].callingCodes);
                ws.Cell(row, 7).Value = checkif_ItsNull(_cont[i].capital.ToString());
                ws.Cell(row, 8).Value = checkif_ItsNull( _cont[i].region.ToString());
                ws.Cell(row, 9).Value = checkif_ItsNull(_cont[i].subregion.ToString());
                ws.Cell(row, 10).Value = checkif_ItsNull(_cont[i].population.ToString());
                ws.Cell(row, 11).Value = checkif_ItsNull(_cont[i].area.ToString());
                ws.Cell(row, 12).Value = checkif_ItsNull(_cont[i].nativeName.ToString());
                ws.Cell(row, 25).Value = _cont[i].timezones.ToString();
                ws.Cell(row, 13).Value = checkif_ItsNull(_cont[i].flag.ToString());
                ws.Cell(row, 14).Value = "Lat=" + _cont[i].latlng[0].ToString() + ", Lon=" + _cont[i].latlng[1].ToString();
                ws.Cell(row, 15).Value = Stringify_array(_cont[i].altSpellings);
                ws.Cell(row, 16).Value = checkif_ItsNull(_cont[i].demonym.ToString());
                ws.Cell(row, 17).Value = Stringify_array(_cont[i].borders);
                ws.Cell(row, 18).Value = checkif_ItsNull( _cont[i].numericCode.ToString());
                ws.Cell(row, 19).Value = _cont[i].currencies.ToString();
                ws.Cell(row, 20).Value = Stringify_array(_cont[i].languages.ToString());
                ws.Cell(row, 21).Value = checkif_ItsNull(_cont[i].gini.ToString());
                ws.Cell(row, 22).Value = _cont[i].translations.ToString();
                ws.Cell(row, 23).Value = _cont[i].regionalBlocs.ToString();
                ws.Cell(row, 24).Value = Stringify_array(_cont[i].cioc.ToString());

        }  
            
                
        
        
            
            IXLRange range = ws.Range(ws.Cell("B2").Address, ws.Cell("W2").Address).Merge();
           
            string path = Server.MapPath("~/Content/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filenome = "ContriesInfo.xlsx";

            var filePath = Path.Combine(path, filenome);

            if (System.IO.File.Exists(filePath))
            {
                filePath = Path.Combine(path, "ContriesInfo" + RNumber().ToString() + ".xlsx");
            }
          
            wb.SaveAs(filePath);

            return filePath;
        }

        private int RNumber()
        {
            Random rd = new Random();

            int rand_num = rd.Next(0, 1000);

            return rand_num;
        }

        private string Stringify_array(dynamic response)
        {
            string borders = "";
            for (var b = 0; b < response.Length; b++)
            {
                borders = borders+ "[" + response[b].ToString() + "] ";
            }

            return borders;
        }

        private string checkif_ItsNull(string text)
        {
            string t = "";
            if (String.IsNullOrEmpty(text) | String.IsNullOrWhiteSpace(text))
            {
                return t;
            }
            else
            {
                return text;
            }
            

        }


        private void GetFile(string filePath)
        {
            Response.Clear();
           // Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(filePath));
            Response.TransmitFile(Server.MapPath("~/Content/" + Path.GetFileName(filePath)));
            Response.WriteFile(filePath);
            //cleanup
            Response.Flush();
            Response.End();
          //  System.IO.File.Delete(filePath);
        }


        [HttpPost]
        public async Task<JsonResult> GetByName(string[] nomes)
        {
            ///

            List<country> resp = new List<country>();
          
            for (int i = 0; i<nomes.Length; i++) {
                List<country> CountryList = new List<country>();
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("name/" + nomes[i]+"?fullText=true");
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var CoResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        CountryList = JsonConvert.DeserializeObject<List<country>>(CoResponse);
                    }

                    resp.AddRange(CountryList);
                    
                }


            }

            DownloadInXLS(resp);

            //returning the employee list to view
            return Json(resp, JsonRequestBehavior.AllowGet);
            // return resp.ToList();


        }

    }
}