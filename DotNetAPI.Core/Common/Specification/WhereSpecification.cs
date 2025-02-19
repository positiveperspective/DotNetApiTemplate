using System.Linq.Expressions;

namespace DotNetAPI.Core.Common.Specification;

internal sealed class IdentityWhereSpecification<T> : WhereSpecification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        return Expression => true;
    }
}

public abstract class WhereSpecification<T>
{
    public static readonly WhereSpecification<T> All = new IdentityWhereSpecification<T>();

    public abstract Expression<Func<T, bool>> ToExpression();

    public WhereSpecification<T> Or(WhereSpecification<T> specification)
    {
        if(this == All || specification == All)
        {
            return All;
        }

        return new OrWhereSpecification<T>(this, specification);
    }

    public WhereSpecification<T> And(WhereSpecification<T> specification)
    {
        if (this == All)
        {
            return specification;
        }

        if (specification == All)
        {
            return this;
        }

        return new AndWhereSpecification<T>(this, specification);
    }

    public WhereSpecification<T> Not()
    {
        return new NotWhereSpecification<T>(this);
    }
}

public class OrWhereSpecification<T> : WhereSpecification<T>
{
    private readonly WhereSpecification<T> _left;
    private readonly WhereSpecification<T> _right;

    public OrWhereSpecification(WhereSpecification<T> left, WhereSpecification<T> right)
    {
        _right = right;
        _left = left;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        var invokedExpr = Expression.Invoke(rightExpression, leftExpression.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(leftExpression.Body, invokedExpr), leftExpression.Parameters);
    }
}

public class AndWhereSpecification<T> : WhereSpecification<T>
{
    private readonly WhereSpecification<T> _left;
    private readonly WhereSpecification<T> _right;

    public AndWhereSpecification(WhereSpecification<T> left, WhereSpecification<T> right)
    {
        _right = right;
        _left = left;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        var invokedExpr = Expression.Invoke(rightExpression, leftExpression.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(leftExpression.Body, invokedExpr), leftExpression.Parameters);
    }
}

public class NotWhereSpecification<T> : WhereSpecification<T>
{
    private readonly WhereSpecification<T> _specification;

    public NotWhereSpecification(WhereSpecification<T> specification)
    {
        _specification = specification;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> expression = _specification.ToExpression();
        UnaryExpression notExpression = Expression.Not(expression.Body);

        return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());
    }
}