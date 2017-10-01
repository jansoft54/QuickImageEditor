using System;
using System.Reflection;


namespace Main.Commands
{
    class Command
    {

        private string inpath;
        private string outpath;
        private Edit edit;
        private Storage storage;
        private Other other;

        public Command()
        {
            edit = new Edit(this);
            storage = new Storage(this);
            other = new Other();
        }

        public string IPath
        {
            get => inpath;
            set => inpath = value;
        }
        public string OPath
        {
            get => outpath;
            set => outpath = value;
        }
        public Edit Edit
        {
            get => edit;
        }

        public void Update(string input)
        {
            (Mode?, string) tuple = CheckCommand(input);

            if (tuple.Item1 == null && tuple.Item2 == null)
                return;

            Mode? m = tuple.Item1;
            string cmd = tuple.Item2;
            Type t = null;
            object cmdobj = null;
            // Console.WriteLine("{0}", m != null ? "Current Mode " + m.ToString() : "Unkown Command '" + cmd + "'");

            switch (m)
            {
                case Mode.STORAGE:
                    t = typeof(Storage);
                    cmdobj = storage;
                    break;
                case Mode.EDIT:
                    t = typeof(Edit);
                    cmdobj = edit;
                    break;
                case Mode.OTHER:
                    t = typeof(Other);
                    cmdobj = other;
                    break;
                default:
                    return;
            }
            (string, object[]) args = Parse(cmd);
            MethodInfo info = t.GetMethod(args.Item1);
            if (info != null)
                try
                {
                    info.Invoke(cmdobj, args.Item2);
                }
                catch (Exception ae)
                when (ae is ArgumentException || ae is TargetParameterCountException)
                {
                    Console.WriteLine("Wrong arguments for command '{0}' see the documentation for help", args.Item1);
                }
            else
                Console.WriteLine("Unkown command you may want to see --help");

        }


        public (string, object[]) Parse(string input)
        {
            string[] splits = input.Split(' ');
            string cmd = splits[0];

            object[] Boxedargs = new object[splits.Length - 1];
            for (int i = 0; i < splits.Length - 1; i++) Boxedargs[i] = splits[i + 1];

            return (cmd, Boxedargs);
        }

        private (Mode?, string) CheckCommand(string input)
        {
            if (input.Length == 0)
                return (null, null);
            if (input.Length >= 1 && input[0].Equals('-'))
            {
                if (input.Length >= 2 && input[1].Equals('-'))
                {
                    if (input.Length >= 3 && input[2].Equals('-'))
                        return (null, input.Substring(2, input.Length - 2));

                    return (Mode.OTHER, input.Substring(2, input.Length - 2));
                }
                return (Mode.EDIT, input.Substring(1, input.Length - 1));
            }
            else
                return (Mode.STORAGE, input); ;
        }
    }
}



enum Mode : byte
{
    STORAGE, EDIT, FINISH, OTHER
}




