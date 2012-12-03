using System;
using System.Threading;

namespace Entities
{
    public delegate void Execute();
    public class EntityThread
    {
        public event Execute Execute;
        Thread entityThread;
        DateTime Time;
        int Tick;

        public void Start(int tick)
        {
            this.Tick = tick;
            entityThread = new Thread(new ThreadStart(Run));
            entityThread.Start();
            entityThread.IsBackground = true;
        }

        bool closed = false;
        public void Close()
        {
            closed = true;
        }

        void Run()
        {
            if (closed)
                return;
            while (true)
            {
                if (DateTime.Now > Time.AddMilliseconds(Tick))
                {
                    Time = DateTime.Now;
                    try
                    {
                        Execute.Invoke();
                    }
                     catch
                    {
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}