using System;
using MultiTasks.RT;
using Irony.Interpreter;
using Irony.Interpreter.Ast;

namespace MultiTasks.AST
{
    public class MtFlowRightToLeft : MtAstNode
    {
        private AstNode _target;
        private AstNode _source;

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            _target = AddChild(string.Empty, treeNode.ChildNodes[0]) as AstNode;
            _source = AddChild(string.Empty, treeNode.ChildNodes[1]) as AstNode;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                var result = new MtResult();
                var targetThread = _target.NewScriptThread(thread);
                var rTarget = _target.Evaluate(targetThread) as MtResult;

                var sourceThread = _source.NewScriptThread(thread);
                var rSource = _source.Evaluate(sourceThread) as MtResult;
                
                rTarget.GetValue(target =>
                {
                    rSource.GetValue(source =>
                    {
                        MtStream.ReadFromWriteTo(
                            source.Value as MtStream, target.Value as MtStream, () => { 
                                result.SetValue(MtObject.True); 
                            });
                    });
                });
                
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtFlowRightToLeft.DoEvaluate", e);
            }
            finally
            {
                thread.CurrentNode = Parent;
            }
        }
    }
}
