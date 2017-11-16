using System;

namespace <%= ProjectName %>.Utils
{
    public interface ILog
    {
        void Write(Exception ex);
    }
}