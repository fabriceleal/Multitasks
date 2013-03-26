using System;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using MultiTasks.RT;

namespace MultiTasks.AST
{
    public class MtChain : MtAstNode
    {
        private AstNode _head;
        private AstNode _tail;

        public override void Init(Irony.Ast.AstContext context, Irony.Parsing.ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            if (treeNode.ChildNodes.Count != 2)
            {
                throw new Exception("MtChain expects 1 child (received {0}.)".SafeFormat(treeNode.ChildNodes.Count));
            }

            _head = AddChild(string.Empty, treeNode.ChildNodes[0]);

            // Called with a new context, include result of head as '_'
            _tail = AddChild(string.Empty, treeNode.ChildNodes[1]);

            AsString = "Chain";
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            // PROLOG
            thread.CurrentNode = this;

            try
            {

#if true //SILVERLIGHT
                                
                // synchronous eval??
                var headResult = _head.Evaluate(thread);

                // Wrap if necessary.
                if (headResult as ICallTarget != null)
                {
                    headResult = MtResult.CreateAndWrap(headResult);
                }

                //var wrkHeadResult = headResult as MtResult;

                if (headResult as MtResult == null)
                {
                    throw new Exception("Head of chain evaluated to null!");
                }

                var subthread = _tail.NewScriptThread(thread);
                var accessor = subthread.Bind("_", BindingRequestFlags.Write | BindingRequestFlags.ExistingOrNew);
                accessor.SetValueRef(subthread, headResult);

                var _tailResult = _tail.Evaluate(subthread) as MtResult;

                if (_tailResult == null)
                {
                    throw new Exception("tail of chain evaluated to null!");
                }

                return _tailResult;

#else
                MtResult chainResult = new MtResult();

                _head.Evaluate.BeginInvoke(thread, iar =>
                {
                    var headResult = _head.Evaluate.EndInvoke(iar);
                    
                    // Wrap if necessary.
                    if (headResult as ICallTarget != null)
                    {
                        headResult = MtResult.CreateAndWrap(headResult);
                    }

                    if (headResult as MtResult == null)
                    {
                        throw new Exception("Head of chain evaluated to null!");
                    }

                    var subthread = _tail.NewScriptThread(thread);
                    var accessor = subthread.Bind("_", BindingRequestFlags.Write | BindingRequestFlags.ExistingOrNew);
                    accessor.SetValueRef(subthread, headResult);

                    var tailResult = _tail.Evaluate(subthread) as MtResult;

                    if (tailResult == null)
                    {
                        throw new Exception("tail of chain evaluated to null!");
                    }

                    tailResult.GetValue(o =>
                    {
                        chainResult.SetValue(o);
                    });

                }, null);

                return chainResult;
#endif

            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtChain.DoEvaluate.", e);
            }
            finally
            {
                // EPILOG
                thread.CurrentNode = Parent;
            }            
        }

    }
}
