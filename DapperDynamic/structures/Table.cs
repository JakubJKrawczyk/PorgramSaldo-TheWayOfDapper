namespace DapperDynamic.structures;

public class Table
{
    public string DisplayName { get; set; }
    
    public ICollection<Column> Columns { get; set; }
}