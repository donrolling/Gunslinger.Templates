namespace Gunslinger.Models.SQL
{
    public class SQLForeignKey
    {
		public bool Nullable { get; set; }
		public ColumnSource Source { get; set; }
        public ColumnSource Reference { get; set; }
    }
}