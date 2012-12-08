using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter;
using System;
using MultiTasks.RT;
using Irony.Interpreter.Ast;

namespace MultiTasks.AST
{
    public class MtListenerStatement : MtAstNode
    {
        private AstNode _eventEmitter;
        private string _eventName;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            _eventEmitter = AddChild(string.Empty, treeNode.ChildNodes[0]) as AstNode;

            _eventName = (treeNode.ChildNodes[1].AstNode as AstNode).AsString;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            try
            {
                var ret = new MtResult();

                var subthread = _eventEmitter.NewScriptThread(thread);
                var emitter = _eventEmitter.Evaluate(subthread) as MtResult;
                emitter.GetValue(o =>
                {
                    // Add listener
                    var wrkO = o.Value;

                    var wrkEmitter = wrkO as IEventEmitter;
                    if (wrkEmitter  != null)
                    {
                        wrkEmitter.On("context", args => 
                        {
                            
                        });
                    }

                    ret.SetValue(o);
                });
                
                return ret;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on listener statement evaluate", e);
            }
            finally
            {
                thread.CurrentNode = Parent;
            }
        }
    }
}
