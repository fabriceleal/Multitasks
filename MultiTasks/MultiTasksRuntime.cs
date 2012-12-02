using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Interpreter;
using MultiTasks.AST;
using Irony.Parsing;
using MultiTasks.RT;
using System.IO;

namespace MultiTasks
{
    public class MultiTasksRuntime : LanguageRuntime
    {
        public MultiTasksRuntime(LanguageData lang) : base(lang) { }
                
        public Stream OutputStream { get; set; }

        public override void Init()
        {
            base.Init();
            BuiltIns.AddMethod(mtprint, "print", 1);
            BuiltIns.AddMethod(mtfalse, "false");
            BuiltIns.AddMethod(mttrue, "true");
            BuiltIns.AddMethod(mtidentity, "identity", 1);
            BuiltIns.AddMethod(mtadd, "add");
        }

        public MtResult mtadd(ScriptThread thread, object[] args)
        {
            var result = new MtResult();
            var tot = 0;

            Action<int> recursiveGet = null; 
            recursiveGet = delegate(int i)
            {
                if (i == args.Length)
                {
                    result.SetValue(new MtObject(tot));
                }
                else
                {
                    var arg = args[i] as MtResult;
                    arg.GetValue((a) => {
                        tot += (int)a.Value;
                        recursiveGet(i + 1);
                    });
                }
            };

            recursiveGet(0);

            return result;
        }

        public MtResult mtfalse(ScriptThread thread, object[] args)
        {
            // does nothing, just returns false            
            return MtResult.False;
        }

        public MtResult mttrue(ScriptThread thread, object[] args)
        {
            // does nothing, just returns true
            return MtResult.True;
        }

        public MtResult mtidentity(ScriptThread thread, object[] args)
        {
            return args[0] as MtResult;
        }

        static byte[] _newLineRaw = Encoding.UTF8.GetBytes(Environment.NewLine);
        static object _mtprintlock = new object();
        public MtResult mtprint(ScriptThread thread, object[] args)
        {
            var r = new MtResult();
            var arg0 = args[0] as MtResult;
            arg0.GetValue((o) =>
            {
                String toWrite = o == null ? "<null>" : o.ToString();

                lock (_mtprintlock) /* Lock all streams */
                {
                    if (OutputStream != null)
                    {
                        byte[] raw = Encoding.UTF8.GetBytes(toWrite);
                        OutputStream.Write(raw, 0, raw.Length);
                        OutputStream.Write(_newLineRaw, 0, _newLineRaw.Length);
                    }
                    thread.App.WriteLine(toWrite);
                }
                
                r.SetValue(MtObject.True);
            });
            return r;
        }

    }
}
