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
            try
            {
                string url = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=" + symbol + "&interval="+interval+"min&apikey=3G6Z7P7YYUSBPI4N&datatype=csv";
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