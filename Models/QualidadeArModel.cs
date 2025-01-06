namespace APIRest.Models
{
    public class QualidadeArModel
    {
        public long Id { get; set; }
        public long IdEstacao { get; set; }
        public DateTime DataHora { get; set; }
        public double NivelPm25 { get; set; }
        public double NivelPm10 { get; set; }
        public long ConfigAlertasIdConfiguracao { get; set; }
    }
}
