using System;
using <%= ProjectName %>.Models;
using <%= ProjectName %>.Repositories;

namespace <%= ProjectName %>.Utils
{
    public class DbLog : ILog
    {
        IRepository _repository;

        public DbLog(IRepository repository)
        {
            _repository = repository;
        }

        public void Write(Exception ex)
        {
            try
            {
                _repository.Create<ErrorLog>(new ErrorLog(){
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Date = DateTime.UtcNow
                });
            }
            catch
            {
                //
            }
        }
    }

    public class ErrorLog: Entity
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Date { get; set; }
    }
}