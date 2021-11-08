using ClosedXML.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        //Hosted web API REST Service base url
        string Baseurl = "https://restcountries.com/v2/";
        FileMakers.filemakers fm = new FileMakers.filemakers();
        // GET: Home
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //I get all data from api to show in the view method with same name.
        public async Task<ActionResult> Index()
        {

            List<country> CountryList = new List<country>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource using HttpClient
                HttpResponseMessage Res = await client.GetAsync("all");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var CoResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the country list
                    CountryList = JsonConvert.DeserializeObject<List<country>>(CoResponse);
                }
                //returning the countries list to view
                return View(CountryList);
            }
        }


        // This method |I used to return a json response that I can easly manipulate
        //iT makes easy To test in the browser console
        public async Task<JsonResult> Contries()
        {
            List<country> CountryList = new List<country>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("all");
                if (Res.IsSuccessStatusCode)
                {
                    var CoResponse = Res.Content.ReadAsStringAsync().Result;
                    CountryList = JsonConvert.DeserializeObject<List<country>>(CoResponse);
                }
                return Json(CountryList, JsonRequestBehavior.AllowGet);
            }

        }


        //TO download file
        [HttpGet]
        public virtual FileResult  GetFile(string filePath)
        {
            
             string fullPath = Path.Combine(Server.MapPath("~/Donwnloded"), filePath);
          
            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);

          
        }

        //Metodos repetitivos p/buscr 
        [HttpPost]
        public async Task<JsonResult> GetByName(string[] nomes, string typeDoc)
        {
            List<country> resp = new List<country>();
          
            for (int i = 0; i<nomes.Length; i++) {
                List<country> CountryList = new List<country>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("name/" + nomes[i]+"?fullText=true");
                    if (Res.IsSuccessStatusCode)
                    {
                        var CoResponse = Res.Content.ReadAsStringAsync().Result;
                        CountryList = JsonConvert.DeserializeObject<List<country>>(CoResponse);
                    }
                    resp.AddRange(CountryList);
                }
            }

            var fileNome = "";

            if(typeDoc=="excel")
                fileNome = MakeEXCEL(resp);

            if (typeDoc == "csv")
                fileNome = MakeCSV(resp);

            if (typeDoc == "xml")
                fileNome =MakeXML(resp);

            return Json(fileNome, JsonRequestBehavior.AllowGet);


        }


        public string MakeCSV(List<country> nomes)
        {
            string path = Server.MapPath("~/Donwnloded/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filenome = "ContriesInfo.csv";

            var filePath = Path.Combine(path, filenome);

            if (System.IO.File.Exists(filePath))
            {
                filenome = "ContriesInfo" + fm.RNumber().ToString() + ".csv";
                filePath = Path.Combine(path, filenome);
            }


            StreamWriter sw = new StreamWriter(filePath, false);
            //headers  
            sw.WriteLine("nome,capital,fronteiras");
            //rows
            for (int i = 0; i < nomes.Count; i++)
            {
                var nome = "";
                var capital = "";
                var borders = "";

                if (fm.checkif_ItsNull(nomes[i].name) != "***")
                   nome = fm.removeComma(nomes[i].name);
                if (fm.checkif_ItsNull(nomes[i].capital) != "***")
                    capital = fm.removeComma(nomes[i].capital);

                //Evita o erro de null value (Chrome console)
               // if (nomes[i].borders.Length > 0 )
                    borders = fm.removeComma(fm.Stringify_array(nomes[i].borders));

                sw.WriteLine( nome+ "," + capital + "," + borders);
            }

            sw.Close();

            return filenome;
        }

        //I used the xml.ling to generate the xmlfile
        public string MakeXML(List<country> nomes)
        {

            XDocument countriesDet = new XDocument(new XDeclaration("1.0", "UTF - 8", "yes"),
            new XElement("Countries",
            from item in nomes
            select new XElement("Country",
             new XAttribute("Name", item.name),
           // new XAttribute("Cioc", item.cioc.ToString()),
               new XElement("capital", fm.checkif_ItsNull(item.capital)),
            new XElement("TopLevelDomains",
            from it in item.topLevelDomain
            select new XElement("TopLevelDomain", it)),
            new XElement("alpha2Code", fm.checkif_ItsNull(item.alpha2Code.ToString())),
            new XElement("alpha3Code", fm.checkif_ItsNull(item.alpha3Code.ToString())),
            new XElement("callingCodes",
            from c in item.callingCodes
            select new XElement("callingCode", c)),
            new XElement("altSpellings",
            from sp in item.altSpellings
            select new XElement("altSpelling", sp)),
            new XElement("Region", fm.checkif_ItsNull( item.region)),
            new XElement("population", fm.checkif_ItsNull( item.population.ToString())),
            new XElement("Currencies",
            from cur in item.currencies
            select new XElement("Currency",
           // from cur in item.currencies
            //select
            new XAttribute("Code", fm.checkif_ItsNull(cur["code"].ToString())),
           //  from cur in item.currencies
            // select
          new XAttribute("symbol", cur["symbol"]),
           //  from cur in item.currencies
           //  select
               new XAttribute("name", fm.checkif_ItsNull(cur["name"].ToString()))
            )))));


            string path = Server.MapPath("~/Donwnloded/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            var filenome = "ContriesInfo.xml";

            var filePath = Path.Combine(path, filenome);

            if (System.IO.File.Exists(filePath))
            {
                filenome = "ContriesInfo" + fm.RNumber().ToString() + ".xml";
                filePath = Path.Combine(path, filenome);
            }

            countriesDet.Save(filePath);
            return filenome;


        }

        public string MakeEXCEL(List<country> nomes)
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

            ws.Cell("S4").Value = "currencies";
            ws.Range(ws.Cell("S4"), ws.Cell("U4")).Merge();
            ws.Range(ws.Cell("S4"), ws.Cell("U4")).Style.Fill.BackgroundColor = XLColor.YellowGreen;

            ws.Cell("V4").Value = "languages";
            ws.Range(ws.Cell("V4"), ws.Cell("Y4")).Merge();
            ws.Range(ws.Cell("V4"), ws.Cell("Y4")).Style.Fill.BackgroundColor = XLColor.RedMunsell;

            ws.Range(ws.Cell("B5"), ws.Cell("AC5")).Style.Font.Bold = true;
            ws.Range(ws.Cell("B5"), ws.Cell("AC5")).Style.Font.Italic = true;
            ws.Range(ws.Cell("B5"), ws.Cell("AC5")).Style.Font.FontSize = 14;
            ws.Cell("B5").Value = "Nome";
            ws.Cell("C5").Value = "TopLevelDomain";
            ws.Cell("D5").Value = "alpha2Code";
            ws.Cell("E5").Value = "alpha3Code";
            ws.Cell("F5").Value = "callingCodes";
            ws.Cell("G5").Value = "capital";
            ws.Cell("H5").Value = "region";
            ws.Cell("I5").Value = "subregion";
            ws.Cell("J5").Value = "population";
            ws.Cell("K5").Value = "area";
            ws.Cell("L5").Value = "nativeName";
            ws.Cell("M5").Value = "flag";
            ws.Cell("N5").Value = "latlng";
            ws.Cell("O5").Value = "altSpellings";
            ws.Cell("P5").Value = "demonym";
            ws.Cell("Q5").Value = "borders";
            ws.Cell("R5").Value = "numericCode";
            ws.Cell("S5").Value = "code";
            ws.Cell("T5").Value = "Name";
            ws.Cell("U5").Value = "Symbol";
            ws.Cell("V5").Value = "ISO369_1";
            ws.Cell("W5").Value = "ISO369_2";
            ws.Cell("X5").Value = "name";
            ws.Cell("Y5").Value = "native name";
            ws.Cell("Z5").Value = "gini";
            ws.Cell("AA5").Value = "translations";
            ws.Cell("AB5").Value = "regionalBlocs";
            ws.Cell("AC5").Value = "cioc";
            ws.Cell("AC5").Value = "Timeone";



            for (int i = 0; i < _cont.Count; i++)
            {
                var row = i + 6;
                ws.Cell(row, 2).Value = fm.checkif_ItsNull(_cont[i].name.ToString());
                ws.Cell(row, 3).Value = fm.Stringify_array(_cont[i].topLevelDomain);
                ws.Cell(row, 4).Value = fm.checkif_ItsNull(_cont[i].alpha2Code.ToString());
                ws.Cell(row, 5).Value = fm.checkif_ItsNull(_cont[i].alpha3Code.ToString());
                ws.Cell(row, 6).Value = fm.Stringify_array(_cont[i].callingCodes);
                ws.Cell(row, 7).Value = fm.checkif_ItsNull(_cont[i].capital);
                ws.Cell(row, 8).Value = fm.checkif_ItsNull(_cont[i].region.ToString());
                ws.Cell(row, 9).Value = fm.checkif_ItsNull(_cont[i].subregion.ToString());
                ws.Cell(row, 10).Value = fm.checkif_ItsNull(_cont[i].population.ToString());
                ws.Cell(row, 11).Value = fm.checkif_ItsNull(_cont[i].area.ToString());
                ws.Cell(row, 12).Value = fm.checkif_ItsNull(_cont[i].nativeName.ToString());
                ws.Cell(row, 25).Value = _cont[i].timezones.ToString();
                ws.Cell(row, 13).Value = fm.checkif_ItsNull(_cont[i].flag.ToString());
                ws.Cell(row, 14).Value = "Lat=" + _cont[i].latlng[0].ToString() + ", Lon=" + _cont[i].latlng[1].ToString();
                ws.Cell(row, 15).Value = fm.Stringify_array(_cont[i].altSpellings);
                ws.Cell(row, 16).Value = fm.checkif_ItsNull(_cont[i].demonym.ToString());
                ws.Cell(row, 17).Value = fm.Stringify_array(_cont[i].borders);
                ws.Cell(row, 18).Value = fm.checkif_ItsNull(_cont[i].numericCode.ToString());


                // I get each value of a the object to property currencies

                ws.Cell(row, 19).Value = fm.checkif_ItsNull(_cont[i].currencies[0]["code"].ToString());
                ws.Cell(row, 20).Value = fm.checkif_ItsNull(_cont[i].currencies[0]["name"].ToString());
                ws.Cell(row, 21).Value = fm.checkif_ItsNull(_cont[i].currencies[0]["symbol"].ToString());

                // I get each value of a the object to property currencies
                ws.Cell(row, 22).Value = fm.checkif_ItsNull(_cont[i].languages[0]["iso639_1"].ToString());
                ws.Cell(row, 23).Value = fm.checkif_ItsNull(_cont[i].languages[0]["iso639_2"].ToString());
                ws.Cell(row, 24).Value = fm.checkif_ItsNull(_cont[i].languages[0]["name"].ToString());
                ws.Cell(row, 25).Value = fm.checkif_ItsNull(_cont[i].languages[0]["nativeName"].ToString());

                ws.Cell(row, 26).Value = fm.checkif_ItsNull(_cont[i].gini.ToString());
              
                ws.Cell(row, 29).Value = fm.checkif_ItsNull(_cont[i].cioc);
                ws.Cell(row, 29).Value = fm.Stringify_array(_cont[i].timezones);

            }

            IXLRange range = ws.Range(ws.Cell("B2").Address, ws.Cell("W2").Address).Merge();

            string path = Server.MapPath("~/Donwnloded/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filenome = "ContriesInfo.xlsx";

            var filePath = Path.Combine(path, filenome);

            if (System.IO.File.Exists(filePath))
            {
                filenome = "ContriesInfo" + fm.RNumber().ToString() + ".xlsx";
                filePath = Path.Combine(path, filenome);
            }

            wb.SaveAs(filePath);


            return filenome;
        }
    }
}