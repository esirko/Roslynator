﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pihrtsoft.CodeAnalysis.CSharp.Analysis
{
    public class SwitchStatementAnalysisResult
    {
        public SwitchStatementAnalysisResult(SwitchStatementSyntax switchStatement)
        {
            if (switchStatement == null)
                throw new ArgumentNullException(nameof(switchStatement));

            foreach (SwitchSectionSyntax section in switchStatement.Sections)
            {
                if (CanReplaceStatementsWithBlock && CanReplaceBlockWithStatements)
                    break;

                if (!CanReplaceStatementsWithBlock)
                    CanReplaceStatementsWithBlock = SwitchStatementAnalysis.CanReplaceStatementsWithBlock(section);

                if (!CanReplaceBlockWithStatements)
                    CanReplaceBlockWithStatements = SwitchStatementAnalysis.CanReplaceBlockWithStatements(section);
            }
        }

        public bool CanReplaceStatementsWithBlock { get; }
        public bool CanReplaceBlockWithStatements { get; }
    }
}
