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
using System.Xml.Linq;
using System.Linq;

namespace WebApplication1.FileMakers
{
    public class filemakers
    {
        public string Stringify_array(dynamic response)
        {
            string borders = "";
            
            //Type tp = response.GetType();

            //if (tp.Equals(typeof(Null)))

            if(response != null)
            {
                for (var b = 0; b < response.Length; b++)
                {
                    borders = borders + "[" + checkif_ItsNull(response[b].ToString()) + "] ";
                }
            }
               
            return borders;
        }

        public string checkif_ItsNull(string text)
        {
            string t = "";
            if (text != null)
            {
                if (String.IsNullOrEmpty(text) | String.IsNullOrWhiteSpace(text))
                {
                    return t;
                }
                else
                {
                    t = text;
                }
            }

            return t; 
        }

        //to generate a random number for document name
        public int RNumber()
        {
            Random rd = new Random();

            int rand_num = rd.Next(0, 1000);

            return rand_num;
        }

        public string stringify_Object(dynamic resp)
        {

            string reply = "";
            if (resp != null)
            {
                foreach (JProperty property in resp.Properties())
                {
                    reply = reply + "[" + property.Name + ": " + checkif_ItsNull(property.Value.ToString()) + "] ";
                    // ws.Cell("B4").Value = property.Value[1];
                }
            }
                

            return reply;
        }

        public string Stringify_arrayOfObject(dynamic resp)
        {

            string ddos = "";
            if (resp != null)
            {
                for (var b = 0; b < resp.Count; b++)
                {
                    ddos = ddos + stringify_Object(resp[b]);
                }

            }

               

            return ddos;

        }

        //data to CSV
        // Instead of using an external package such as CSVHelper I prefered to make manuallly the csv file


        //as some contries can have comma in their information I removed all comma and put - to make csv
        public string removeComma(string vlue)
        {
            string value = vlue;

            if (vlue.Contains(','))
            {
                value = vlue.Replace(',', '-'); ;

            }
            return value;
        }

    }
}