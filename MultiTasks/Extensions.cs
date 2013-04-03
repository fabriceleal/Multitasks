using System;
using Irony.Interpreter.Ast;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter;

namespace MultiTasks
{
    static class _
    {

        public static ScriptThread SelectMany(this ScriptThread _this)
        {
            return _this;
        }

        public static ScriptThread NewScriptThread(this AstNode _this, ScriptThread current)
        {
            if (_this == null)
            {
                throw new NullReferenceException("NewScriptThread needs an AstNode");
            }
            if (current == null)
            {
                throw new NullReferenceException("NewScriptThread needs a current thread");
            }
            try
            {
                var subthread = new ScriptThread(current.App);
                
                //if (_this.DependentScopeInfo != null)
                //    throw new Exception("Unexpected condition (AstNode has dependentScopeInfo when it shouldnt), handle this!");

                if (subthread.CurrentScope == null)
                    throw new Exception("Unexpected condition (new ScriptThread has no current scope), handle this!");

                // subthread.CurrentScope is, at creation, the base scope which is at
                // ScriptApp (root of all scopes). We can override it like this.
                subthread.CurrentScope = current.CurrentScope;

                // We'll try to mimic the behavior of LambdaNode-FunctionCallNode
                // used by the mini-python sample in the Irony's samples
                
                // We create a new ScopeInfo.
                //var localScopeInfo = new ScopeInfo(_this, true);

                // Now we put this in the AstNode. This is needed so Scope.GetParent()
                // works properly. This is atrocious, and will force us to clone
                // a functions body each time it's evaluated
                //_this.DependentScopeInfo = localScopeInfo;

                // Now we push the new ScopeInfo.
                //subthread.PushScope(localScopeInfo, new object[] { });

                // I think this is superfluous, because I'm doing this in the begining of each
                // AstNode.DoEvaluate(), but wont touch it in the meanwhile
                subthread.CurrentNode = _this;
            
                return subthread;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on NewScriptThread.", e);
            }            
        }

        public static T NewAndInit<T>(AstContext ctx, ParseTreeNode tree)  where T : AstNode, new()
        {
            T v = new T();
            v.Init(ctx, tree);
            return v;
        }
    }
}
