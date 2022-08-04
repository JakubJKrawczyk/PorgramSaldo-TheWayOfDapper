namespace DapperDynamic.queries;

public class FromStatement
{
    public FromStatement (string tableDisplayName, bool isAlias = false)
    {
        
    }

    public FromStatement(SelectQuery selectQuery, string alias)
    {
        
    }
    
    public FromStatement InnerJoin(string tableDisplayName, string alias, WhereStatement on)
    {
        return this;
    }
    
    public FromStatement InnerJoin(SelectQuery selectQuery, string alias, WhereStatement on)
    {
        return this;
    }
    
    public FromStatement LeftJoin(string tableDisplayName, string alias, WhereStatement on)
    {
        return this;
    }
    
    public FromStatement LeftJoin(SelectQuery selectQuery, WhereStatement on)
    {
        return this;
    }
    
    public FromStatement RightJoin(string tableDisplayName, WhereStatement on)
    {
        return this;
    }
    
    public FromStatement RightJoin(SelectQuery selectQuery, WhereStatement on)
    {
        return this;
    }
    
    public FromStatement FullJoin(string tableDisplayName, WhereStatement on)
    {
        return this;
    }
    
    public FromStatement FullJoin(SelectQuery selectQuery, WhereStatement on)
    {
        return this;
    }
}