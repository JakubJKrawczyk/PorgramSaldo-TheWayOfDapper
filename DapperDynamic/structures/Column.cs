using System.Drawing;

namespace DapperDynamic.structures;

public class Column
{
    public string DisplayName { get; set; }
    
    public Table Table { get; set; }
    
    public Color Color { get; set; }
    
    public Type Type { get; set; }
}