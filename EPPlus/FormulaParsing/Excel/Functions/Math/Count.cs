﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
    public class Count : HiddenValuesHandlingFunction
    {
        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
        {
            ValidateArguments(arguments, 1);
            var nItems = 0d;
            Calculate(arguments, ref nItems);
            return CreateResult(nItems, DataType.Integer);
        }

        private void Calculate(IEnumerable<FunctionArgument> items, ref double nItems)
        {
            foreach (var item in items)
            {
                if (item.Value is IEnumerable<FunctionArgument>)
                {
                    Calculate((IEnumerable<FunctionArgument>)item.Value, ref nItems);
                }
                else if (ShouldCount(item))
                {
                    nItems++;
                }
            }
        }

        private bool ShouldCount(FunctionArgument item)
        {
            if (ShouldIgnore(item))
            {
                return false;
            }
            if (item.Value == null) return false;
            if (item.Value.GetType() == typeof(int)
                ||
                item.Value.GetType() == typeof(double)
                ||
                item.Value.GetType() == typeof(decimal)
                ||
                item.Value.GetType() == typeof(System.DateTime))
            {
                return true;
            }
            return false;
        }
    }
}