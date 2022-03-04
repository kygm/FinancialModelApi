using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancialAPI.Data;
using FinancialAPI.Models;
using System.Net;
using System.Text.Json;
using ServiceStack;
using ServiceStack.Text;
using Newtonsoft.Json;
using HtmlAgilityPack;


namespace FinancialAPI
{
    public enum IntervalEnums
    {
        Five = 5,
        Fifteen = 15,
        Thirty = 30,
    }
    public class ApiStore
    {
        public List<Stock> Stocks { get; set; }

        public List<Commodity> Commodity {  get; set; }

        public Dictionary<string,double> GetMilkPrices()
        {
            //fetch key from encryped source??? or auth controller..?

            string key = "ayrizEM55fccZxKeAWae";
            HttpClient client = new HttpClient();
            string? result;
            var values = new Dictionary<string, double>();
            var returnObj = new NDQRetObj();
            string uri = "https://data.nasdaq.com/api/v3/datasets/ODA/PMILK_USD.json?api_key=" + key;
            try
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;

                //attempt deserialization
                try
                {
                    returnObj = JsonConvert.DeserializeObject<NDQRetObj>(result);

                    System.Diagnostics.Debug.WriteLine(returnObj.dataset.data);

                    var objs = GenKvp(returnObj.dataset.data);

                    List<object> GenKvp(object data)
                    {
                        var json = JsonConvert.SerializeObject(data);


                        try
                        {
                            var j = JsonConvert.DeserializeObject<List<object>>(json);

                            string a = JsonConvert.SerializeObject(j);
                            System.Diagnostics.Debug.WriteLine(j);

                            return j;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.ToString());
                            return null;
                        }
         
                    }
                    //sto. most recent price first!
                    var html = new HtmlWeb();
                    var document = html.Load("https://www.investing.com/commodities/class-iii-milk-futures-historical-data");
                    var node = document.DocumentNode.SelectNodes("//*[@id=\"quotes_summary_secondary_data\"]/div/ul/li[1]/span[2]");
                    values.Add(DateTime.Now.ToString("d"), node[0].InnerText.ToDouble());
                    foreach (var i in objs)
                    {
                        System.Diagnostics.Debug.WriteLine(i);
                        string dump = JsonConvert.SerializeObject(i);
                        dump = dump.Replace("[", "{");
                        dump = dump.Replace("]", "}");
                        dump = dump.Replace(@"\", "");

                        string date = dump.Split('"', '"')[1];
                        double price = dump.Split(',', '}')[1].ToDouble();
                        values.Add(date, price);

                    }
                    

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }


            return values;
        }
        public List<Stock> GetCurrentStocks(string symbol, int interval)
        {
            IntervalEnums dur = new IntervalEnums();
            if(interval == 5)
            {
                dur = IntervalEnums.Five;
            }
            else if(interval == 15)
            {
                dur = IntervalEnums.Fifteen;
            }
            else
            {
                dur = IntervalEnums.Thirty;
            }

            //NOTE All durations result in 100 stock objects
            //As duration increases, amount of days increases proportionally

            //reading from file
            string key = "";
            try
            {
                key = System.IO.File.ReadAllText(@"C:\Users\ricke\Desktop\avKey.txt");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            if(key.IsNullOrEmpty())
            {
                return null;
            }    
            try
            {
                string url = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=" + symbol + "&interval="+interval+"min&apikey=" + key +"&datatype=csv";
                var prices = url.GetStringFromUrl().FromCsv<List<Stock>>();
                return prices;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }

        }
    }
}

