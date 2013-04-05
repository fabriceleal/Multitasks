using Irony.Interpreter;
using Irony.Interpreter.Ast;
using MultiTasks.RT;
using System;

namespace MultiTasks.AST
{
    public class MtChain : MtAstNode
    {
        protected AstNode _head;
        protected AstNode _tail;

        public override AstNode ToTS()
        {
            var x = new MtChain();
            x._head = _head.ConvertToTS();
            x._tail = _tail.ConvertToTS();
            x.ModuleNode = ModuleNode;
            return x;
        }

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

#if SILVERLIGHT

                var result = new MtResult();
                                
                // This is the best we can do in silverlight. There is no BeginInvoke!
                ThreadPool.QueueUserWorkItem(new WaitCallback(_ => 
                {
                    // synchronous eval :(
                    var headResult = _head.Evaluate(thread);

                    // Wrap it if necessary.
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

                    var _tailResult = _tail.Evaluate(subthread) as MtResult;

                    if (_tailResult == null)
                    {
                        throw new Exception("tail of chain evaluated to null!");
                    }

                    _tailResult.GetValue(o => { result.SetValue(o); });
                }));

                return result;

#else // SILVERLIGHT

#if ALL_SYNC
                // synchronous eval :(
                var headResult = _head.Evaluate(thread);

                // Wrap it if necessary.
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

                var _tailResult = _tail.Evaluate(subthread) as MtResult;

                if (_tailResult == null)
                {
                    throw new Exception("tail of chain evaluated to null!");
                }

                return _tailResult;

#else // ALL_SYNC

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
#endif // ALL_SYNC

#endif // SILVERLIGHT

            }
            catch (Exception e)
            {
                throw new Exception("Exception on MtChain.DoEvaluate.", e);
            }
            finally
            {
                // EPILOG
                //thread.CurrentNode = Parent;
            }            
        }

    }
}
