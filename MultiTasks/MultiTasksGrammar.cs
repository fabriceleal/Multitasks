using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony.Interpreter;
using System.Diagnostics;
using Irony.Ast;
using Irony.Interpreter.Ast;
using MultiTasks.AST;

namespace MultiTasks
{

    public class MtGrammar : InterpretedLanguageGrammar /* Grammar*/
    {

        public MtGrammar()
            : base(true)
        {
            // Terminals
            var stringLiteral = new StringLiteral("stringLiteral", "\"", StringOptions.AllowsLineBreak);
            var nbrLiteral = new NumberLiteral("nbrLiteral");
            var identifier = new IdentifierTerminal("identifier", IdOptions.IsNotKeyword);
            
            // Punctuation terminals; these shouldn't appear on the tree
            var pipe = ToTerm("|", "pipe");
            var semicomma = ToTerm(";", "semicomma");
            var openparen = ToTerm("(", "openparen");
            var closeparen = ToTerm(")", "closeparen");
            var openbracket = ToTerm("{", "openbracket");
            var closebracket = ToTerm("}", "closebracket");
            var bind = ToTerm("=>", "bind");
            var comma = ToTerm(",", "comma");
            var ift = ToTerm("if", "if");
            MarkPunctuation(pipe, semicomma, openparen, closeparen, bind, openbracket, closebracket, comma, ift);

            // Non Terminals            
            AstNodeCreator MakeExpressionNode = delegate(AstContext context, ParseTreeNode treeNode)
            {
                if (treeNode.ChildNodes.Count != 1)
                    throw new Exception("Expression expects 1 child (received {0}.)".SafeFormat(treeNode.ChildNodes.Count));
                                
                // Check child term name
                var possibleValid = treeNode.ChildNodes[0];
                var tag = possibleValid.Term.Name;              

                if (tag == "APPLICATION")
                {
                    treeNode.AstNode = _.NewAndInit<MtApplication>(context, possibleValid);
                }
                else if (tag == "ATOM")
                {
                    treeNode.AstNode = _.NewAndInit<MtAtom>(context, possibleValid);
                }
                else if (tag == "FORK")
                {
                    treeNode.AstNode = _.NewAndInit<MtFork>(context, possibleValid);
                }
                else if (tag == "BIND")
                {
                    treeNode.AstNode = _.NewAndInit<MtBind>(context, possibleValid);
                }
                else if (tag == "IF")
                {
                    treeNode.AstNode = _.NewAndInit<MtIf>(context, possibleValid);
                }
                else if (tag == "identifier")
                {
                    // Do nothing here ...
                    treeNode.AstNode = _.NewAndInit<Irony.Interpreter.Ast.IdentifierNode>(context, possibleValid);
                }
                else
                {
                    throw new Exception("Unexpected tag in Expression child: {0}".SafeFormat(tag));
                }
            };

            var TOP_CHAIN = new NonTerminal("TOP_CHAIN", delegate(AstContext context, ParseTreeNode treeNode)
            {
                if (treeNode.ChildNodes.Count != 1)
                    throw new Exception("Top chain expects 1 child (received {0}.)".SafeFormat(treeNode.ChildNodes.Count));
                
                // Check child term name
                var possibleValid = treeNode.ChildNodes[0];
                var tag = possibleValid.Term.Name;      

                if (tag == "CHAIN")
                {
                    treeNode.AstNode = _.NewAndInit<MtChain>(context, possibleValid);
                }
                else if (tag == "EXPRESSION")
                {
                    // HACK
                    MakeExpressionNode(context, possibleValid);
                    treeNode.AstNode = possibleValid.AstNode;                    
                }
                else
                {
                    throw new Exception("Unexpected tag in TOP_CHAIN child: {0}".SafeFormat(tag));
                }
            });

            var IF = new NonTerminal("IF", typeof(MtIf));
            var CHAIN = new NonTerminal("CHAIN", typeof(MtChain));
            var ATOM = new NonTerminal("ATOM", typeof(MtAtom));
            var NCHAINS = new NonTerminal("NCHAINS", typeof(MtFork));
            var APPLICATION = new NonTerminal("APPLICATION", typeof(MtApplication));
            var BIND = new NonTerminal("BIND", typeof(MtBind));
            var FORK = new NonTerminal("FORK", typeof(MtFork));

            var ARGLIST = new NonTerminal("ARGLIST", typeof(MtArguments));

            var FUNCTION = new NonTerminal("FUNCTION", delegate(AstContext context, ParseTreeNode treeNode)
            {
                if (treeNode.ChildNodes.Count != 1)
                    throw new Exception("Function expects 1 child (received {0}.)".SafeFormat(treeNode.ChildNodes.Count));

                // Check child term name
                var possibleValid = treeNode.ChildNodes[0];
                var tag = possibleValid.Term.Name;

                if (tag == "APPLICATION")
                {
                    treeNode.AstNode = _.NewAndInit<MtApplication>(context, possibleValid);
                }
                else if (tag == "identifier")
                {                    
                    // This is ugly as fuck, but works
                    Type t = identifier.AstConfig.NodeType;
                    treeNode.AstNode = t.GetConstructors()[0].Invoke(new object[]{ });
                    (treeNode.AstNode as AstNode).Init(context, possibleValid);
                    // _.NewAndInit<t>(context, possibleValid);
                }
                else
                {
                    throw new Exception("Unexpected tag in FUNCTION child: {0}".SafeFormat(tag));
                }
            });

            var EXPRESSION = new NonTerminal("EXPRESSION", MakeExpressionNode);

            Root = NCHAINS;

            /*
             * RULES
             */

            // TODO: ARGLIST 
                        
            NCHAINS.Rule = MakePlusRule(NCHAINS, TOP_CHAIN);
                        
            TOP_CHAIN.Rule = CHAIN + semicomma |
                            EXPRESSION + semicomma;
                        
            CHAIN.Rule = EXPRESSION + pipe + EXPRESSION |
                        EXPRESSION + pipe + CHAIN;

            EXPRESSION.Rule = FORK | IF | BIND | APPLICATION | ATOM | identifier;

            FORK.Rule = openbracket + NCHAINS + closebracket;

            ATOM.Rule =
                      nbrLiteral |
                      stringLiteral /*|
                      identifier*/;

            APPLICATION.Rule = FUNCTION + openparen + ARGLIST + closeparen;

            ARGLIST.Rule = MakeStarRule(ARGLIST, comma, EXPRESSION);

            FUNCTION.Rule = identifier | APPLICATION;

            BIND.Rule = identifier + bind + EXPRESSION;

            IF.Rule = ift + EXPRESSION + TOP_CHAIN + TOP_CHAIN;
        }

        public override LanguageRuntime CreateRuntime(LanguageData language)
        {
            return new MultiTasksRuntime(language);
        }
    }

    
}
