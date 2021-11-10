using System;

namespace guigubahuang
{
    public class WorkerStateChangedEventArgs : EventArgs
    {
        public bool IsBusy { get; set; }
        public WorkerStateChangedEventArgs(bool isBusy)
        {
            IsBusy = isBusy;
        }
    }
}
