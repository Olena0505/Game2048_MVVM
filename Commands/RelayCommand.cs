using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2048.Commands.Base;

namespace _2048.Commands
{
    public class RelayCommand : BaseCommand
    {
        public readonly Action execute;

        public RelayCommand(Action execute)
        {
            this.execute = execute;
        }
        public override void Execute(object parameter)
        {
            execute?.Invoke();
        }
    }
}
