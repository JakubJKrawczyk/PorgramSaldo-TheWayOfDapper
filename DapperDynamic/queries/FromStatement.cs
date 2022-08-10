namespace DapperDynamic.queries;

public class FromStatement
{
    public FromStatement (string tableDisplayName, bool isAlias = false)
    {
        
    }

    public FromStatement(SelectQuery selectQuery, string alias)
    {
        
    }
    
    public FromStatement InnerJoin(string tableDisplayName, WhereStatement on, string? alias = null)
    {
        return this;
    }
    
    public FromStatement InnerJoin(SelectQuery selectQuery, WhereStatement on, string alias)
    {
        return this;
    }
    
    public FromStatement LeftJoin(string tableDisplayName, WhereStatement on, string? alias = null)
    {
        return this;
    }
    
    public FromStatement LeftJoin(SelectQuery selectQuery, WhereStatement on, string alias)
    {
        return this;
    }
    
    public FromStatement RightJoin(string tableDisplayName, WhereStatement on, string? alias = null)
    {
        return this;
    }
    
    public FromStatement RightJoin(SelectQuery selectQuery, WhereStatement on, string alias)
    {
        return this;
    }
    
    public FromStatement FullJoin(string tableDisplayName, WhereStatement on, string? alias = null)
    {
        return this;
    }
    
    public FromStatement FullJoin(SelectQuery selectQuery, WhereStatement on, string alias)
    {
        return this;
    }
}