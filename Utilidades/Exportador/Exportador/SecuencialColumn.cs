namespace Utilidades.Exportador
{
    public class SecuencialColumn : Column
    {
        public char? FillCharacter { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Header { get; set; }
    }
}
