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
                subthread.CurrentScope = current.CurrentScope;

                var localScopeInfo = new ScopeInfo(_this, true);

                if (_this.DependentScopeInfo != null)
                    throw new Exception("Unexpected condition (AstNode has dependentScopeInfo when it shouldnt), handle this!");

                if (subthread.CurrentScope == null)
                    throw new Exception("Unexpected condition (new ScriptThread has no current scope), handle this!");

                _this.DependentScopeInfo = localScopeInfo;
                
                //subthread.CurrentScope = current.CurrentScope;                
                //subthread.PushClosureScope(localScopeInfo, subthread.CurrentScope, new object[] { });
                
                //subthread.PushScope(current.CurrentScope.Info, new object[] { });                
                subthread.PushScope(_this.DependentScopeInfo, new object[] { });

                //subthread.PushClosureScope(_this.DependentScopeInfo, current.CurrentScope, new object[] { });
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
