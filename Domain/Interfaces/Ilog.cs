using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface ILogRepository
    {
        public void WriteLog(string message);

        public void WriteLog(Exception e);


    }
}
