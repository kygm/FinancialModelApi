using Newtonsoft.Json;

namespace FinancialAPI.Models
{
    public class NDQRetObj
    {
        public ObjInsNdq dataset;
        public string frequency;
        public string description;
        public DateTime start_date;
        public DateTime end_date;
        public string fequency;
    }
    public class ObjInsNdq
    {
        public int id;
        public string name; 
        public string description;
        public object data;

    }
    public class DataObj
    {
        string date;
        double price;

    }
    
    
}
