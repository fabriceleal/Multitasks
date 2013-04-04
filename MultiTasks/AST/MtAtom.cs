using System;
using MultiTasks.RT;

namespace MultiTasks.AST
{
    public class MtAtom : MtAstNode
    {
        private object _value;

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            _value = treeNode.ChildNodes[0].Token.Value;
            AsString = "Atom";
        }
        
        protected override object DoEvaluate(Irony.Interpreter.ScriptThread thread)
        {
            // PROLOG
            thread.CurrentNode = this;
            try
            {
                // Atoms are synchronous, and evaluate to MtObjects
                return MtResult.CreateAndWrap(_value);
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtAtom.", e);
            }
            finally
            {
                // EPILOG
                //thread.CurrentNode = Parent;
            }
        }

    }
}
