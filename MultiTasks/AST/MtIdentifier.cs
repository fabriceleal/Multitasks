using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using System;

namespace MultiTasks.AST
{
    public class MtIdentifier : MtAstNode
    {

        protected AstNode _id;

        public override AstNode ToTS()
        {
            var i = new MtIdentifier();
            i._id = _id;
            i.ModuleNode = ModuleNode;
            return i;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.ChildNodes;

            if (nodes.Count != 1)
            {
                throw new Exception("Unexpected number of child nodes: " + nodes.Count);
            }

            _id = AddChild(string.Empty, nodes[0]) as AstNode;
            if (_id == null)
                throw new Exception("No identifier?!");

            AsString = _id.AsString;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                // No NewScriptThread!!!
                return _id.Evaluate(thread);
            }
            catch (Exception e)
            {
                throw new Exception("Error evaluating identifier", e);
            }
            finally
            {
                thread.CurrentNode = Parent;
            }            
        }

    }
}
