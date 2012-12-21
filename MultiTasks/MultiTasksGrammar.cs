using System;
using Irony.Parsing;
using Irony.Interpreter;
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
            var stringLiteral = new StringLiteral("stringLiteral", "\"", StringOptions.AllowsLineBreak | StringOptions.AllowsAllEscapes);
            var nbrLiteral = new NumberLiteral("nbrLiteral");
            var identifier = new IdentifierTerminal("identifier", IdOptions.IsNotKeyword);

            var comment = new CommentTerminal("comment", "/*", "*/");

            // Punctuation terminals; these shouldn't appear on the tree
            var pipe = ToTerm("|", "pipe");
            var semicomma = ToTerm(";", "semicomma");
            var openparen = ToTerm("(", "openparen");
            var closeparen = ToTerm(")", "closeparen");
            var openbrace = ToTerm("{", "openbrace");
            var closebrace = ToTerm("}", "closebrace");
            var openbracket = ToTerm("[", "openbracket");
            var closebracket = ToTerm("]", "closebracket");
            var bind = ToTerm("<=", "bind");
            var argsBodySeparator = ToTerm("=>", "argsBodySeparator");
            var comma = ToTerm(",", "comma");
            var lambda = ToTerm("L", "lambda");
            var ift = ToTerm("if", "if");
            var dot = ToTerm(".", "dot");
            var flowRightToLeft = ToTerm("<-", "flowRightToLeft");
            var listenerOp = ToTerm("on", "listenerOp");
                        
            MarkPunctuation(pipe, semicomma, openparen, closeparen, 
                            openbrace, closebrace, 
                            openbracket, closebracket, bind, 
                            argsBodySeparator, comma, lambda,
                            ift, dot, flowRightToLeft, listenerOp);
            //--

            // Non Terminals            
            AstNodeCreator MakeExpressionNode = delegate(AstContext context, ParseTreeNode treeNode)
            {
                if (treeNode == null)
                    throw new Exception("treeNode cant be null at MakeExpressionNode");

                if (treeNode.ChildNodes.Count != 1)
                    throw new Exception("Expression expects 1 child (received {0}.)".SafeFormat(treeNode.ChildNodes.Count));
                
                // Check child term name
                var possibleValid = treeNode.ChildNodes[0];
                if (possibleValid == null)
                    throw new Exception("Only child cant be null at MakeExpressionNode");

                var tag = possibleValid.Term.Name;
                if (string.IsNullOrWhiteSpace(tag))
                    throw new Exception("tag cant be null or empty at MakeExpressionNode");

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
                else if (tag == "FUNCTION_LITERAL")
                {
                    treeNode.AstNode = _.NewAndInit<MtFunctionLiteral>(context, possibleValid);
                }
                else if (tag == "FLOW_RIGHT_TO_LEFT")
                {
                    treeNode.AstNode = _.NewAndInit<MtFlowRightToLeft>(context, possibleValid);
                }
                else if (tag == "ARRAY")
                {
                    treeNode.AstNode = _.NewAndInit<MtArray>(context, possibleValid);
                }
                else if (tag == "LISTENER_STATEMENT")
                {
                    treeNode.AstNode = _.NewAndInit<MtListenerStatement>(context, possibleValid);
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
                        
            var ARRAY = new NonTerminal("ARRAY", typeof(MtArray));
            var IF = new NonTerminal("IF", typeof(MtIf));
            var CHAIN = new NonTerminal("CHAIN", typeof(MtChain));
            var ATOM = new NonTerminal("ATOM", typeof(MtAtom));
            var NCHAINS = new NonTerminal("NCHAINS", typeof(MtFork));
            var APPLICATION = new NonTerminal("APPLICATION", typeof(MtApplication));
            var BIND = new NonTerminal("BIND", typeof(MtBind));
            var FORK = new NonTerminal("FORK", typeof(MtFork));

            var ARG_LIST_FOR_DECL = new NonTerminal("ARG_LIST_FOR_DECL", typeof(MtArgListForDecl));
            var FUNCTION_LITERAL = new NonTerminal("FUNCTION_LITERAL", typeof(MtFunctionLiteral));

            var EXPRESSION_LIST = new NonTerminal("EXPRESSION_LIST", typeof(MtExpressionList));

            var DOTTED_EXPRESSION = new NonTerminal("DOTTED_EXPRESSION", typeof(MtDottedExpression));

            var FLOW_RIGHT_TO_LEFT = new NonTerminal("FLOW_RIGHT_TO_LEFT", typeof(MtFlowRightToLeft));

            var LISTENER_STATEMENT = new NonTerminal("LISTENER_STATEMENT", typeof(MtListenerStatement));

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
                else if (tag == "FUNCTION_LITERAL")
                {
                    treeNode.AstNode = _.NewAndInit<MtFunctionLiteral>(context, possibleValid);
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

            // TODO: FUNCTION LITERALS / LAMBDAS
            // TODO: SWITCH
                        
            NCHAINS.Rule = MakePlusRule(NCHAINS, TOP_CHAIN);
                        
            TOP_CHAIN.Rule = CHAIN + semicomma |
                            EXPRESSION + semicomma;
                        
            CHAIN.Rule = EXPRESSION + pipe + EXPRESSION |
                        EXPRESSION + pipe + CHAIN;

            EXPRESSION.Rule = ARRAY | FORK | IF | BIND | 
                    FUNCTION_LITERAL | APPLICATION |
                    LISTENER_STATEMENT | FLOW_RIGHT_TO_LEFT | 
                    ATOM | identifier;

            FORK.Rule = openbrace + NCHAINS + closebrace;

            LISTENER_STATEMENT.Rule = identifier + listenerOp + identifier + FUNCTION;

            ATOM.Rule =
                      nbrLiteral |
                      stringLiteral /*|
                      identifier*/;

            FLOW_RIGHT_TO_LEFT.Rule = identifier + flowRightToLeft + identifier;

            APPLICATION.Rule = FUNCTION + openparen + EXPRESSION_LIST + closeparen;

            EXPRESSION_LIST.Rule = MakeStarRule(EXPRESSION_LIST, comma, EXPRESSION);

            FUNCTION.Rule = identifier | FUNCTION_LITERAL | APPLICATION;

            BIND.Rule = identifier + bind + EXPRESSION;

            IF.Rule = ift + EXPRESSION + TOP_CHAIN + TOP_CHAIN;

            ARRAY.Rule = openbracket + EXPRESSION_LIST + closebracket;

            // For function literals
            ARG_LIST_FOR_DECL.Rule = MakeStarRule(ARG_LIST_FOR_DECL, comma, identifier);

            FUNCTION_LITERAL.Rule = lambda + openparen + ARG_LIST_FOR_DECL + closeparen + argsBodySeparator + TOP_CHAIN;            
        }

        public override LanguageRuntime CreateRuntime(LanguageData language)
        {
            return new MultiTasksRuntime(language);
        }
    }

    
}
