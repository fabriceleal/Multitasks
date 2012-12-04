using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Interpreter;
using MultiTasks.AST;
using Irony.Parsing;
using MultiTasks.RT;
using System.IO;
using System.Threading;

namespace MultiTasks
{
    public class MultiTasksRuntime : LanguageRuntime
    {
        public MultiTasksRuntime(LanguageData lang) : base(lang) { }
                
        public Stream OutputStream { get; set; }

        public override void Init()
        {
            base.Init();
                                                
            BuiltIns.AddMethod(MtPrint, "print", 1);
            BuiltIns.AddMethod(MtFalse, "false");
            BuiltIns.AddMethod(MtTrue, "true");
            BuiltIns.AddMethod(MtIdentity, "identity", 1);
            BuiltIns.AddMethod(MtSleep, "sleep", 1, 2);
            BuiltIns.AddMethod(MtAdd, "add");
        }

        public MtResult MtSleep(ScriptThread thread, object[] args)
        {
            var result = new MtResult();
            var ms = args[0] as MtResult;
            var value = args.Length > 1 ? args[1] as MtResult : MtResult.True;

            ms.GetValue((waitPeriod) =>
            {
                Thread.Sleep((int)waitPeriod.Value);               

                value.GetValue((retValue) => {
                    result.SetValue(retValue);   
                });
            });

            return result;
        }

        private void Reduce(object[] list, int i, Action<MtObject> reducer, Action finalAction)
        {
            if (i == list.Length)
            {
                finalAction();
            }
            else
            {
                var item = list[i] as MtResult;
                if (item == null)
                    throw new Exception("Argument should be a MtResult!");

                item.GetValue((a) =>
                {
                    reducer(a);
                    Reduce(list, i + 1, reducer, finalAction);
                });
            }
        }

        private void Reduce(object[] list, Action<MtObject> reducer, Action finalAction)
        {
            Reduce(list, 0, reducer, finalAction);
        }

        public MtResult MtAdd(ScriptThread thread, object[] args)
        {
            var result = new MtResult();
            var tot = 0;

            Action finalAction = () => { result.SetValue(new MtObject(tot)); };
            Action<MtObject> reducer = (MtObject a) => { tot += (int)a.Value; };

            Reduce(args, reducer, finalAction);

            return result;
        }

        public MtResult MtFalse(ScriptThread thread, object[] args)
        {
            // does nothing, just returns false            
            return MtResult.False;
        }

        public MtResult MtTrue(ScriptThread thread, object[] args)
        {
            // does nothing, just returns true
            return MtResult.True;
        }

        public MtResult MtIdentity(ScriptThread thread, object[] args)
        {
            try
            {
                var arg = args[0] as MtResult;
                if (arg == null)
                    throw new Exception("Argument cant be null!");

                return arg;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Runtime function: identity", e);
            }            
        }

        static byte[] _newLineRaw = Encoding.UTF8.GetBytes(Environment.NewLine);
        
        object _mtprintlock = new object();

        public MtResult MtPrint(ScriptThread thread, object[] args)
        {
            try
            {
                var r = new MtResult();

                lock (_mtprintlock) /* Lock all streams */
                {
                                    
                    Reduce(args, (o) => {
                                     
                        String toWrite = o == null ? "<null>" : o.ToString();

                        if (OutputStream != null)
                        {
                            byte[] raw = Encoding.UTF8.GetBytes(toWrite);
                            OutputStream.Write(raw, 0, raw.Length);                            
                        }

                        thread.App.Write(toWrite);
                    }, () => { 

                        // Write new lines
                        if (OutputStream != null)
                        {
                            OutputStream.Write(_newLineRaw, 0, _newLineRaw.Length);
                            OutputStream.Flush();
                        }

                        thread.App.WriteLine("");
                        
                        r.SetValue(MtObject.True); 
                    });
                }

                return r;
            }
            catch (Exception e)
            {
                throw new Exception("Exception on Runtime function: print", e);
            }
        }

    }
}
