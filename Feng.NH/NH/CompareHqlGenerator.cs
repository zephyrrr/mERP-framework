using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Hql.Ast;

namespace Feng.NH
{
    //public static class CompareLinqExtensions
    //{
    //    public static int CompareTo(this string x, string y)
    //    {
    //        return 0;
    //    }
    //}

    public class CompareToHqlGenerator : BaseHqlGeneratorForMethod
    {
        public CompareToHqlGenerator()
        {
            SupportedMethods = new[] { 
                ReflectionHelper.GetMethodDefinition((Guid x) => x.CompareTo(Guid.Empty)), 
                ReflectionHelper.GetMethodDefinition((string x) => x.CompareTo(string.Empty)),
                ReflectionHelper.GetMethodDefinition((byte x) => x.CompareTo(0)),
                ReflectionHelper.GetMethodDefinition((int x) => x.CompareTo(0)),
                ReflectionHelper.GetMethodDefinition((long x) => x.CompareTo(0L)),
                ReflectionHelper.GetMethodDefinition((DateTime x) => x.CompareTo(DateTime.MinValue)),
                ReflectionHelper.GetMethodDefinition((char x) => x.CompareTo(char.MinValue)),
                ReflectionHelper.GetMethodDefinition((float x) => x.CompareTo(float.MinValue)),
                ReflectionHelper.GetMethodDefinition((double x) => x.CompareTo(double.MinValue)),
            };
        }

        public override NHibernate.Hql.Ast.HqlTreeNode BuildHql(System.Reflection.MethodInfo method, System.Linq.Expressions.Expression targetObject, System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments, NHibernate.Hql.Ast.HqlTreeBuilder treeBuilder, NHibernate.Linq.Visitors.IHqlExpressionVisitor visitor)
        {
            //return treeBuilder.Constant(0);
            return treeBuilder.Case(new HqlWhen[] { 
                treeBuilder.When(
                    treeBuilder.LessThan(visitor.Visit(targetObject).AsExpression(), visitor.Visit(arguments[0]).AsExpression()), 
                    treeBuilder.Constant(-1)),
                treeBuilder.When(
                    treeBuilder.GreaterThan(visitor.Visit(targetObject).AsExpression(), visitor.Visit(arguments[0]).AsExpression()), 
                    treeBuilder.Constant(1)) }, 
                treeBuilder.Constant(0));
        }
    }

    public class CompareHqlGenerator : BaseHqlGeneratorForMethod
    {
        public CompareHqlGenerator()
        {
            SupportedMethods = new[] { 
                //ReflectionHelper.GetMethodDefinition(() => Guid.Compare(Guid.Empty, Guid.Empty)), 
                ReflectionHelper.GetMethodDefinition(() => string.Compare(string.Empty, string.Empty)),
                //ReflectionHelper.GetMethodDefinition(() => byte.Compare(0, 0)),
                //ReflectionHelper.GetMethodDefinition(() => int.Compare(0, 0)),
                //ReflectionHelper.GetMethodDefinition(() => long.Compare(0L, 0L)),
                ReflectionHelper.GetMethodDefinition(() => DateTime.Compare(DateTime.MinValue, DateTime.MinValue)),
                //ReflectionHelper.GetMethodDefinition(() => char.Compare(char.MinValue, char.MinValue)),
                //ReflectionHelper.GetMethodDefinition(() => float.Compare(float.MinValue, float.MinValue)),
                //ReflectionHelper.GetMethodDefinition(() => double.Compare(double.MinValue, double.MinValue))
            };
        }

        public override NHibernate.Hql.Ast.HqlTreeNode BuildHql(System.Reflection.MethodInfo method, System.Linq.Expressions.Expression targetObject, System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments, NHibernate.Hql.Ast.HqlTreeBuilder treeBuilder, NHibernate.Linq.Visitors.IHqlExpressionVisitor visitor)
        {
            //return treeBuilder.Constant(0);
            return treeBuilder.Case(new HqlWhen[] { 
                treeBuilder.When(
                    treeBuilder.LessThan(visitor.Visit(arguments[0]).AsExpression(), visitor.Visit(arguments[1]).AsExpression()), 
                    treeBuilder.Constant(-1)),
                treeBuilder.When(
                    treeBuilder.GreaterThan(visitor.Visit(arguments[0]).AsExpression(), visitor.Visit(arguments[1]).AsExpression()), 
                    treeBuilder.Constant(1)) },
                treeBuilder.Constant(0));
        }
    }
}