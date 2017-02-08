﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Internal;
using Roslynator.Extensions;

namespace Roslynator.CSharp.Extensions
{
    public static class CSharpExtensions
    {
        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, expression, cancellationToken)
                .Symbol;
        }

        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            AttributeSyntax attribute,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, attribute, cancellationToken)
                .Type;
        }

        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            ConstructorInitializerSyntax constructorInitializer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, constructorInitializer, cancellationToken)
                .Type;
        }

        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, expression, cancellationToken)
                .Type;
        }

        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            SelectOrGroupClauseSyntax selectOrGroupClause,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, selectOrGroupClause, cancellationToken)
                .Type;
        }

        public static IMethodSymbol GetMethodSymbol(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, expression, cancellationToken)
                .Symbol as IMethodSymbol;
        }

        public static bool IsExplicitConversion(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            ITypeSymbol destinationType,
            bool isExplicitInSource = false)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (!destinationType.IsErrorType()
                && !destinationType.IsVoid())
            {
                Conversion conversion = semanticModel.ClassifyConversion(
                    expression,
                    destinationType,
                    isExplicitInSource);

                return conversion.IsExplicit;
            }

            return false;
        }

        public static IParameterSymbol DetermineParameter(
            this SemanticModel semanticModel,
            ArgumentSyntax argument,
            bool allowParams = false,
            bool allowCandidate = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return DetermineParameterHelper.DetermineParameter(argument, semanticModel, allowParams, allowParams, cancellationToken);
        }

        public static IParameterSymbol DetermineParameter(
            this SemanticModel semanticModel,
            AttributeArgumentSyntax attributeArgument,
            bool allowParams = false,
            bool allowCandidate = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return DetermineParameterHelper.DetermineParameter(attributeArgument, semanticModel, allowParams, allowCandidate, cancellationToken);
        }

        public static bool IsDefaultValue(
            this SemanticModel semanticModel,
            ITypeSymbol typeSymbol,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (typeSymbol.IsErrorType())
                return false;

            SyntaxKind kind = expression.Kind();

            switch (typeSymbol.SpecialType)
            {
                case SpecialType.System_Void:
                    return false;
                case SpecialType.System_Boolean:
                    return semanticModel.IsConstantValue(expression, false, cancellationToken);
                case SpecialType.System_Char:
                    return semanticModel.IsConstantValue(expression, '\0', cancellationToken);
                case SpecialType.System_SByte:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_Byte:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_Int16:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_UInt16:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_Int32:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_UInt32:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_Int64:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_UInt64:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_Decimal:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_Single:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
                case SpecialType.System_Double:
                    return semanticModel.IsConstantValue(expression, cancellationToken);
            }

            if (typeSymbol.IsEnum())
            {
                INamedTypeSymbol underlyingType = ((INamedTypeSymbol)typeSymbol).EnumUnderlyingType;

                switch (underlyingType.SpecialType)
                {
                    case SpecialType.System_SByte:
                        return semanticModel.IsConstantValue(expression, cancellationToken);
                    case SpecialType.System_Byte:
                        return semanticModel.IsConstantValue(expression, cancellationToken);
                    case SpecialType.System_Int16:
                        return semanticModel.IsConstantValue(expression, cancellationToken);
                    case SpecialType.System_UInt16:
                        return semanticModel.IsConstantValue(expression, cancellationToken);
                    case SpecialType.System_Int32:
                        return semanticModel.IsConstantValue(expression, cancellationToken);
                    case SpecialType.System_UInt32:
                        return semanticModel.IsConstantValue(expression, cancellationToken);
                    case SpecialType.System_Int64:
                        return semanticModel.IsConstantValue(expression, cancellationToken);
                    case SpecialType.System_UInt64:
                        return semanticModel.IsConstantValue(expression, cancellationToken);
                }

                Debug.Assert(false, underlyingType.SpecialType.ToString());

                return false;
            }

            if (typeSymbol.IsReferenceType)
            {
                if (kind == SyntaxKind.NullLiteralExpression)
                {
                    return true;
                }
                else
                {
                    Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                    if (optional.HasValue)
                        return optional.Value == null;
                }
            }

            if (typeSymbol.IsConstructedFrom(SpecialType.System_Nullable_T)
                && kind == SyntaxKind.NullLiteralExpression)
            {
                return true;
            }

            if (kind == SyntaxKind.DefaultExpression)
            {
                var defaultExpression = (DefaultExpressionSyntax)expression;

                TypeSyntax type = defaultExpression.Type;

                return type != null
                    && typeSymbol.Equals(semanticModel.GetTypeSymbol(type, cancellationToken));
            }

            return false;
        }

        public static bool IsConstantValue(this SemanticModel semanticModel, ExpressionSyntax expression, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

            if (optional.HasValue)
            {
                object value = optional.Value;

                if (value is int)
                {
                    return (int)value == 0;
                }
                else if (value is uint)
                {
                    return (uint)value == 0;
                }
                else if (value is sbyte)
                {
                    return (sbyte)value == 0;
                }
                else if (value is byte)
                {
                    return (byte)value == 0;
                }
                else if (value is short)
                {
                    return (short)value == 0;
                }
                else if (value is ushort)
                {
                    return (ushort)value == 0;
                }
                else if (value is long)
                {
                    return (long)value == 0;
                }
                else if (value is ulong)
                {
                    return (ulong)value == 0;
                }
                else if (value is float)
                {
                    return (float)value == 0;
                }
                else if (value is double)
                {
                    return (double)value == 0;
                }
                else if (value is decimal)
                {
                    return (decimal)value == 0;
                }
            }

            return false;
        }

        private static bool IsConstantValue(this SemanticModel semanticModel, ExpressionSyntax expression, bool value, CancellationToken cancellationToken = default(CancellationToken))
        {
            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

            if (optional.HasValue)
            {
                object constantValue = optional.Value;

                if (constantValue is bool)
                    return (bool)constantValue == value;
            }

            return false;
        }

        private static bool IsConstantValue(this SemanticModel semanticModel, ExpressionSyntax expression, char value, CancellationToken cancellationToken = default(CancellationToken))
        {
            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

            if (optional.HasValue)
            {
                object constantValue = optional.Value;

                if (constantValue is char)
                    return (char)constantValue == value;
            }

            return false;
        }

        public static ExtensionMethodInfo GetExtensionMethodInfo(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetExtensionMethodInfo(semanticModel, expression, ExtensionMethodKind.OrdinaryOrReduced, cancellationToken);
        }

        public static ExtensionMethodInfo GetExtensionMethodInfo(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            ExtensionMethodKind allowedKinds,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ISymbol symbol = GetSymbol(semanticModel, expression, cancellationToken);

            if (symbol?.IsMethod() == true)
            {
                return ExtensionMethodInfo.Create((IMethodSymbol)symbol, semanticModel, allowedKinds);
            }
            else
            {
                return default(ExtensionMethodInfo);
            }
        }
    }
}
