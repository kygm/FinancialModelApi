namespace FinancialAPI.Controllers
{
    public class Coin
    {
        public string id { get; set; }
        public string rank { get; set; }
        public string symbol { get; set; }
        public string supply { get; set; }
        public double priceUsd { get; set; }

        public Coin(string i, string r, string sy, string su, double pri)
        {
            id = i;
            rank = r;
            symbol = sy;
            supply = su;
            priceUsd = pri;
        }
    }
}
