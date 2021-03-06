﻿using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using MultiTasks.AST;
using System;

namespace MultiTasks
{
    static class _
    {
        public static AstNode ConvertToTS(this AstNode _this)
        {
            if (_this as MtAstNode != null)
            {
                return ((MtAstNode)_this).ToTS();
            }

            return _this;
        }

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
                var localScopeInfo = new ScopeInfo(_this, true);
                
                _this.DependentScopeInfo = localScopeInfo;
                
                subthread.CurrentScope = current.CurrentScope;                
                subthread.PushClosureScope(localScopeInfo, subthread.CurrentScope, new object[] { });
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
