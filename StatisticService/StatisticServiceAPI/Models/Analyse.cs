using StatisticServiceAPI.Models.Enums; 

namespace StatisticServiceAPI.Models
{
    public class Analyse
    {
        public Analyse(AnalysisType analysisType, DateTime periodStart, DateTime periodEnd)
        {
            AnalysisType = analysisType;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
        }

        public Guid Id { get; init; } = Guid.NewGuid();
        public AnalysisType AnalysisType { get; private set; }
        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }
        public double Result { get; private set; }
        public string Comparison { get; private set; } = string.Empty;
        public DateTime CalculatedAt { get; private set; }

        //JBS: Vi kører en analyse for en given type og periode
        public void RunAnalysis(AnalysisType type, DateTime period)
        {
            AnalysisType = type;
            PeriodStart = period;
            CalculatedAt = DateTime.UtcNow;
        }

        //JBS: Her sammenligner vi med den forrige periode
        public void CompareWithPrevious()
        {
            Comparison = $"Sammenlignet med forrige periode: {PeriodStart}";
        }
    }
}