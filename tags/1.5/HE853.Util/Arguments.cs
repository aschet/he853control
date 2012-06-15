namespace HE853
{
    using System;

    /// <summary>
    /// Command ling parseing.
    /// </summary>
    public class Arguments
    {
        /// <summary>
        /// Constant for the short option.
        /// </summary>
        public const string Short = "/short"; 
        
        /// <summary>
        /// Gets or sets command argument.
        /// </summary>
        public string CommandText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets parsed dim value.
        /// </summary>
        public int Dim
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets parsed device code.
        /// </summary>
        public int DeviceCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the service should be used.
        /// </summary>
        public bool Service
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets status of short flag.
        /// </summary>
        public CommandStyle Style
        {
            get;
            set;
        }

        /// <summary>
        /// Parse command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public void Parse(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException();
            }

            this.ParseCommandText(args);
            this.ParseDeviceCode(args);
            this.Service = Rpc.HasServiceArg(args);
            this.ParseStyle(args);
        }

        /// <summary>
        /// Parse command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private void ParseCommandText(string[] args)
        {
            this.CommandText = args[0];
            int dim;
            bool dimOk = int.TryParse(this.CommandText, out dim);

            if (!(this.CommandText == Command.On || this.CommandText == Command.Off || dimOk))
            {
                throw new ArgumentException();
            }

            if (dimOk)
            {
                this.Dim = dim;
                if (!Command.IsValidDim(this.Dim))
                {
                    throw new ArgumentException();
                }
            }
        }

        /// <summary>
        /// Parse command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private void ParseDeviceCode(string[] args)
        {
            this.DeviceCode = int.Parse(args[1]);

            if (!Command.IsValidDeviceCode(this.DeviceCode))
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Parse command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private void ParseStyle(string[] args)
        {
            this.Style = CommandStyle.Comprehensive;

            foreach (string arg in args)
            {
                if (arg == Short)
                {
                    this.Style = CommandStyle.Short;
                    break;
                }
            }
        }
    }
}