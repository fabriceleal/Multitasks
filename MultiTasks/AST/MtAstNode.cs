using Irony.Interpreter.Ast;

namespace MultiTasks.AST
{
    public abstract class MtAstNode : AstNode
    {

        public virtual AstNode ToTS()
        {
            return this;
        }

    }
}
