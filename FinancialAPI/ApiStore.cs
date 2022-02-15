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

        class NDQRetObj
        {
            object dataset;
            string frequency;
            string description;
            DateTime start_date;
            DateTime end_date;
            string fequency;
            List<HistoricalMilkPrice> data;
        }
        public List<Commodity> GetMilkPrices()
        {
            //fetch key from encryped source??? or auth controller..?
            string key = "ayrizEM55fccZxKeAWae";
            HttpClient client = new HttpClient();
            string? result;
            try
            {
                HttpResponseMessage response = client.GetAsync("https://data.nasdaq.com/api/v3/datasets/ODA/PMILK_USD?api_key="+key).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;

                //attempt deserialization
                try
                {
                    
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


            return new List<Commodity>();
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