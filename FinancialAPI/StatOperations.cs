namespace FinancialAPI
{
    public class StatOperations
    {
        //DO NOT ASSIGN NULLABLE DATATYPES TO THESE!
        public List<double> Values { get; set; }
        public string SecurityName { get; set; }    

        public StatOperations(List<double> vals, string sname)
        {
            this.Values = vals;
            this.SecurityName = sname;
        }

        public double CalcuateStandardDeviation()
        {
            double average = Values.Average();
            double sum = Values.Sum(d => Math.Pow(d - average, 2));
            return Math.Sqrt((sum) / (Values.Count() - 1));
        }

        public double CalculateConfidenceInterval()
        {
            throw new NotImplementedException();
           
        }



    }
}
